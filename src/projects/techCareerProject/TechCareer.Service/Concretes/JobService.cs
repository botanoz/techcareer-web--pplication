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
using TechCareer.DataAccess.Repositories.Concretes;
using TechCareer.Service.Abstracts;

namespace TechCareer.Service.Concretes
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public async Task<Job> AddAsync(Job job)
        {
            Job addedJob = await _jobRepository.AddAsync(job);

            return addedJob;
        }

        public async Task<Job> DeleteAsync(Job job, bool permanent = false)
        {
           var selectedJob = (await GetListAsync(x=>x.Id == job.Id)).FirstOrDefault();
            selectedJob.IsDeleted = true;

            return selectedJob;
        }

        public async Task<Job?> GetAsync(Expression<Func<Job, bool>> predicate, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            //var selectedEvent = (await _eventRepository.GetListAsync(x => x.Id == Job.Id)).FirstOrDefault();

            //return selectedEvent;
            return null;
        }

        public async Task<List<Job>> GetListAsync(Expression<Func<Job, bool>>? predicate = null, Func<IQueryable<Job>, IOrderedQueryable<Job>>? orderBy = null, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            List<Job> jobList = await _jobRepository.GetListAsync();

            return jobList;
        }

        public async Task<Paginate<Job>> GetPaginateAsync(Expression<Func<Job, bool>>? predicate = null, Func<IQueryable<Job>, IOrderedQueryable<Job>>? orderBy = null, bool include = false, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<Job> query = (IQueryable<Job>)_jobRepository.GetListAsync();


            if (!withDeleted)
                query = query.Where(c => !c.IsDeleted);
            if (predicate != null)
                query = query.Where(predicate);
            if (!enableTracking)
                query = query.AsNoTracking();

            int totalItems = await query.CountAsync(cancellationToken);


            List<Job> items = await query
                .Skip(index * size)
                .Take(size)
                .ToListAsync(cancellationToken);


            return new Paginate<Job>
            {
                Items = items,
                Index = index,
                Size = size,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)size)
            };
        }

        public async Task<Job> UpdateAsync(Job job)
        {
            throw new NotImplementedException();
        }
    }
}
