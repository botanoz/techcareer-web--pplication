using Core.Security.Entities;
using Core.Security.Hashing;
using System;
using System.Collections.Generic;

namespace Core.Security.SeedData
{
    public static class UserSeedData
    {
        public static List<User> GetSeedData()
        {
            HashingHelper.CreatePasswordHash(
                password: "Passw0rd",
                passwordHash: out byte[] passwordHash,
                passwordSalt: out byte[] passwordSalt
            );

            return new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "Techcareer",
                    Email = "admin@admin.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Status = true,
                    CreatedDate = DateTime.UtcNow
                }
            };
        }
    }
}
