using Foodzfame.Data.Entities;
using Foodzfame.Data.Enums;
using Foodzfame.Data.FoodzfameContext;
using Foodzfame.Utility;
using Foodzfame2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc.Html;

namespace Foodzfame2.Controllers
{
    public class RecipeController : Controller
    {
        IDistributedCache _memoryCache;
        FoodzfameContext _dbContext;
        public RecipeController(IDistributedCache distributedCache, FoodzfameContext context)
        {
            _memoryCache = distributedCache;
            _dbContext = context;
        }
        public IActionResult Index(SearchRecipeInput input)
        {
            var mostPopular = _memoryCache.Get("PopularPosts");
            var popularPosts = new List<Dish>();
            if (mostPopular == null)
            {
                popularPosts = _dbContext.Dishes.AsEnumerable().OrderByDescending(n => n.Likes).Take(3).ToList();
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(7)
                };
                _memoryCache.Set("PopularPosts", Utils.ObjectToByteArray(popularPosts), cacheOptions);
            }
            ViewBag.popularPosts = (List<Dish>)Utils.ByteArrayToObject(_memoryCache.Get("PopularPosts"));
            ViewBag.SubCategory = (from subcatgry in _dbContext.SubCategories
                                       select new SelectListItem
                                       {
                                           Text = subcatgry.Category,
                                           Value = subcatgry.Id.ToString()
                                       }).ToList();
            ViewBag.CookingTime = (from recipe in _dbContext.Dishes
                                   select new SelectListItem
                                   {
                                       Text = recipe.CookingTime,
                                       Value = recipe.CookingTime
                                   }).Distinct().ToList();

            ViewBag.Difficulty = (from lst in EnumHelper.GetSelectList(typeof(DifficultyEnum))
                                  select new SelectListItem
                                  {
                                      Text = lst.Text,
                                      Value = lst.Value
                                  }).ToList();
            ViewBag.CookingMethod = (from lst in EnumHelper.GetSelectList(typeof(CookingMethodEnum))
                                     select new SelectListItem
                                     {
                                         Text = lst.Text,
                                         Value = lst.Value
                                     }).ToList();
            if (input.CookingMethod == 0 && (input.CookingTime == null || input.CookingTime=="null") && input.Difficulty == 0 && input.RecipeName == null && input.SubCatId == 0 && string.IsNullOrEmpty(input.Tag))
            {
                var searchInput = new SearchRecipeInput();
                searchInput.Dishes = (from dishes in _dbContext.Dishes
                                      select dishes).Take(9).ToList();
                return View("Recipes", searchInput); 
            }
            else
            {
                var filtered = new SearchRecipeInput();
                filtered.Dishes = (from dishes in _dbContext.Dishes
                                   where (dishes.DishName.Contains(input.RecipeName) || input.RecipeName == null)
                                   && (dishes.DishCategoryId == input.SubCatId || input.SubCatId == 0)
                                   && (dishes.CookingMethod == input.CookingMethod || input.CookingMethod == 0)
                                   && (dishes.RecipeDifficulty == input.Difficulty || input.Difficulty == 0)
                                   && (dishes.CookingTime == input.CookingTime || (input.CookingTime == null || input.CookingTime == "null"))
                                   && (dishes.Tags.Contains(input.Tag) || string.IsNullOrEmpty(input.Tag))
                                   select dishes).Take(9).ToList();
                return View("Recipes", filtered);
            }
        }
        [OutputCache(Duration = 7200)]
        public IActionResult Recipe(int? id)
        {
            ViewBag.Dish = _dbContext.Dishes
                .Include(x => x.DishCategory)
                .Include(x => x.Reviews)
                .Include(x => x.DishIngMaps)
                .ThenInclude(y => y.Ing)
                .FirstOrDefault(z => z.Id == id);
            return View("Index");
        }
        public IActionResult AddLike(int id)
        {
            var recipe = _dbContext.Dishes.FirstOrDefault(x => x.Id == id);
            recipe.Likes = (recipe.Likes==null?0:recipe.Likes) + 1;
            _dbContext.Dishes.Update(recipe);
            _dbContext.SaveChanges();
            return Json(recipe.Likes);
        }
        public IActionResult AddReview(string reviewerName,string reviewTitle,string review, int id,string rating )
        {
            var _ = new Review()
            {
                DishId = id,
                ReviewerName = reviewerName,
                Review1 = review,
                Rating = Convert.ToByte(rating),
                ReviewTitle=reviewTitle,
                ReviewDate=DateTime.Now
            };
            _dbContext.Reviews.Add(_);
            _dbContext.SaveChanges();
            return Json("Success");
        }
        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            var dish = (from _dish in _dbContext.Dishes
                        where _dish.DishName.StartsWith(prefix)
                        select new
                        {
                            label = _dish.DishName,
                            val = _dish.Id
                        }).Take(5).ToList();
            return Json(dish);
        }
        [HttpGet]
        public JsonResult GetDishes()
        {
            var dish = (from _dish in _dbContext.Dishes
                        select new
                        {
                            label = _dish.DishName,
                            val = _dish.Id
                        }).ToList();
            return Json(dish);
        }

        public IActionResult Gallery(int id)
        {
            if(id==0)
            {
                return RedirectToAction("Index");
            }
            var mostPopular = _memoryCache.Get("PopularPosts");
            var popularPosts = new List<Dish>();
            if (mostPopular == null)
            {
                popularPosts = _dbContext.Dishes.AsEnumerable().OrderByDescending(n => n.Likes).Take(3).ToList();
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(7)
                };
                _memoryCache.Set("PopularPosts", Utils.ObjectToByteArray(popularPosts), cacheOptions);
            }
            ViewBag.popularPosts = (List<Dish>)Utils.ByteArrayToObject(_memoryCache.Get("PopularPosts"));
            ViewBag.Gallery = (from recipe in _dbContext.Dishes.Where(x => x.Id == id)
                               join gallery in _dbContext.Galleries.Where(x => x.DishId == id)
                               on recipe.Id equals gallery.DishId
                             select new GalleryModel
                             {
                                 DishId = recipe.Id,
                                 recipeName = recipe.DishName,
                                 Img = gallery.Img,
                                 Title = gallery.Title
                             }).Distinct().ToList();

            return View();
        }
    }
}
