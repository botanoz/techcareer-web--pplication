using Core.Security.Entities;

namespace TechCareer.Service.Rules
{
    public interface IUserBusinessRules
    {
        Task UserShouldBeExistsWhenSelected(User? user);
        Task UserIdShouldExistWhenSelected(int id);
        Task UserPasswordShouldBeMatched(User user, string password);
        Task UserEmailShouldNotExistWhenInsert(string email);
        Task UserEmailShouldNotExistWhenUpdate(int id, string email);
    }
}
