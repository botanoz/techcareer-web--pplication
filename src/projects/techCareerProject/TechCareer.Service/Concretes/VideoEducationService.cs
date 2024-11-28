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
    public class VideoEducationService : IVideoEducationService
    {
        public Task<VideoEducation> AddAsync(VideoEducation videoEducation)
        {
            throw new NotImplementedException();
        }

        public Task<VideoEducation> DeleteAsync(VideoEducation videoEducation, bool permanent = false)
        {
            throw new NotImplementedException();
        }

        public Task<VideoEducation?> GetAsync(Expression<Func<VideoEducation, bool>> predicate, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VideoEducation>> GetListAsync(Expression<Func<VideoEducation, bool>>? predicate = null, Func<IQueryable<VideoEducation>, IOrderedQueryable<VideoEducation>>? orderBy = null, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Paginate<VideoEducation>> GetPaginateAsync(Expression<Func<VideoEducation, bool>>? predicate = null, Func<IQueryable<VideoEducation>, IOrderedQueryable<VideoEducation>>? orderBy = null, bool include = false, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<VideoEducation> UpdateAsync(VideoEducation videoEducation)
        {
            throw new NotImplementedException();
        }
    }
}
