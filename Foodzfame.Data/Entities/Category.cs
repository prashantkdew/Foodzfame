using System;
using System.Collections.Generic;

#nullable disable

namespace Foodzfame.Data.Entities
{
    public partial class Category
    {
        public Category()
        {
            SubCategories = new HashSet<SubCategory>();
        }

        public int Id { get; set; }
        public string Category1 { get; set; }
        public string Des { get; set; }

        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}
