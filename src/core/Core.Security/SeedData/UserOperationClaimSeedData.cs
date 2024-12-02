using Core.Security.Entities;
using System.Collections.Generic;

namespace Core.Security.SeedData
{
    public static class UserOperationClaimSeedData
    {
        public static List<UserOperationClaim> GetSeedData()
        {
            return new List<UserOperationClaim>
            {
                new UserOperationClaim(id: 1, userId: 1, operationClaimId: 1)
            };
        }
    }
}
