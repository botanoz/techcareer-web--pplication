using Core.Persistence.Extensions;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.DataAccess.Repositories.Concretes;
using TechCareer.Models.Dtos.Event;
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

        // Tek bir eğitmen döner
        public async Task<InstructorResponseDto?> GetAsync(
            Expression<Func<Instructor, bool>> predicate,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            var instructor = await _instructorRepository.GetAsync(predicate, withDeleted: withDeleted, enableTracking: enableTracking);

            if (instructor == null)
                return null;

            return new InstructorResponseDto
            {
                Id = instructor.Id,
                Name = instructor.Name,
                About = instructor.About
            };
        }

        // Eğitmenlerin sayfalandırılmış listesi
        public async Task<Paginate<InstructorResponseDto>> GetPaginateAsync(
            Expression<Func<Instructor, bool>>? predicate = null,
            Func<IQueryable<Instructor>, IOrderedQueryable<Instructor>>? orderBy = null,
            bool include = false,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            var paginateResult = await _instructorRepository.GetPaginateAsync(
                predicate,
                index: index,
                size: size,
                enableTracking: enableTracking,
                withDeleted: withDeleted
            );

            return new Paginate<InstructorResponseDto>
            {
                Items = paginateResult.Items.Select(instructor => new InstructorResponseDto
                {
                    Id = instructor.Id,
                    Name = instructor.Name,
                    About = instructor.About
                }).ToList(),
                Index = paginateResult.Index,
                Size = paginateResult.Size,
                TotalItems = paginateResult.TotalItems,
                TotalPages = paginateResult.TotalPages
            };
        }

        // Eğitmenlerin listesi
        public async Task<List<InstructorResponseDto>> GetListAsync(
            Expression<Func<Instructor, bool>>? predicate = null,
            Func<IQueryable<Instructor>, IOrderedQueryable<Instructor>>? orderBy = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            var instructors = await _instructorRepository.GetListAsync(
                predicate,
                enableTracking: enableTracking,
                withDeleted: withDeleted
            );

            var filteredInstructors = withDeleted
                ? instructors
                : instructors.Where(i => !i.IsDeleted).ToList();

            return filteredInstructors.Select(i => new InstructorResponseDto
            {
                Id = i.Id,
                Name = i.Name,
                About = i.About
            }).ToList();
        }


        // Yeni eğitmen ekler
        public async Task<InstructorResponseDto> AddAsync(InstructorAddRequestDto instructorAddRequestDto)
        {
            try
            {
                Instructor instructorEntity = new Instructor
                (
                    instructorAddRequestDto.Name,
                    instructorAddRequestDto.About
                );

                var addedInstructor = await _instructorRepository.AddAsync(instructorEntity);

                return new InstructorResponseDto
                {
                    Id = addedInstructor.Id,
                    Name = addedInstructor.Name,
                    About = addedInstructor.About
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException("Eğitmen eklenemedi", ex);
            }
        }


        // Eğitmen günceller
        public async Task<InstructorResponseDto> UpdateAsync(InstructorUpdateRequestDto instructorUpdateRequestDto)
        {
            var instructor = await _instructorRepository.GetAsync(x => x.Id == instructorUpdateRequestDto.Id);

            if (instructor == null)
                throw new ApplicationException("Instructor not found.");

            instructor.Name = instructorUpdateRequestDto.Name;
            instructor.About = instructorUpdateRequestDto.About;

            var updatedInstructor = await _instructorRepository.UpdateAsync(instructor);

            return new InstructorResponseDto
            {
                Id = updatedInstructor.Id,
                Name = updatedInstructor.Name,
                About = updatedInstructor.About
            };
        }

        // Eğitmen siler
        public async Task<InstructorResponseDto> DeleteAsync(InstructorRequestDto instructorRequestDto, bool permanent = false)
        {
            var selectedInstructor = await _instructorRepository.GetAsync(
                x => x.Id == instructorRequestDto.Id,
                withDeleted: true
            );

            if (selectedInstructor == null)
                throw new ApplicationException("Instructor not found.");

            if (permanent)
            {
                await _instructorRepository.DeleteAsync(selectedInstructor, true);
            }
            else
            {
                selectedInstructor.IsDeleted = true;
                await _instructorRepository.DeleteAsync(selectedInstructor);
            }

            return new InstructorResponseDto
            {
                Id = selectedInstructor.Id,
                Name = selectedInstructor.Name,
                About = selectedInstructor.About
            };
        }


        // Eğitmen bulur
        public async Task<InstructorResponseDto?> FindInstructorAsync(InstructorRequestDto instructorRequestDto)
        {
            var instructor = await _instructorRepository.GetAsync(x => x.Id == instructorRequestDto.Id);

            if (instructor == null)
                return null;

            return new InstructorResponseDto
            {
                Id = instructor.Id,
                Name = instructor.Name,
                About = instructor.About
            };
        }
    }
}
