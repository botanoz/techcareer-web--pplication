using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.SeedData
{
    public static class YearsOfExperienceSeedData
    {
        public static List<YearsOfExperience> GetSeedData()
        {
            return new List<YearsOfExperience>()
            {
                new YearsOfExperience{Id=1,Name="Deneyimsiz"},
                new YearsOfExperience{Id=2,Name="Deneyim: 4-6 Yıl"},
                new YearsOfExperience{Id=3,Name="Deneyim: 2-4 Yıl"},
                new YearsOfExperience{Id=4,Name="Deneyim: 1-2 Yıl"},
                new YearsOfExperience{Id=5,Name="Deneyimsiz / Deneyimli"},

            };


        }

    }
}
