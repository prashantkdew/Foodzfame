using Foodzfame.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foodzfame2.Models
{
    public class AddRecipe:Dish
    {
        public IFormFile Image { get; set; }
        public Category Category { get; set; }
        public SubCategory Subcategory { get; set; }
        public Ingredient Ingredient { get; set; }
        public List<DishIng> Ings { get; set; }
    }
    [Serializable]
    public class DishIng:Ingredient
    {
        public bool isChecked { get; set; }
    }
}
