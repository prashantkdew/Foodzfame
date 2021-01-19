using System;
using System.Collections.Generic;

#nullable disable

namespace Foodzfame.Data.Entities
{
    public partial class SubScription
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public DateTime? AddedDate { get; set; }
    }
}
