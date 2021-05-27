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
    public class GalleryQuery
    {
        [UsePaging(IncludeTotalCount = true)]
        [UseProjection]
        [HotChocolate.Data.UseFiltering]
        [UseSorting]
        public IQueryable<Gallery> GetGalleryImages([Service] FoodzfameContext dbContext) => dbContext.Galleries;
    }
}
