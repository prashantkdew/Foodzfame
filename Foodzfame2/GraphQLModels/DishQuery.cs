using Foodzfame.Data.Entities;
using Foodzfame.Data.FoodzfameContext;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foodzfame2.GraphQLModels
{
    [ExtendObjectType(typeof(Query))]
    public class DishQuery
    {
        [UsePaging(IncludeTotalCount = true)]
        [UseProjection]
        [HotChocolate.Data.UseFiltering]
        [UseSorting]
        public IQueryable<Dish> GetDishes([Service] FoodzfameContext dbContext) => dbContext.Dishes;

        [UsePaging(IncludeTotalCount = true)]
        [UseProjection]
        [HotChocolate.Data.UseFiltering]
        [UseSorting]
        public IQueryable<Review> GetReviews([Service] FoodzfameContext dbContext) => dbContext.Reviews;
    }
}
