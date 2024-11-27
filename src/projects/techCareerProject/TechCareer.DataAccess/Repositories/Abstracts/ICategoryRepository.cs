using Core.Persistence.Repositories;
using Core.Security.Entities;

namespace TechCareer.DataAccess.Repositories.Abstracts;

public interface ICategoryRepository : IAsyncRepository<Category,int>
{
    
}