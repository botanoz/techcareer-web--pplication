using Core.Security.Entities;
using System.Collections.Generic;

namespace Core.Security.SeedData
{
    public static class OperationClaimSeedData
    {
        public static List<OperationClaim> GetSeedData()
        {
            return new List<OperationClaim>
            {
                new OperationClaim { Id = 1, Name = "Admin" },
                new OperationClaim { Id = 2, Name = "User" },
                new OperationClaim { Id = 3, Name = "Instructor" }
            };
        }
    }
}
