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
                popularPosts = _dbContext.Dishes.AsEnumerable().OrderByDescending(n => n.Likes).Take(8).ToList();
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

            ViewBag.Difficulty = ((DifficultyEnum[])Enum.GetValues(typeof(DifficultyEnum))).Select(c => new SelectListItem() { Value = Convert.ToString((int)c), Text = c.ToString() }).ToList();

           ViewBag.CookingMethod = ((CookingMethodEnum[])Enum.GetValues(typeof(CookingMethodEnum))).Select(c => new SelectListItem() { Value = Convert.ToString((int)c), Text = c.ToString() }).ToList();
            if (input.CookingMethod == 0 && (input.CookingTime == null || input.CookingTime=="null") && input.Difficulty == 0 && input.RecipeName == null && input.SubCatId == 0 && string.IsNullOrEmpty(input.Tag))
            {
                var searchInput = new SearchRecipeInput();
                searchInput.Dishes = (from dishes in _dbContext.Dishes
                                      select dishes).ToList();
                List<string> tags = new List<string>();
                tags.AddRange(searchInput.Dishes.Select(s=>s.DishName));
                tags.AddRange(searchInput.Dishes.Select(s=>s.Tags));
                List<KeyValuePair<string, string>> attr = new List<KeyValuePair<string, string>>();
                attr.Add(new KeyValuePair<string, string>("title", "Foodzfame Recipes"));
                attr.Add(new KeyValuePair<string, string>("url", "https://foodzfame.com/Recipe"));
                attr.Add(new KeyValuePair<string, string>("image", "https://foodzfame.com/img/bg1.jpg"));
                attr.Add(new KeyValuePair<string, string>("type", "website"));
                attr.Add(new KeyValuePair<string, string>("description", "Search recipes with different filters, helps in finding the right recipe for you. Be it category wise, hashtags, difficulty or preparation time."));
                attr.Add(new KeyValuePair<string, string>("site_name", "foodzfame.com"));
                attr.Add(new KeyValuePair<string, string>("locale", "en_US"));
                ViewBag.MetaTag = Utils.RecipeMetaTags(tags.Distinct().ToArray(), "Search recipes with different filters, helps in finding the right recipe for you. Be it category wise, hashtags, difficulty or preparation time.",attr);
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
                                   select dishes).ToList();
                List<string> tags = new List<string>();
                tags.AddRange(filtered.Dishes.Select(s => s.DishName));
                tags.AddRange(filtered.Dishes.Select(s => s.Tags));
                ViewBag.MetaTag = Utils.RecipeMetaTags(tags.Distinct().ToArray(), "Search recipes with different filters, helps in finding the right recipe for you. Be it category wise, hashtags, difficulty or preparation time.");
                return View("Recipes", filtered);
            }
        }
        public IActionResult Recipe(int? id)
        {
            var _dish= _dbContext.Dishes
                .Include(x => x.DishCategory)
                .Include(x => x.Reviews)
                .Include(x => x.DishIngMaps)
                .ThenInclude(y => y.Ing)
                .FirstOrDefault(z => z.Id == id);
            ViewBag.Dish = _dish;
            List<string> tags = new List<string>();
            tags.Add(_dish.DishName);
            tags.Add(string.Join(" ",_dish.DishCategory.Category,_dish.CookingMethod,_dish.CookingTime));
            tags.Add(_dish.Tags);
            List<KeyValuePair<string, string>> attr = new List<KeyValuePair<string, string>>();
            attr.Add(new KeyValuePair<string, string>("title",_dish.DishName));
            attr.Add(new KeyValuePair<string, string>("url","https://foodzfame.com/Recipe/Recipe/"+id.ToString()));
            var base64 = Convert.ToBase64String(_dish.Img);
            var imgSrc = String.Format("data:image/svg;base64,{0}", base64);
            attr.Add(new KeyValuePair<string, string>("image", imgSrc));
            attr.Add(new KeyValuePair<string, string>("type", "article"));
            attr.Add(new KeyValuePair<string, string>("description", _dish.Desc));
            attr.Add(new KeyValuePair<string, string>("site_name", "foodzfame.com"));
            attr.Add(new KeyValuePair<string, string>("locale", "en_US"));
            ViewBag.MetaTag = Utils.RecipeMetaTags(tags.Distinct().ToArray(), _dish.Desc, attr);
            return View("Index");
        }
        public IActionResult AddLike(int id)
        {
            var recipe = _dbContext.Dishes.FirstOrDefault(x => x.Id == id);
            recipe.Likes = (recipe.Likes==null?0:recipe.Likes) + 1;
            _dbContext.Dishes.Update(recipe);
            _dbContext.SaveChanges();
            return Json(new { recipe.Likes , link=string.Concat("<a style='color:white;' href='/Recipe/Recipe/",recipe.Id, "'>","Someone has liked ",recipe.DishName,"</a>") });
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
            var recipe = _dbContext.Dishes.FirstOrDefault(x => x.Id == id);
            return Json(new {msg= "Success",link= string.Concat("<a style='color:white;' href='/Recipe/Recipe/", recipe.Id, "'>", reviewerName, " has reviewed ", recipe.DishName, "</a>") });
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
            var dish = _dbContext.Dishes.Where(x => x.Id == id);
            var galry = new List<GalleryModel>();
            galry.Add(new GalleryModel() { 
                    DishId=dish.FirstOrDefault().Id,
                    recipeName=dish.FirstOrDefault().DishName,
                    Img= dish.FirstOrDefault().Img,
                    Title=dish.FirstOrDefault().DishName
            });
            var galries = (from recipe in dish
                           join gallery in _dbContext.Galleries.Where(x => x.DishId == id)
                               on recipe.Id equals gallery.DishId
                             select new GalleryModel
                             {
                                 Id= gallery.Id,
                                 DishId = recipe.Id,
                                 recipeName = recipe.DishName,
                                 Img = gallery.Img,
                                 Title = gallery.Title,
                                 Tags=recipe.Tags
                             }).Distinct().ToList();
            if (galries != null && galries.Count > 0)
                galry.AddRange(galries);
            ViewBag.Gallery = galry;
            List<string> tags = new List<string>();
            tags.AddRange(galries.Select(s=>s.recipeName));
            tags.AddRange(galries.Select(s=>s.Title));
            tags.AddRange(galries.Select(s => s.Tags));
            ViewBag.MetaTag = Utils.RecipeMetaTags(tags.Distinct().ToArray(), dish.FirstOrDefault().DishName+" Image gallery");
            return View();
        }

    }
}
