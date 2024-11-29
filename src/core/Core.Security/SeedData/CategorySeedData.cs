using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.SeedData
{
    public class CategorySeedData
    {
        public static List<Category> GetSeedData()
        {
            return new List<Category>()
            {
             new Category {Id=1,Name = "Bootcamp"  },
             new Category {Id=2,Name = "Hackathon" },
             new Category {Id=3,Name = "Hiring Challenge"},
             new Category {Id=4,Name = "Tüm Etkinlikler" },
            };


        }
    }
}
