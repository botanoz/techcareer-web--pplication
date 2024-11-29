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
    public interface IJobService
    {
        Task<Job?> GetAsync(
Expression<Func<Job, bool>> predicate,
bool include = false,
bool withDeleted = false,
bool enableTracking = true,
CancellationToken cancellationToken = default
);


        Task<Paginate<Job>> GetPaginateAsync(Expression<Func<Job, bool>>? predicate = null,
            Func<IQueryable<Job>, IOrderedQueryable<Job>>? orderBy = null,
            bool include = false,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);


        Task<List<Job>> GetListAsync(Expression<Func<Job, bool>>? predicate = null,
            Func<IQueryable<Job>, IOrderedQueryable<Job>>? orderBy = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);


        Task<Job> AddAsync(Job job);
        Task<Job> UpdateAsync(Job job);
        Task<Job> DeleteAsync(Job job, bool permanent = false);

    }
}

