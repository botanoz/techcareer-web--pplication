using Core.Persistence.Extensions;
using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Dtos.OperationClaim;
using TechCareer.Service.Abstracts;

namespace TechCareer.Service.Concretes
{
    public class OperationClaimService : IOperationClaimService
    {
        private readonly IOperationClaimRepository _operationClaimRepository;

        public OperationClaimService(IOperationClaimRepository operationClaimRepository)
        {
            _operationClaimRepository = operationClaimRepository;
        }

        public async Task<OperationClaim> AddAsync(OperationClaim operationClaim)
        {
            if (string.IsNullOrWhiteSpace(operationClaim.Name))
                throw new ArgumentException("Operation claim name cannot be null or empty.");

            var addedClaim = await _operationClaimRepository.AddAsync(operationClaim);
            return addedClaim;
        }

        public async Task<OperationClaim> DeleteAsync(OperationClaimDeleteRequestDto operationClaimDeleteRequestDto)
        {
            var existingClaim = await _operationClaimRepository.GetAsync(c => c.Id == operationClaimDeleteRequestDto.Id);

            if (existingClaim == null)
                throw new KeyNotFoundException("Operation claim not found.");

            if (operationClaimDeleteRequestDto.Permanent)
            {
                await _operationClaimRepository.DeleteAsync(existingClaim);
            }
            else
            {
                existingClaim.IsDeleted = true;
                await _operationClaimRepository.UpdateAsync(existingClaim);
            }

            return existingClaim;
        }


        public async Task<OperationClaim?> GetAsync(Expression<Func<OperationClaim, bool>> predicate, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            var operationClaim = await _operationClaimRepository.GetAsync(predicate);

            return operationClaim;
        }

        public async Task<List<OperationClaim>> GetListAsync(Expression<Func<OperationClaim, bool>>? predicate = null, Func<IQueryable<OperationClaim>, IOrderedQueryable<OperationClaim>>? orderBy = null, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            var operationClaims = await _operationClaimRepository.GetListAsync();
            return operationClaims;
        }

        public async Task<Paginate<OperationClaim>> GetPaginateAsync(Expression<Func<OperationClaim, bool>>? predicate = null, Func<IQueryable<OperationClaim>, IOrderedQueryable<OperationClaim>>? orderBy = null, bool include = false, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<OperationClaim> operationClaims = (IQueryable<OperationClaim>)_operationClaimRepository.GetListAsync();

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

        public async Task<OperationClaim> UpdateAsync(OperationClaimUpdateRequestDto updateRequestDto)
        {
            var existingClaim = await _operationClaimRepository.GetAsync(c => c.Id == updateRequestDto.Id);

            if (existingClaim == null)
                throw new KeyNotFoundException("Operation claim not found.");

            existingClaim.Name = updateRequestDto.Name;

            var updatedClaim = await _operationClaimRepository.UpdateAsync(existingClaim);

            return updatedClaim;
        }

    }
}
