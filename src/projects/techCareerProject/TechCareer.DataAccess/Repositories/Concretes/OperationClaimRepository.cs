using Azure;
using Core.Persistence.Repositories;
using Core.Security.Entities;
using TechCareer.DataAccess.Contexts;
using TechCareer.DataAccess.Repositories.Abstracts;

namespace TechCareer.DataAccess.Repositories.Concretes;

public sealed class OperationClaimRepository(BaseDbContext context) : EfRepositoryBase<OperationClaim,int,BaseDbContext>(context), IOperationClaimRepository
{
    
}