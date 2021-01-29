using Foodzfame.Data.Entities;
using Foodzfame.Data.FoodzfameContext;
using Foodzfame.Utility;
using Foodzfame2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Foodzfame2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IDistributedCache _memoryCache;
        FoodzfameContext _dbContext;
        public HomeController(ILogger<HomeController> logger, IDistributedCache distributedCache, FoodzfameContext context)
        {
            _logger = logger;
            _memoryCache = distributedCache;
            _dbContext = context;
        }
        [OutputCache(Duration = 14400)]
        public IActionResult Index()
        {
            var mostPopular = _memoryCache.Get("PopularPosts");
            var popularPosts = new List<Dish>();
            if (mostPopular==null)
            {
                popularPosts = _dbContext.Dishes.AsEnumerable().OrderByDescending(n => n.Likes).Take(8).ToList();
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(7)
                };
                _memoryCache.Set("PopularPosts",Utils.ObjectToByteArray(popularPosts), cacheOptions);              
            }
            ViewBag.popularPosts = (List<Dish>)Utils.ByteArrayToObject(_memoryCache.Get("PopularPosts"));
            ViewBag.recipeByCategory = _dbContext.Dishes.Include(x=>x.DishCategory).AsEnumerable().OrderByDescending(n => Guid.NewGuid()).Take(12).ToList();
            var Category = _dbContext.Categories.Include(x => x.SubCategories).ToList();
            var Counters = new Counters();
            Counters.counts = new List<CategoryRecipeCounts>();
            foreach (var item in Category)
            {
                int counter = 0;
                foreach(var subItem in item.SubCategories)
                {
                    counter += _dbContext.SubCategories.Include(x => x.Dishes).FirstOrDefault(x => x.Id == subItem.Id).Dishes.ToList().Count();
                }
                
                Counters.counts.Add(new CategoryRecipeCounts()
                {
                    SubCatCount = item.SubCategories.ToList().Count(),
                    CategoryName=item.Category1,
                    RecipeCount= counter,
                    CategoryId=item.Id

                });
            }
            ViewBag.Category = Counters;
            ViewBag.Blogs = _dbContext.Blogs.Take(4).ToList();
            return View();
        }
        [OutputCache(Duration = 604800)]
        public IActionResult Privacy()
        {
            return View();
        }
        [OutputCache(Duration = 604800)]
        public IActionResult About()
        {
            var Category = _dbContext.Categories.Include(x => x.SubCategories).ToList();
            var Counters = new Counters();
            Counters.counts = new List<CategoryRecipeCounts>();
            foreach (var item in Category)
            {
                int counter = 0;
                foreach (var subItem in item.SubCategories)
                {
                    counter += _dbContext.SubCategories.Include(x => x.Dishes).FirstOrDefault(x => x.Id == subItem.Id).Dishes.ToList().Count();
                }

                Counters.counts.Add(new CategoryRecipeCounts()
                {
                    SubCatCount = item.SubCategories.ToList().Count(),
                    CategoryName = item.Category1,
                    RecipeCount = counter,
                    CategoryId = item.Id

                });
            }
            ViewBag.Category = Counters;
            return View();
        }
        public IActionResult Subscribe(string email)
        {
            if(_dbContext.SubScriptions.Any(x=>x.Email==email))
            {
                return Json(0);
            }
            _dbContext.SubScriptions.Add(
                new SubScription() { 
                Email=email,
                AddedDate=DateTime.Now
                }
                );
            _dbContext.SaveChanges();
            return Json(1);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
