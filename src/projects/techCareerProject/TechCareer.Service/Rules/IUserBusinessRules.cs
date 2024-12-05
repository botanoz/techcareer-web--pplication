using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechCareer.Service.Rules
{
    // IUserBusinessRules.cs dosyası
    public interface IUserBusinessRules
    {
        Task UserShouldBeExistsWhenSelected(User user);
        Task UserPasswordShouldBeMatched(User user, string password);
    }

}




