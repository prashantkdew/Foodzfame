using Foodzfame.Data.FoodzfameContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foodzfame2
{
    public class Common
    {
        FoodzfameContext _dbContext;

        public Common(FoodzfameContext foodzfameContext)
        {
            _dbContext = foodzfameContext;
        }
        public List<KeyValuePair<int,string>>  GetAllDish()
        {
            List<KeyValuePair<int, string>> lst = new List<KeyValuePair<int, string>>();

            lst = _dbContext.Dishes.Select(x => new KeyValuePair<int, string>(x.Id, x.DishName)).ToList();
            return lst;
        }
    }
}
