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
using TechCareer.Models.Dtos.Instructor;
using TechCareer.Service.Abstracts;

namespace TechCareer.Service.Concretes
{
    public class InstructorService : IInstructorService
    {

        private readonly IInstructorRepository _instructorRepository;

        public InstructorService(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public async Task<Instructor> AddAsync(Instructor Instructor)
        {
            Instructor addedInstructor = await _instructorRepository.AddAsync(Instructor);

            return addedInstructor;
        }

        public async Task<Instructor> DeleteAsync(InstructorDeleteRequestDto deleteRequestDto)
        {
            if (deleteRequestDto == null)
                throw new ArgumentNullException(nameof(deleteRequestDto), "Delete request data is required.");

            try
            {
                if (deleteRequestDto.Permanent)
                {
                    // Kalıcı silme: Veritabanından tamamen kaldır
                    var instructorToDelete = await _instructorRepository.GetAsync(x => x.Id == deleteRequestDto.Id);
                    if (instructorToDelete == null)
                        throw new KeyNotFoundException("Instructor not found.");

                    await _instructorRepository.DeleteAsync(instructorToDelete);
                    return instructorToDelete;
                }
                else
                {
                    // Yumuşak silme: IsDeleted alanını işaretle
                    var instructorToSoftDelete = await _instructorRepository.GetAsync(x => x.Id == deleteRequestDto.Id);
                    if (instructorToSoftDelete == null)
                        throw new KeyNotFoundException("Instructor not found.");

                    instructorToSoftDelete.IsDeleted = true;
                    await _instructorRepository.UpdateAsync(instructorToSoftDelete);
                    return instructorToSoftDelete;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw new ApplicationException("An error occurred while deleting the instructor.", ex);
            }
        }

        public async Task<Instructor?> GetAsync(Expression<Func<Instructor, bool>> predicate, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            try
            {
                var instructor = await _instructorRepository.GetAsync(predicate);
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
                var instructors = await _instructorRepository.GetListAsync();
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
                IQueryable<Instructor> instructors = (IQueryable<Instructor>)_instructorRepository.GetListAsync();

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

        public async Task<Instructor> UpdateAsync(Instructor instructor)
        {
            if (instructor == null)
                throw new ArgumentNullException(nameof(instructor), "Güncelleme için eğitmen bilgisi gereklidir.");

            try
            {
               
                var existingInstructor = await _instructorRepository.GetAsync(x => x.Id == instructor.Id);

                if (existingInstructor == null)
                    throw new KeyNotFoundException("Güncellenecek eğitmen bulunamadı.");


                existingInstructor.Name = instructor.Name;
                existingInstructor.About = instructor.About;
               
                var updatedInstructor = await _instructorRepository.UpdateAsync(existingInstructor);
                return updatedInstructor;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException("Eğitmen güncellenirken bir hata oluştu.", ex);
            }
        }


        public async Task<Instructor?> GetByIdAsync(Guid id)
        {
            try
            {
                var instructor = await _instructorRepository.GetAsync(x => x.Id == id);
                return instructor;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException(ex.Message, ex);
            }
        }
    }
}
