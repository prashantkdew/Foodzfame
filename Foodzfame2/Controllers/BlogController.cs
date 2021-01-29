using Foodzfame.Data.Entities;
using Foodzfame.Data.FoodzfameContext;
using Foodzfame.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foodzfame2.Controllers
{
    public class BlogController : Controller
    {
        IDistributedCache _memoryCache;
        FoodzfameContext _dbContext;
        public BlogController(IDistributedCache distributedCache, FoodzfameContext context)
        {
            _memoryCache = distributedCache;
            _dbContext = context;
        }
        public IActionResult Index(string search)
        {
            if(!string.IsNullOrEmpty(search))
            {
                ViewBag.search = search;
            }
            else
            {
                ViewBag.search = "_";
            }
            var mostPopular = _memoryCache.Get("PopularPosts");
            var popularPosts = new List<Dish>();
            if (mostPopular == null)
            {
                popularPosts = _dbContext.Dishes.AsEnumerable().OrderByDescending(n => n.Likes).Take(8).ToList();
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(7)
                };
                _memoryCache.Set("PopularPosts", Utils.ObjectToByteArray(popularPosts), cacheOptions);
            }
            ViewBag.popularPosts = (List<Dish>)Utils.ByteArrayToObject(_memoryCache.Get("PopularPosts"));

            ViewBag.Blogs = _dbContext.Blogs.Include(x => x.BlogPosts)
                            .Where(x=>x.Title.Contains(search) || string.IsNullOrEmpty(search)).ToList();
            return View();
        }
        public IActionResult Post(int? id)
        {
            var mostPopular = _memoryCache.Get("PopularPosts");
            var popularPosts = new List<Dish>();
            if (mostPopular == null)
            {
                popularPosts = _dbContext.Dishes.AsEnumerable().OrderByDescending(n => n.Likes).Take(12).ToList();
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(7)
                };
                _memoryCache.Set("PopularPosts", Utils.ObjectToByteArray(popularPosts), cacheOptions);
            }
            ViewBag.popularPosts = (List<Dish>)Utils.ByteArrayToObject(_memoryCache.Get("PopularPosts"));
            if (id == null || id == 0)
                RedirectToAction("Index");
            ViewBag.Post = _dbContext.Blogs.Include(x=>x.BlogPosts).FirstOrDefault(x => x.Id == id.Value);
            return View();
        }
    }
}
