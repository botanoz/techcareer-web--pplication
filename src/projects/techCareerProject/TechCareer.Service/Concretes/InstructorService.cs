using Core.Persistence.Extensions;
using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechCareer.Service.Abstracts;

namespace TechCareer.Service.Concretes
{
    public class InstructorService : IInstructorService
    {

        private readonly IInstructorService _instructorService;


        public async Task<Instructor> AddAsync(Instructor Instructor)
        {
            Instructor addedInstructor = await _instructorService.AddAsync(Instructor);

            return addedInstructor;
        }

        public async Task<Instructor> DeleteAsync(Instructor Instructor, bool permanent = false)
        {
            try
            {
                var deletedInstructor = (await GetListAsync(x => x.Id == Instructor.Id)).FirstOrDefault();

                deletedInstructor.IsDeleted = true;

                return deletedInstructor;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException(ex.Message, ex);
            }

        }

        public async Task<Instructor?> GetAsync(Expression<Func<Instructor, bool>> predicate, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            try
            {
                var instructor = await _instructorService.GetAsync(predicate);
                return instructor;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException(ex.Message, ex);
            }

        }

        public async Task<List<Instructor>> GetListAsync(Expression<Func<Instructor, bool>>? predicate = null, Func<IQueryable<Instructor>, IOrderedQueryable<Instructor>>? orderBy = null, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            try
            {
                var instructors = await _instructorService.GetListAsync();
                return instructors;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException(ex.Message, ex);
            }

        }

        public async Task<Paginate<Instructor>> GetPaginateAsync(Expression<Func<Instructor, bool>>? predicate = null, Func<IQueryable<Instructor>, IOrderedQueryable<Instructor>>? orderBy = null, bool include = false, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            try
            {
                IQueryable<Instructor> instructors = (IQueryable<Instructor>)_instructorService.GetListAsync();

                if (!withDeleted)
                    instructors = instructors.Where(c => !c.IsDeleted);
                if (predicate != null)
                    instructors = instructors.Where(predicate);
                if (!enableTracking)
                    instructors = instructors.AsNoTracking();

                int totalItems = await instructors.CountAsync(cancellationToken);

                List<Instructor> items = await instructors
                    .Skip(index * size)
                    .Take(size)
                    .ToListAsync(cancellationToken);

                return new Paginate<Instructor>
                {
                    Items = items,
                    Index = index,
                    Size = size,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)size)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException(ex.Message, ex);
            }
        }

        public async Task<Instructor> UpdateAsync(Instructor Instructor)
        {
            //var existingInstructor = await _instructorService.GetAsync(Instructor.Id);
            //if (existingInstructor == null) throw new KeyNotFoundException("Instructor not found.");

            //_dbContext.Entry(existingInstructor).CurrentValues.SetValues(instructor);
            //await _dbContext.SaveChangesAsync();

            //return existingInstructor;
            return null;
        }
    }
}
