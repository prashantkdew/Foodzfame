using Foodzfame.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Foodzfame2.Models.GraphQLResponse
{

    public class GraphQLResponseVm
    {
        public FFDishes dishes { get; set; }
        public FFCategory categories { get; set; }
    }

    public class FFDishes
    {
        public List<Dish> nodes { get; set; }
    }
    public class FFCategory
    {
        public List<Category> nodes { get; set; }
    }


}
