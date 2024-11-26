using Core.Security.Entities;
using Core.Security.JWT;

namespace TechCareer.Service.Abstracts;

public interface IUserWithTokenService
{
    public Task<AccessToken> CreateAccessToken(User user);
}