using System;
using System.Collections.Generic;

#nullable disable

namespace Foodzfame.Data.Entities
{
    public partial class BlogPost
    {
        public int Id { get; set; }
        public int? BlogId { get; set; }
        public byte[] Img { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }

        public virtual Blog Blog { get; set; }
    }
}
