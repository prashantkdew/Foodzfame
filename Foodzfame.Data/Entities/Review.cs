using System;
using System.Collections.Generic;

#nullable disable

namespace Foodzfame.Data.Entities
{
    public partial class Review
    {
        public int Id { get; set; }
        public int? DishId { get; set; }
        public byte? Rating { get; set; }
        public string ReviewerName { get; set; }
        public string Review1 { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string ReviewTitle { get; set; }

        public virtual Dish Dish { get; set; }
    }
}
