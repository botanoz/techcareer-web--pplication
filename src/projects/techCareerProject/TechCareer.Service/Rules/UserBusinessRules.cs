using Core.CrossCuttingConcerns.Exceptions.ExceptionTypes;
using Core.CrossCuttingConcerns.Rules;
using Core.Security.Entities;
using Core.Security.Hashing;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Service.Constants;

namespace TechCareer.Service.Rules
{
    public class UserBusinessRules : BaseBusinessRules,IUserBusinessRules
    {
        private readonly IUserRepository _userRepository;

        public UserBusinessRules(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task UserShouldBeExistsWhenSelected(User? user)
        {
            if (user == null)
                throw new BusinessException(AuthMessages.UserDontExists);
            return Task.CompletedTask;
        }

        public async Task UserIdShouldExistWhenSelected(int id)
        {
            bool doesExist = await _userRepository.AnyAsync(u => u.Id == id, enableTracking: false);
            if (!doesExist)
                throw new BusinessException(AuthMessages.UserDontExists);
        }

        public Task UserPasswordShouldBeMatched(User user, string password)
        {
            if (!HashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                throw new BusinessException(AuthMessages.PasswordDontMatch);
            return Task.CompletedTask;
        }

        public async Task UserEmailShouldNotExistWhenInsert(string email)
        {
            bool doesExist = await _userRepository.AnyAsync(u => u.Email == email, enableTracking: false);
            if (doesExist)
                throw new BusinessException(AuthMessages.UserMailAlreadyExists);
        }

        public async Task UserEmailShouldNotExistWhenUpdate(int id, string email)
        {
            bool doesExist = await _userRepository.AnyAsync(u => u.Id != id && u.Email == email, enableTracking: false);
            if (doesExist)
                throw new BusinessException(AuthMessages.UserMailAlreadyExists);
        }
    }
}
