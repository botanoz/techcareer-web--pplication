using Core.Persistence.Extensions;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TechCareer.Service.Abstracts
{
    public interface IEventService
    {
        Task<Event?> GetAsync(
    Expression<Func<Event, bool>> predicate,
    bool include = false,
    bool withDeleted = false,
    bool enableTracking = true,
    CancellationToken cancellationToken = default
);


        Task<Paginate<Event>> GetPaginateAsync(Expression<Func<Event, bool>>? predicate = null,
            Func<IQueryable<Event>, IOrderedQueryable<Event>>? orderBy = null,
            bool include = false,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);


        Task<List<Event>> GetListAsync(Expression<Func<Event, bool>>? predicate = null,
            Func<IQueryable<Event>, IOrderedQueryable<Event>>? orderBy = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);


        Task<Event> AddAsync(Event Event);
        Task<Event> UpdateAsync(Event Event);
        Task<Event> DeleteAsync(Event Event, bool permanent = false);

    }
}
