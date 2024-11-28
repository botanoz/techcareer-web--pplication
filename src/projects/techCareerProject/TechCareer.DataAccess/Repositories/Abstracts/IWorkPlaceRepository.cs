using Core.Persistence.Repositories;
using Core.Security.Entities;

namespace TechCareer.DataAccess.Repositories.Abstracts;

public interface IWorkPlaceRepository : IAsyncRepository<WorkPlace, int>
{
    
}