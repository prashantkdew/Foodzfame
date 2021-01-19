using System;
using System.Collections.Generic;

#nullable disable

namespace Foodzfame.Data.Entities
{
    public partial class HealthBenefit
    {
        public int Id { get; set; }
        public byte[] Img { get; set; }
        public string Title { get; set; }
        public string Benefits { get; set; }
    }
}
