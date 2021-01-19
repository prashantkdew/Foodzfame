using System;
using System.Collections.Generic;

#nullable disable

namespace Foodzfame.Data.Entities
{
    public partial class DishIngMap
    {
        public int Id { get; set; }
        public int? IngId { get; set; }
        public int? DishId { get; set; }

        public virtual Dish Dish { get; set; }
        public virtual Ingredient Ing { get; set; }
    }
}
