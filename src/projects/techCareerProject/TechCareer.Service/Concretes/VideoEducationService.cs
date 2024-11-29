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
using TechCareer.Service.Abstracts;

namespace TechCareer.Service.Concretes
{
    public class VideoEducationService : IVideoEducationService
    {
        private readonly IVideoEducationService _videoEducationService;

        public VideoEducationService(IVideoEducationService videoEducationService)
        {
            _videoEducationService = videoEducationService;
        }

        public async Task<VideoEducation> AddAsync(VideoEducation videoEducation)
        {
            VideoEducation addedVideoEducation = await _videoEducationService.AddAsync(videoEducation);

            return addedVideoEducation;
        }

        public async Task<VideoEducation> DeleteAsync(VideoEducation videoEducation, bool permanent = false)
        {
            var deletedVideoEducation = (await GetListAsync(x => x.Id == videoEducation.Id)).FirstOrDefault();

            deletedVideoEducation.IsDeleted = true;

            return deletedVideoEducation;
        }

        public async Task<VideoEducation?> GetAsync(Expression<Func<VideoEducation, bool>> predicate, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            var userOperationClaim = await _videoEducationService.GetAsync(predicate);

            return userOperationClaim;
        }

        public async Task<List<VideoEducation>> GetListAsync(Expression<Func<VideoEducation, bool>>? predicate = null, Func<IQueryable<VideoEducation>, IOrderedQueryable<VideoEducation>>? orderBy = null, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            var videoEducations = await _videoEducationService.GetListAsync();
            return videoEducations;
        }

        public async Task<Paginate<VideoEducation>> GetPaginateAsync(Expression<Func<VideoEducation, bool>>? predicate = null, Func<IQueryable<VideoEducation>, IOrderedQueryable<VideoEducation>>? orderBy = null, bool include = false, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<VideoEducation> videoEducations = (IQueryable<VideoEducation>)_videoEducationService.GetListAsync();


            if (!withDeleted)
            {
                videoEducations = videoEducations.Where(c => !c.IsDeleted);
            }


            if (predicate != null)
            {
                videoEducations = videoEducations.Where(predicate);
            }

            if (!enableTracking)
            {
                videoEducations = videoEducations.AsNoTracking();
            }

            int totalItems = await videoEducations.CountAsync(cancellationToken);


            List<VideoEducation?> items = await videoEducations
                .Skip(index * size)
                .Take(size)
                .ToListAsync(cancellationToken);


            return new Paginate<VideoEducation?>
            {
                Items = items,
                Index = index,
                Size = size,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)size)
            };
        }

        public Task<VideoEducation> UpdateAsync(VideoEducation videoEducation)
        {
            throw new NotImplementedException();
        }
    }
}
