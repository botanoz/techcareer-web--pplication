using Core.Persistence.Extensions;
using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
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
        private readonly IOperationClaimService _operationClaimService;

        public OperationClaimService(IOperationClaimService operationClaimService)
        {
            _operationClaimService = operationClaimService;
        }

        public async Task<OperationClaim> AddAsync(OperationClaim OperationClaim)
        {
            OperationClaim addedOperationClaim = await _operationClaimService.AddAsync(OperationClaim);

            return addedOperationClaim;
        }

        public async Task<OperationClaim> DeleteAsync(OperationClaim OperationClaim, bool permanent = false)
        {
            var deletedOperationClaim = (await GetListAsync(x => x.Id == OperationClaim.Id)).FirstOrDefault();

            deletedOperationClaim.IsDeleted = true;

            return deletedOperationClaim;
        }

        public async Task<OperationClaim?> GetAsync(Expression<Func<OperationClaim, bool>> predicate, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            var operationClaim = await _operationClaimService.GetAsync(predicate);

            return operationClaim;
        }

        public async Task<List<OperationClaim>> GetListAsync(Expression<Func<OperationClaim, bool>>? predicate = null, Func<IQueryable<OperationClaim>, IOrderedQueryable<OperationClaim>>? orderBy = null, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            var operationClaims = await _operationClaimService.GetListAsync();
            return operationClaims;
        }

        public async Task<Paginate<OperationClaim>> GetPaginateAsync(Expression<Func<OperationClaim, bool>>? predicate = null, Func<IQueryable<OperationClaim>, IOrderedQueryable<OperationClaim>>? orderBy = null, bool include = false, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<OperationClaim> operationClaims = (IQueryable<OperationClaim>)_operationClaimService.GetListAsync();

            if (!withDeleted)
              operationClaims = operationClaims.Where(c => !c.IsDeleted);
            if (predicate != null)
               operationClaims = operationClaims.Where(predicate);
            if (!enableTracking)
               operationClaims = operationClaims.AsNoTracking();
            
            int totalItems = await operationClaims.CountAsync(cancellationToken);

            List<OperationClaim> items = await operationClaims
                .Skip(index * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return new Paginate<OperationClaim>
            {
                Items = items,
                Index = index,
                Size = size,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)size)
            };
        }

        public Task<OperationClaim> UpdateAsync(OperationClaim OperationClaim)
        {
            throw new NotImplementedException();
        }
    }
}
