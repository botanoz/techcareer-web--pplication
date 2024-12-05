using Core.Persistence.Extensions;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechCareer.Models.Dtos.Instructor;

namespace TechCareer.Service.Abstracts
{

    public interface IInstructorService
    {
        Task<Instructor?> GetAsync(
    Expression<Func<Instructor, bool>> predicate,
    bool include = false,
    bool withDeleted = false,
    bool enableTracking = true,
    CancellationToken cancellationToken = default
);


        Task<Paginate<Instructor>> GetPaginateAsync(Expression<Func<Instructor, bool>>? predicate = null,
            Func<IQueryable<Instructor>, IOrderedQueryable<Instructor>>? orderBy = null,
            bool include = false,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);


        Task<List<Instructor>> GetListAsync(Expression<Func<Instructor, bool>>? predicate = null,
            Func<IQueryable<Instructor>, IOrderedQueryable<Instructor>>? orderBy = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);


        Task<Instructor> AddAsync(Instructor Instructor);
        Task<Instructor> UpdateAsync(Instructor Instructor);
        Task<Instructor> DeleteAsync(InstructorDeleteRequestDto deleteRequestDto);
        Task<Instructor?> GetByIdAsync(Guid id);

    }
}
