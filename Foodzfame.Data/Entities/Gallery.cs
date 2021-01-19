using System;
using System.Collections.Generic;

#nullable disable

namespace Foodzfame.Data.Entities
{
    public partial class Gallery
    {
        public long Id { get; set; }
        public int? DishId { get; set; }
        public string Title { get; set; }
        public byte[] Img { get; set; }
    }
}
