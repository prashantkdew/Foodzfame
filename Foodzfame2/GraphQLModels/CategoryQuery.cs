﻿using Foodzfame.Data.Entities;
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
    public class CategoryQuery
    {
        [UsePaging(IncludeTotalCount = true)]
        [UseProjection]
        [HotChocolate.Data.UseFiltering]
        [UseSorting]
        public IQueryable<Category> GetCategories([Service] FoodzfameContext dbContext) => dbContext.Categories;

        [UsePaging(IncludeTotalCount = true)]
        [UseProjection]
        [HotChocolate.Data.UseFiltering]
        [UseSorting]
        public IQueryable<SubCategory> GetSubCategories([Service] FoodzfameContext dbContext) => dbContext.SubCategories;
    }
}