using Foodzfame.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foodzfame2.Models
{
    public class GalleryModel:Gallery
    {
        public IFormFile Image { get; set; }
        public string recipeName { get; set; }
    }
}
