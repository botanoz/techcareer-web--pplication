using Core.CrossCuttingConcerns.Serilog;
using Core.Persistence.Extensions;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.DataAccess.Repositories.Concretes;
using TechCareer.Models.Dtos.Category;
using TechCareer.Models.Dtos.Job;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Rules;

namespace TechCareer.Service.Concretes
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly LoggerServiceBase _logger;

        public JobService(IJobRepository jobRepository, LoggerServiceBase logger)
        {
            _jobRepository = jobRepository;
            _logger = logger;
        }

        // Get a single job with optional filters
        public async Task<JobResponseDto?> GetAsync(
            Expression<Func<Job, bool>> predicate,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {

            try
            {
                var job = await _jobRepository.GetAsync(predicate, withDeleted: withDeleted);

                if (job == null)
                    return null;

                return new JobResponseDto
                {
                    Id = job.Id,
                    Title = job.Title,
                    TypeOfWork = job.TypeOfWork,
                    YearsOfExperience = job.YearsOfExperience,
                    WorkPlace = job.WorkPlace,
                    StartDate = job.StartDate,
                    Content = job.Content,
                    Description = job.Description,
                    Skills = job.Skills,
                    CompanyId = job.CompanyId
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }
        }

        // Get paginated list of jobs
        public async Task<Paginate<JobResponseDto>> GetPaginateAsync(
            Expression<Func<Job, bool>>? predicate = null,
            Func<IQueryable<Job>, IOrderedQueryable<Job>>? orderBy = null,
            bool include = false,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var paginateResult = await _jobRepository.GetPaginateAsync(predicate, index: index, size: size, enableTracking: enableTracking, withDeleted: withDeleted);

                return new Paginate<JobResponseDto>
                {
                    Items = paginateResult.Items.Select(job => new JobResponseDto
                    {
                        Id = job.Id,
                        Title = job.Title,
                        TypeOfWork = job.TypeOfWork,
                        YearsOfExperience = job.YearsOfExperience,
                        WorkPlace = job.WorkPlace,
                        StartDate = job.StartDate,
                        Content = job.Content,
                        Description = job.Description,
                        Skills = job.Skills,
                        CompanyId = job.CompanyId
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

        // Get list of jobs
        public async Task<List<JobResponseDto>> GetListAsync(
            Expression<Func<Job, bool>>? predicate = null,
            Func<IQueryable<Job>, IOrderedQueryable<Job>>? orderBy = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var jobs = await _jobRepository.GetListAsync(predicate, orderBy, enableTracking, withDeleted);

                return jobs.Select(job => new JobResponseDto
                {
                    Id = job.Id,
                    Title = job.Title,
                    TypeOfWork = job.TypeOfWork,
                    YearsOfExperience = job.YearsOfExperience,
                    WorkPlace = job.WorkPlace,
                    StartDate = job.StartDate,
                    Content = job.Content,
                    Description = job.Description,
                    Skills = job.Skills,
                    CompanyId = job.CompanyId
                }).ToList();

            }
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }


        }

        // Add a new job
        public async Task<JobResponseDto> AddAsync(JobAddRequestDto jobAddRequestDto)
        {
            try
            {
                // Yeni bir Job entity oluştur
                var job = new Job
                {
                    Title = jobAddRequestDto.Title,
                    TypeOfWork = jobAddRequestDto.TypeOfWork,
                    YearsOfExperience = jobAddRequestDto.YearsOfExperience,
                    WorkPlace = jobAddRequestDto.WorkPlace,
                    StartDate = jobAddRequestDto.StartDate,
                    Content = jobAddRequestDto.Content,
                    Description = jobAddRequestDto.Description,
                    Skills = jobAddRequestDto.Skills,
                    CompanyId = jobAddRequestDto.CompanyId
                };

                // Veritabanına kaydet
                var addedJob = await _jobRepository.AddAsync(job);

                _logger.Info("Info log: Job added.");

                // Cevap DTO'su oluştur ve geri döndür
                return new JobResponseDto
                {
                    Id = addedJob.Id,
                    Title = addedJob.Title,
                    TypeOfWork = addedJob.TypeOfWork,
                    YearsOfExperience = addedJob.YearsOfExperience,
                    WorkPlace = addedJob.WorkPlace,
                    StartDate = addedJob.StartDate,
                    Content = addedJob.Content,
                    Description = addedJob.Description,
                    Skills = addedJob.Skills,
                    CompanyId = addedJob.CompanyId
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

        }


        // Update an existing job
        public async Task<JobResponseDto> UpdateAsync(JobUpdateRequestDto jobUpdateRequestDto)
        {
            try
            {
                var job = await _jobRepository.GetAsync(x => x.Id == jobUpdateRequestDto.Id);

                if (job == null)
                {
                    _logger.Warn("Warn log: Job not found.");
                    throw new ApplicationException("Job not found.");
                }                 

                // Update fields
                job.Title = jobUpdateRequestDto.Title;
                job.TypeOfWork = jobUpdateRequestDto.TypeOfWork;
                job.YearsOfExperience = jobUpdateRequestDto.YearsOfExperience;
                job.WorkPlace = jobUpdateRequestDto.WorkPlace;
                job.StartDate = jobUpdateRequestDto.StartDate;
                job.Content = jobUpdateRequestDto.Content;
                job.Description = jobUpdateRequestDto.Description;
                job.Skills = jobUpdateRequestDto.Skills;
                job.CompanyId = jobUpdateRequestDto.CompanyId;

                var updatedJob = await _jobRepository.UpdateAsync(job);

                _logger.Info("Info log: Job updated.");

                return new JobResponseDto
                {
                    Id = updatedJob.Id,
                    Title = updatedJob.Title,
                    TypeOfWork = updatedJob.TypeOfWork,
                    YearsOfExperience = updatedJob.YearsOfExperience,
                    WorkPlace = updatedJob.WorkPlace,
                    StartDate = updatedJob.StartDate,
                    Content = updatedJob.Content,
                    Description = updatedJob.Description,
                    Skills = updatedJob.Skills,
                    CompanyId = updatedJob.CompanyId
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }
            
        }

        public async Task<JobResponseDto> DeleteAsync(JobRequestDto jobRequestDto, bool permanent = false)
        {
            try
            {
                var job = await _jobRepository.GetAsync(
                    x => x.Id == jobRequestDto.Id,
                    withDeleted: true
                );

                if (job == null)
                {
                    _logger.Warn("Warn log: Job not found.");
                    throw new ApplicationException("Job not found.");
                }
                    

                if (permanent)
                {
                    await _jobRepository.DeleteAsync(job, true);
                }
                else
                {
                    job.IsDeleted = true;
                    await _jobRepository.DeleteAsync(job);
                }

                _logger.Info("Info log: Job deleted.");

                return new JobResponseDto
                {
                    Id = job.Id,
                    Title = job.Title,
                    TypeOfWork = job.TypeOfWork,
                    YearsOfExperience = job.YearsOfExperience,
                    WorkPlace = job.WorkPlace,
                    StartDate = job.StartDate,
                    Content = job.Content,
                    Description = job.Description,
                    Skills = job.Skills,
                    CompanyId = job.CompanyId
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
