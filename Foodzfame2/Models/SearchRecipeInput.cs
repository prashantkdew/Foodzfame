using Foodzfame.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foodzfame2.Models
{
    public class SearchRecipeInput
    {
        public string RecipeName { get; set; }
        public int SubCatId { get; set; }
        public string CookingTime { get; set; }
        public int CookingMethod { get; set; }
        public int Difficulty { get; set; }
        public List<Dish> Dishes { get; set; }
    }
}
