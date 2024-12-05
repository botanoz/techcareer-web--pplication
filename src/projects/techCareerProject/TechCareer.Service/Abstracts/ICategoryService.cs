using Core.Persistence.Extensions;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechCareer.Models.Dtos.Category;

namespace TechCareer.Service.Abstracts
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto?> GetAsync(
            Expression<Func<Category, bool>> predicate, 
            bool withDeleted = false, 
            CancellationToken cancellationToken = default
            );

        Task<Paginate<CategoryResponseDto?>> GetPaginateAsync
            (Expression<Func<Category, bool>>? predicate = null,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);

        Task<List<CategoryResponseDto>> GetListAsync(Expression<Func<Category, bool>>? predicate = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);

        Task<CategoryResponseDto> AddAsync(CategoryAddRequestDto categoryAddRequestDto);
        Task<CategoryResponseDto> UpdateAsync(CategoryUpdateRequestDto categoryUpdateRequestDto);
        Task<CategoryResponseDto> DeleteAsync(CategoryRequestDto categoryRequestDto, bool permanent = false);

        Task<CategoryResponseDto> FindCategoryAsync(CategoryRequestDto categoryRequestDto);
    }
}
