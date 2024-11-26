using Core.Persistence.Repositories;
using Core.Security.Entities;

namespace TechCareer.DataAccess.Repositories.Abstracts;

public interface IUserRepository : IAsyncRepository<User,int>
{
    
}