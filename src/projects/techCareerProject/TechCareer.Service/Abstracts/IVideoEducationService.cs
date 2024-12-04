using Core.Persistence.Extensions;
using Core.Persistence.Repositories;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace TechCareer.Service.Abstracts
{
    public interface IVideoEducationService
    {
        Task<VideoEducation?> GetAsync(
            Expression<Func<VideoEducation, bool>> predicate,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        );

        Task<Paginate<VideoEducation>> GetPaginateAsync(
            Expression<Func<VideoEducation, bool>>? predicate = null,
            Func<IQueryable<VideoEducation>, IOrderedQueryable<VideoEducation>>? orderBy = null,
            bool include = false,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        );

        Task<List<VideoEducation>> GetListAsync(
            Expression<Func<VideoEducation, bool>>? predicate = null,
            Func<IQueryable<VideoEducation>, IOrderedQueryable<VideoEducation>>? orderBy = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        );

        Task<VideoEducation> AddAsync(VideoEducation videoEducation);
        Task<VideoEducation> UpdateAsync(VideoEducation videoEducation);
        Task<VideoEducation> DeleteAsync(VideoEducation videoEducation, bool permanent = false);
    }
}
