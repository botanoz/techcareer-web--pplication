using Core.Persistence.Extensions;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechCareer.Service.Abstracts;

namespace TechCareer.Service.Concretes
{
    public class UserOperationClaimService : IUserOperationClaimService
    {
        public Task<UserOperationClaim> AddAsync(UserOperationClaim userOperationClaim)
        {
            throw new NotImplementedException();
        }

        public Task<UserOperationClaim> DeleteAsync(UserOperationClaim userOperationClaim, bool permanent = false)
        {
            throw new NotImplementedException();
        }

        public Task<UserOperationClaim?> GetAsync(Expression<Func<UserOperationClaim, bool>> predicate, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserOperationClaim>> GetListAsync(Expression<Func<UserOperationClaim, bool>>? predicate = null, Func<IQueryable<UserOperationClaim>, IOrderedQueryable<UserOperationClaim>>? orderBy = null, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Paginate<UserOperationClaim>> GetPaginateAsync(Expression<Func<UserOperationClaim, bool>>? predicate = null, Func<IQueryable<UserOperationClaim>, IOrderedQueryable<UserOperationClaim>>? orderBy = null, bool include = false, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UserOperationClaim> UpdateAsync(UserOperationClaim userOperationClaim)
        {
            throw new NotImplementedException();
        }
    }
}
