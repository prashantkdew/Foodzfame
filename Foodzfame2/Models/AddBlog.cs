using Foodzfame.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foodzfame2.Models
{
    public class AddBlog:Blog
    {
        public IFormFile Image { get; set; }
        public List<Post> blogPost { get; set; }

    }
    public class Post:BlogPost
    {
        public IFormFile Image { get; set; }

    }
}
