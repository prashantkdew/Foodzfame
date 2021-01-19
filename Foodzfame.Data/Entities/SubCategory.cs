using System;
using System.Collections.Generic;

#nullable disable

namespace Foodzfame.Data.Entities
{
    [Serializable]
    public partial class SubCategory
    {
        public SubCategory()
        {
            Dishes = new HashSet<Dish>();
        }

        public int Id { get; set; }
        public string Category { get; set; }
        public string Des { get; set; }
        public int CategoryId { get; set; }

        public virtual Category CategoryNavigation { get; set; }
        public virtual ICollection<Dish> Dishes { get; set; }
    }
}
