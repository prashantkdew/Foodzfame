using System;
using System.Collections.Generic;

#nullable disable

namespace Foodzfame.Data.Entities
{
    public partial class Ingredient
    {
        public Ingredient()
        {
            DishIngMaps = new HashSet<DishIngMap>();
        }

        public int Id { get; set; }
        public string IngName { get; set; }
        public string Qty { get; set; }
        public string Precessed { get; set; }

        public virtual ICollection<DishIngMap> DishIngMaps { get; set; }
    }
}
