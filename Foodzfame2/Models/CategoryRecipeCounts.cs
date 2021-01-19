using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foodzfame2.Models
{
    public class CategoryRecipeCounts
    {
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public int SubCatCount { get; set; }
        public int RecipeCount { get; set; }
    }
    public class Counters
    {
        public List<CategoryRecipeCounts> counts { get; set; }
    }
}