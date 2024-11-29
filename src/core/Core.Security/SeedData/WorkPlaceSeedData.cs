using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.SeedData
{
    public class WorkPlaceSeedData
    {
        public static List<WorkPlace> GetSeedData()
        {
            return new List<WorkPlace>()
            {
                new WorkPlace{Id=1,Name="İş Yerinde" },
                new WorkPlace{Id=2,Name="Hibrit" },
                new WorkPlace{Id=3,Name="Uzaktan" },
            };
        }
    }
}
