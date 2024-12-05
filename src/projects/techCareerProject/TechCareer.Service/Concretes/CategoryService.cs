using Core.Persistence.Extensions;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Dtos.Category;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Rules;

namespace TechCareer.Service.Concretes
{
    public sealed class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly CategoryBusinessRules _categoryBusinessRules;

        public CategoryService(ICategoryRepository categoryRepository, CategoryBusinessRules categoryBusinessRules)
        {
            _categoryRepository = categoryRepository;
            _categoryBusinessRules = categoryBusinessRules;
        }

        // Add a new category
        public async Task<CategoryResponseDto> AddAsync(CategoryAddRequestDto categoryAddRequestDto)
        {
            Category c = new Category(categoryAddRequestDto.Name);
            // Check business rules
            await _categoryBusinessRules.CategoryShouldBeExistsWhenSelected(c);

            // Create and save the category
            var category = new Category(categoryAddRequestDto.Name);
            var addedCategory = await _categoryRepository.AddAsync(category);

            // Return response DTO
            return new CategoryResponseDto
            {
                Id = addedCategory.Id,
                Name = addedCategory.Name
            };
        }

        // Delete a category
        public async Task<CategoryResponseDto> DeleteAsync(CategoryRequestDto categoryRequestDto, bool permanent = false)
        {
            var category = await _categoryRepository.GetAsync(x => x.Id == categoryRequestDto.Id, withDeleted: true);

            if (category == null)
                throw new ApplicationException("Category not found.");

            if (permanent)
            {
                await _categoryRepository.DeleteAsync(category, true);
            }
            else
            {
                category.IsDeleted = true;
                await _categoryRepository.UpdateAsync(category);
            }

            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        // Find a category by ID
        public async Task<CategoryResponseDto> FindCategoryAsync(CategoryRequestDto categoryRequestDto)
        {
            var category = await _categoryRepository.GetAsync(x => x.Id == categoryRequestDto.Id);

            if (category == null)
                throw new ApplicationException("Category not found.");

            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        // Get a single category with optional filters
        public async Task<CategoryResponseDto?> GetAsync(
            Expression<Func<Category, bool>> predicate,
            bool withDeleted = false, 
            CancellationToken cancellationToken = default
        )
        {
            var category = await _categoryRepository.GetAsync(predicate, withDeleted: withDeleted);

            if (category == null)
                return null;

            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        // Get all categories with optional filters
        public async Task<List<CategoryResponseDto>> GetListAsync(
            Expression<Func<Category, bool>>? predicate = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            var categories = await _categoryRepository.GetListAsync(
                predicate,
                enableTracking: enableTracking,
                withDeleted: true);

            var filteredCategories = withDeleted
                ? categories
                : categories.Where(category => !category.IsDeleted).ToList();

            return filteredCategories.Select(category => new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name
            }).ToList();
        }

        // Get paginated list of categories
        public async Task<Paginate<CategoryResponseDto>> GetPaginateAsync(
            Expression<Func<Category, bool>>? predicate = null,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            var paginateResult = await _categoryRepository.GetPaginateAsync(predicate, index: index, size: size, enableTracking: enableTracking, withDeleted: withDeleted);

            return new Paginate<CategoryResponseDto>
            {
                Items = paginateResult.Items.Select(category => new CategoryResponseDto
                {
                    Id = category.Id,
                    Name = category.Name
                }).ToList(),
                Index = paginateResult.Index,
                Size = paginateResult.Size,
                TotalItems = paginateResult.TotalItems,
                TotalPages = paginateResult.TotalPages
            };
        }

        // Update a category
        public async Task<CategoryResponseDto> UpdateAsync(CategoryUpdateRequestDto categoryUpdateRequestDto)
        {
            var category = await _categoryRepository.GetAsync(x => x.Id == categoryUpdateRequestDto.Id);

            if (category == null)
                throw new ApplicationException("Category not found.");

            // Update fields
            category.Name = categoryUpdateRequestDto.Name;
            await _categoryRepository.UpdateAsync(category);

            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
