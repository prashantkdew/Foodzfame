using Foodzfame.Data.Entities;
using Foodzfame.Data.FoodzfameContext;
using Foodzfame.Utility;
using Foodzfame2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foodzfame2.Controllers
{
    public class AdminController : Controller
    {
        FoodzfameContext _dbContext;
        public AdminController(FoodzfameContext context)
        {
            _dbContext = context;
        }
        public IActionResult Index()
        {
            TempData["SubCategory"] = (from subcatgry in _dbContext.SubCategories
                                    select new SelectListItem
                                    {
                                        Text=subcatgry.Category,
                                        Value=subcatgry.Id.ToString()
                                    }).ToList();
            TempData["Category"] = (from catgry in _dbContext.Categories
                                       select new SelectListItem
                                       {
                                           Text = catgry.Category1,
                                           Value = catgry.Id.ToString()
                                       }).ToList();
            var recipe = new AddRecipe();
            recipe.Ings = (from ing in _dbContext.Ingredients
                           select new DishIng
                           {
                               IngName = ing.IngName +ing.Qty,
                               Id=ing.Id,
                               Qty = ing.Qty
                           }).ToList();
            return View(recipe);
        }
        public IActionResult AddRecipe(AddRecipe recipe)
        {
            recipe.Img = Utils.GetByteArrayFromImage(recipe.Image);
            recipe.AddedDate = DateTime.Now;
            recipe.AddedBy = "Nupoor Prashant";
            _dbContext.Dishes.Add(recipe);
            _dbContext.SaveChanges();
            foreach(var ing in recipe.Ings)
            {
                if (ing.isChecked)
                {
                    _dbContext.DishIngMaps.Add(
                        new DishIngMap()
                        {
                            DishId = recipe.Id,
                            IngId = ing.Id
                        });
                    _dbContext.SaveChanges();
                }
            }
            ViewBag.Success = "Successfully added the recipe";
            return RedirectToAction("Index");
        }
        public IActionResult AddSubCategory(AddRecipe subcatgry)
        {
            if(subcatgry.Subcategory!=null)
            {
                _dbContext.Add(subcatgry.Subcategory);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");

        }
        public IActionResult AddCategory(AddRecipe catgry)
        {
            if(catgry.Category!=null)
            {
                _dbContext.Categories.Add(catgry.Category);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");

        }
        public IActionResult Blog()
        {
            return View(new AddBlog());
        }
        public IActionResult AddBlog(AddBlog blog)
        {
            blog.blogPost.RemoveAll(x => string.IsNullOrEmpty(x.Title));
            Foodzfame.Data.Entities.Blog blog1 = blog;
            blog1.Img = Utils.GetByteArrayFromImage(blog.Image);
            blog.blogPost.ForEach(x => x.Img = Utils.GetByteArrayFromImage(x.Image));
            blog1.BlogPosts = blog.blogPost.ToList<BlogPost>();
            _dbContext.Add(blog1);
            _dbContext.SaveChanges();
            return RedirectToAction("Blog");
        }
        public IActionResult AddIng(AddRecipe ing)
        {
            _dbContext.Ingredients.Add(ing.Ingredient);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Gallery()
        {
            ViewBag.Dishes = (from dish in _dbContext.Dishes
                              select new SelectListItem
                              {
                                  Text = dish.DishName,
                                  Value = dish.Id.ToString()
                              }).ToList();
            return View(new GalleryModel());
        }
        public IActionResult AddGallery(GalleryModel gallery)
        {
            gallery.Img = Utils.GetByteArrayFromImage(gallery.Image);
            _dbContext.Galleries.Add(gallery);
            _dbContext.SaveChanges();
            return RedirectToAction("Gallery");
        }
    }
}
