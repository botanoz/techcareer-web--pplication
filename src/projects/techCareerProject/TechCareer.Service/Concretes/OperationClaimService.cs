using Core.CrossCuttingConcerns.Serilog;
using Core.Persistence.Extensions;
using Core.Security.Entities;
using System.Linq.Expressions;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Dtos.OperationClaim;
using TechCareer.Service.Abstracts;

namespace TechCareer.Service.Concretes
{
    public class OperationClaimService : IOperationClaimService
    {
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly LoggerServiceBase _logger;

        public OperationClaimService(IOperationClaimRepository operationClaimRepository, LoggerServiceBase logger)
        {
            _operationClaimRepository = operationClaimRepository;
            _logger = logger;
        }

        // Get a single operation claim with optional filters
        public async Task<OperationClaimResponseDto?> GetAsync(
            Expression<Func<OperationClaim, bool>> predicate,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var operationClaim = await _operationClaimRepository.GetAsync(predicate, withDeleted: withDeleted);

                if (operationClaim == null)
                    return null;

                return new OperationClaimResponseDto
                {
                    Id = operationClaim.Id,
                    Name = operationClaim.Name
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

        }

        // Get paginated list of operation claims
        public async Task<Paginate<OperationClaimResponseDto>> GetPaginateAsync(
            Expression<Func<OperationClaim, bool>>? predicate = null,
            Func<IQueryable<OperationClaim>, IOrderedQueryable<OperationClaim>>? orderBy = null,
            bool include = false,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var paginateResult = await _operationClaimRepository.GetPaginateAsync(predicate, index: index, size: size, enableTracking: enableTracking, withDeleted: withDeleted);

                return new Paginate<OperationClaimResponseDto>
                {
                    Items = paginateResult.Items.Select(operationClaim => new OperationClaimResponseDto
                    {
                        Id = operationClaim.Id,
                        Name = operationClaim.Name
                    }).ToList(),
                    Index = paginateResult.Index,
                    Size = paginateResult.Size,
                    TotalItems = paginateResult.TotalItems,
                    TotalPages = paginateResult.TotalPages
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

        }

        // Get list of operation claims
        public async Task<List<OperationClaimResponseDto>> GetListAsync(
            Expression<Func<OperationClaim, bool>>? predicate = null,
            Func<IQueryable<OperationClaim>, IOrderedQueryable<OperationClaim>>? orderBy = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var operationClaims = await _operationClaimRepository.GetListAsync(predicate, orderBy, enableTracking, withDeleted);

                return operationClaims.Select(operationClaim => new OperationClaimResponseDto
                {
                    Id = operationClaim.Id,
                    Name = operationClaim.Name
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

        }

        // Add a new operation claim
        public async Task<OperationClaimResponseDto> AddAsync(OperationClaimAddRequestDto operationClaimAddRequestDto)
        {
            try
            {
                // Yeni bir OperationClaim entity oluştur
                var operationClaim = new OperationClaim
                {
                    Name = operationClaimAddRequestDto.Name
                };

                // Veritabanına kaydet
                var addedOperationClaim = await _operationClaimRepository.AddAsync(operationClaim);

                _logger.Info("Info log: OperationClaim added.");

                // Cevap DTO'su oluştur ve geri döndür
                return new OperationClaimResponseDto
                {
                    Id = addedOperationClaim.Id,
                    Name = addedOperationClaim.Name
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

        }

        // Update an existing operation claim
        public async Task<OperationClaimResponseDto> UpdateAsync(OperationClaimUpdateRequestDto operationClaimUpdateRequestDto)
        {
            try
            {
                var operationClaim = await _operationClaimRepository.GetAsync(x => x.Id == operationClaimUpdateRequestDto.Id);

                if (operationClaim == null)
                    throw new ApplicationException("Operation claim not found.");

                // Update fields
                operationClaim.Name = operationClaimUpdateRequestDto.Name;

                var updatedOperationClaim = await _operationClaimRepository.UpdateAsync(operationClaim);

                _logger.Info("Info log: OperationClaim updated.");

                return new OperationClaimResponseDto
                {
                    Id = updatedOperationClaim.Id,
                    Name = updatedOperationClaim.Name
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

        }

        // Delete an operation claim
        public async Task<OperationClaimResponseDto> DeleteAsync(OperationClaimRequestDto operationClaimRequestDto, bool permanent = false)
        {
            try
            {
                var operationClaim = await _operationClaimRepository.GetAsync(
                    x => x.Id == operationClaimRequestDto.Id,
                    withDeleted: true
                );

                if (operationClaim == null)
                    throw new ApplicationException("Operation claim not found.");

                if (permanent)
                {
                    await _operationClaimRepository.DeleteAsync(operationClaim, true);
                }
                else
                {
                    operationClaim.IsDeleted = true;
                    await _operationClaimRepository.DeleteAsync(operationClaim);
                }

                _logger.Info("Info log: OperationClaim deleted.");

                return new OperationClaimResponseDto
                {
                    Id = operationClaim.Id,
                    Name = operationClaim.Name
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

        }
    }
}
