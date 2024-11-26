using Core.Persistence.Extensions;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.JWT;
using TechCareer.Models.Dtos.Users;

namespace TechCareer.Service.Abstracts;

public interface IAuthService
{
    Task<AccessToken> LoginAsync(UserForLoginDto dto,CancellationToken cancellationToken);
    Task<AccessToken> RegisterAsync(UserForRegisterDto dto,CancellationToken cancellationToken);

    Task<Paginate<UserResponseDto>> GetAllPaginateAsync(int page, int size);
    
}