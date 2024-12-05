using Core.Persistence.Extensions;
using Core.Persistence.Repositories;
using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Service.Abstracts;

namespace TechCareer.Service.Concretes
{
    public class VideoEducationService : IVideoEducationService
    {
        private readonly IVideoEducationRepository _videoEducationRepository;

        public VideoEducationService(IVideoEducationRepository videoEducationRepository)
        {
            _videoEducationRepository = videoEducationRepository;
        }

        public async Task<VideoEducation?> GetAsync(
            Expression<Func<VideoEducation, bool>> predicate,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        )
        {
            var videoEducation = await _videoEducationRepository.GetAsync(
                predicate,
                include,
                withDeleted,
                enableTracking,
                cancellationToken
            );

            return videoEducation;
        }

        public async Task<List<VideoEducation>> GetListAsync(
            Expression<Func<VideoEducation, bool>>? predicate = null,
            Func<IQueryable<VideoEducation>, IOrderedQueryable<VideoEducation>>? orderBy = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        )
        {
            var videoEducations = await _videoEducationRepository.GetListAsync(
                predicate,
                orderBy,
                include,
                withDeleted,
                enableTracking,
                cancellationToken
            );

            return videoEducations;
        }

        public async Task<Paginate<VideoEducation>> GetPaginateAsync(
            Expression<Func<VideoEducation, bool>>? predicate = null,
            Func<IQueryable<VideoEducation>, IOrderedQueryable<VideoEducation>>? orderBy = null,
            bool include = false,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        )
        {
            var paginateResult = await _videoEducationRepository.GetPaginateAsync(
                predicate,
                orderBy,
                include,
                index,
                size,
                withDeleted,
                enableTracking,
                cancellationToken
            );

            return paginateResult;
        }

        public async Task<VideoEducation> AddAsync(VideoEducation videoEducation)
        {
            var addedVideoEducation = await _videoEducationRepository.AddAsync(videoEducation);
            return addedVideoEducation;
        }

        public async Task<VideoEducation> UpdateAsync(VideoEducation videoEducation)
        {
            var updatedVideoEducation = await _videoEducationRepository.UpdateAsync(videoEducation);
            return updatedVideoEducation;
        }

        public async Task<VideoEducation> DeleteAsync(VideoEducation videoEducation, bool permanent = false)
        {
            var deletedVideoEducation = await _videoEducationRepository.DeleteAsync(videoEducation, permanent);
            return deletedVideoEducation;
        }
    }
}
