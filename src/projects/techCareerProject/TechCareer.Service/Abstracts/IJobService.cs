using Core.Persistence.Extensions;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechCareer.Models.Dtos.Job;

namespace TechCareer.Service.Abstracts
{
    public interface IJobService
    {
        Task<JobResponseDto?> GetAsync(
        Expression<Func<Job, bool>> predicate,
        bool include = false,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
        );

        Task<Paginate<JobResponseDto>> GetPaginateAsync(Expression<Func<Job, bool>>? predicate = null,
            Func<IQueryable<Job>, IOrderedQueryable<Job>>? orderBy = null,
            bool include = false,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);

        Task<List<JobResponseDto>> GetListAsync(Expression<Func<Job, bool>>? predicate = null,
            Func<IQueryable<Job>, IOrderedQueryable<Job>>? orderBy = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);

        Task<JobResponseDto> AddAsync(JobAddRequestDto jobAddRequestDto);
        Task<JobResponseDto> UpdateAsync(JobUpdateRequestDto jobUpdateRequestDto);
        Task<JobResponseDto> DeleteAsync(JobRequestDto jobRequestDto, bool permanent = false);


    }
}

