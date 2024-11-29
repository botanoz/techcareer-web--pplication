using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.SeedData
{
    public static class TypOfWorkSeedData
    {
        public static List<TypOfWork> GetSeedData()
        {
            return new List<TypOfWork>()
            {
                new TypOfWork{Id=1, Name="Stajyer"},
                new TypOfWork{Id=2,Name="Dönemsel (Sözleşmeli)"},
                new TypOfWork{Id=3,Name="Tam Zamanlı"},

            };
        }
    }
}
