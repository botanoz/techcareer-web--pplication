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
    public class OperationClaimService : IOperationClaimService
    {
        public Task<OperationClaim> AddAsync(OperationClaim OperationClaim)
        {
            throw new NotImplementedException();
        }

        public Task<OperationClaim> DeleteAsync(OperationClaim OperationClaim, bool permanent = false)
        {
            throw new NotImplementedException();
        }

        public Task<OperationClaim?> GetAsync(Expression<Func<OperationClaim, bool>> predicate, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<OperationClaim>> GetListAsync(Expression<Func<OperationClaim, bool>>? predicate = null, Func<IQueryable<OperationClaim>, IOrderedQueryable<OperationClaim>>? orderBy = null, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Paginate<OperationClaim>> GetPaginateAsync(Expression<Func<OperationClaim, bool>>? predicate = null, Func<IQueryable<OperationClaim>, IOrderedQueryable<OperationClaim>>? orderBy = null, bool include = false, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<OperationClaim> UpdateAsync(OperationClaim OperationClaim)
        {
            throw new NotImplementedException();
        }
    }
}
