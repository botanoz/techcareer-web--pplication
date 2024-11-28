using Core.Persistence.Extensions;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TechCareer.Service.Abstracts
{
    public interface IUserOperationClaimService
    {
        Task<UserOperationClaim?> GetAsync(
   Expression<Func<UserOperationClaim, bool>> predicate,
   bool include = false,
   bool withDeleted = false,
   bool enableTracking = true,
   CancellationToken cancellationToken = default
);


        Task<Paginate<UserOperationClaim>> GetPaginateAsync(Expression<Func<UserOperationClaim, bool>>? predicate = null,
            Func<IQueryable<UserOperationClaim>, IOrderedQueryable<UserOperationClaim>>? orderBy = null,
            bool include = false,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);


        Task<List<UserOperationClaim>> GetListAsync(Expression<Func<UserOperationClaim, bool>>? predicate = null,
            Func<IQueryable<UserOperationClaim>, IOrderedQueryable<UserOperationClaim>>? orderBy = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);


        Task<UserOperationClaim> AddAsync(UserOperationClaim userOperationClaim);
        Task<UserOperationClaim> UpdateAsync(UserOperationClaim userOperationClaim);
        Task<UserOperationClaim> DeleteAsync(UserOperationClaim userOperationClaim, bool permanent = false);

    }
}

