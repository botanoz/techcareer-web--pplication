using AutoMapper;
using Core.Persistence.Extensions;
using Core.Security.Entities;
using TechCareer.Models.Dtos.Users;

namespace TechCareer.Service.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserResponseDto>();
        CreateMap<Paginate<User>, Paginate<UserResponseDto>>();
    }
}