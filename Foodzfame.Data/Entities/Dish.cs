using System;
using System.Collections.Generic;

#nullable disable

namespace Foodzfame.Data.Entities
{
    [Serializable]
    public partial class Dish
    {
        public Dish()
        {
            DishIngMaps = new HashSet<DishIngMap>();
            Reviews = new HashSet<Review>();
        }

        public int Id { get; set; }
        public string DishName { get; set; }
        public string Desc { get; set; }
        public int DishCategoryId { get; set; }
        public bool? EditorsChoice { get; set; }
        public DateTime? AddedDate { get; set; }
        public string AddedBy { get; set; }
        public long? Likes { get; set; }
        public byte[] Img { get; set; }
        public string CookingTime { get; set; }
        public string Yields { get; set; }
        public string VideoLink { get; set; }
        public int? RecipeDifficulty { get; set; }
        public int? CookingMethod { get; set; }
        public string Tags { get; set; }
        public string CookingInstruction { get; set; }

        public virtual SubCategory DishCategory { get; set; }
        public virtual ICollection<DishIngMap> DishIngMaps { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
