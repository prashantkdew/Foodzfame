using Foodzfame.Data.Entities;
using Foodzfame.Data.FoodzfameContext;
using Foodzfame.Utility;
using Foodzfame2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foodzfame2.Controllers
{
    public class CategoryController : Controller
    {
        IDistributedCache _memoryCache;
        FoodzfameContext _dbContext;
        public CategoryController(IDistributedCache distributedCache, FoodzfameContext context)
        {
            _memoryCache = distributedCache;
            _dbContext = context;
        }
        public IActionResult Index(CategoryWise input)
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
            //input.Category = 6;//for testing
            //input.SubCategory = 6;//for testing
            if(input.Category==0 & input.SubCategory==0)
            {
                input.Categories = _dbContext.Categories.ToList();
                input.Dishes = _dbContext.Dishes.Take(9).ToList();
            }
            else if(input.Category != 0 & input.SubCategory == 0)
            {
                ViewBag.CategoryName = _dbContext.Categories.FirstOrDefault(x => x.Id == input.Category).Category1;
                input.SubCategories = _dbContext.SubCategories.Where(x => x.CategoryId == input.Category).ToList();
                input.Dishes = (from dish in _dbContext.Dishes.ToList()
                                join subcat in input.SubCategories
                                on dish.DishCategoryId equals subcat.Id
                                select dish).Take(9).ToList();

            }
            else
            {
                ViewBag.CategoryName = _dbContext.Categories.FirstOrDefault(x => x.Id == input.Category).Category1;
                input.SubCategories = _dbContext.SubCategories.Where(x => x.CategoryId == input.Category).ToList();
                ViewBag.SubCategoryName = _dbContext.SubCategories.FirstOrDefault(x => x.Id == input.SubCategory).Category;
                input.Dishes = _dbContext.Dishes.Where(x => x.DishCategoryId == input.SubCategory).Take(9).ToList();
            }
            ViewBag.CategoryWise = input;
            return View();
        }
        public IActionResult Search(string category)
        {
            var cat = _dbContext.Categories.Where(x => x.Category1.Contains(category)).ToList();
            if(cat!=null && cat.Count>0)
            {
                CategoryWise input = new CategoryWise();
                input.Category = cat.FirstOrDefault().Id;
                return RedirectToAction("Index",input);
            }
            else
            {
                var subCat = _dbContext.SubCategories.Include(x=>x.CategoryNavigation).Where(x => x.Category.Contains(category)).ToList();
                if(subCat!=null && subCat.Count>0)
                {
                    CategoryWise input = new CategoryWise();
                    input.SubCategory = subCat.FirstOrDefault().Id;
                    input.Category = subCat.FirstOrDefault().CategoryNavigation.Id;
                    return RedirectToAction("Index", input);
                }
                return RedirectToAction("Index");
            }
        }
    }
}
