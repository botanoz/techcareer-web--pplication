using Core.Persistence.Extensions;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechCareer.Models.Dtos.Category;
using TechCareer.Models.Dtos.Event;

namespace TechCareer.Service.Abstracts
{
    public interface IEventService
    {
        Task<EventResponseDto?> GetAsync(
            Expression<Func<Event, bool>> predicate,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        );


        Task<Paginate<EventResponseDto>> GetPaginateAsync(Expression<Func<Event, bool>>? predicate = null,
            Func<IQueryable<Event>, IOrderedQueryable<Event>>? orderBy = null,
            bool include = false,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);


        Task<List<EventResponseDto>> GetListAsync(Expression<Func<Event, bool>>? predicate = null,
            Func<IQueryable<Event>, IOrderedQueryable<Event>>? orderBy = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);


        Task<EventResponseDto> AddAsync(EventAddRequestDto eventAddRequestDto);
        Task<EventResponseDto> UpdateAsync(EventUpdateRequestDto eventUpdateRequestDto);
        Task<EventResponseDto> DeleteAsync(EventRequestDto eventRequestDto, bool permanent = false);
        Task<EventResponseDto> FindEventAsync(EventRequestDto eventRequestDto);
    }
} 
