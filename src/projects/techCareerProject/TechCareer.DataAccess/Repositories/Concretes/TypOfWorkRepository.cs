using Core.Persistence.Repositories;
using Core.Security.Entities;
using TechCareer.DataAccess.Contexts;
using TechCareer.DataAccess.Repositories.Abstracts;

namespace TechCareer.DataAccess.Repositories.Concretes;

public sealed class TypOfWorkRepository(BaseDbContext context) : EfRepositoryBase<TypOfWork, int,BaseDbContext>(context), ITypOfWorkRepository
{
    
}