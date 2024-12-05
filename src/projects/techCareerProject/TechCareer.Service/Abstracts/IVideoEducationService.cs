using Core.Persistence.Extensions;
using Core.Persistence.Repositories;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TechCareer.Models.Dtos.VideoEducation;

namespace TechCareer.Service.Abstracts
{
    public interface IVideoEducationService
    {
        Task<VideoEducationResponseDto?> GetAsync(
            Expression<Func<VideoEducation, bool>> predicate,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        );

        Task<Paginate<VideoEducationResponseDto>> GetPaginateAsync(
            Expression<Func<VideoEducation, bool>>? predicate = null,
            Func<IQueryable<VideoEducation>, IOrderedQueryable<VideoEducation>>? orderBy = null,
            bool include = false,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        );

        Task<List<VideoEducationResponseDto>> GetListAsync(
            Expression<Func<VideoEducation, bool>>? predicate = null,
            Func<IQueryable<VideoEducation>, IOrderedQueryable<VideoEducation>>? orderBy = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        );

        Task<VideoEducationResponseDto> AddAsync(VideoEducationAddRequestDto videoEducationAddRequestDto);
        Task<VideoEducationResponseDto> UpdateAsync(VideoEducationUpdateRequestDto videoEducationUpdateRequestDto);
        Task<VideoEducationResponseDto> DeleteAsync(VideoEducationRequestDto videoEducationRequestDto, bool permanent = false);
    }
}
