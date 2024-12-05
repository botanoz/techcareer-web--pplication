using Core.Persistence.Extensions;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechCareer.Models.Dtos.OperationClaim;

namespace TechCareer.Service.Abstracts
{

    public interface IOperationClaimService
    {
        Task<OperationClaim?> GetAsync(
    Expression<Func<OperationClaim, bool>> predicate,
    bool include = false,
    bool withDeleted = false,
    bool enableTracking = true,
    CancellationToken cancellationToken = default
);


        Task<Paginate<OperationClaim>> GetPaginateAsync(Expression<Func<OperationClaim, bool>>? predicate = null,
            Func<IQueryable<OperationClaim>, IOrderedQueryable<OperationClaim>>? orderBy = null,
            bool include = false,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);


        Task<List<OperationClaim>> GetListAsync(Expression<Func<OperationClaim, bool>>? predicate = null,
            Func<IQueryable<OperationClaim>, IOrderedQueryable<OperationClaim>>? orderBy = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);



        Task<OperationClaim> AddAsync(OperationClaim operationClaim);
        Task<OperationClaim> UpdateAsync(OperationClaimUpdateRequestDto updateRequestDto);
        Task<OperationClaim> DeleteAsync(OperationClaimDeleteRequestDto operationClaimDeleteRequestDto);


    }

}


