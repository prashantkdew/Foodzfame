using Foodzfame.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foodzfame2.Models
{
    public class CategoryWise
    {
        public List<Dish> Dishes { get; set; }
        public List<Category> Categories { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public int Category { get; set; }
        public int  SubCategory { get; set; }
    }
}
