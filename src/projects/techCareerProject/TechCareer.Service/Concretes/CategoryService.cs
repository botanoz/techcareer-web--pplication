using Core.CrossCuttingConcerns.Serilog;
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
        private readonly LoggerServiceBase _logger;

        public CategoryService(ICategoryRepository categoryRepository, CategoryBusinessRules categoryBusinessRules, LoggerServiceBase logger)
        {
            _categoryRepository = categoryRepository;
            _categoryBusinessRules = categoryBusinessRules;
            _logger = logger;
        }

        // Add a new category
        public async Task<CategoryResponseDto> AddAsync(CategoryAddRequestDto categoryAddRequestDto)
        {
            try
            {
                Category c = new Category(categoryAddRequestDto.Name);
                // Check business rules
                await _categoryBusinessRules.CategoryShouldBeExistsWhenSelected(c);

                // Create and save the category
                var category = new Category(categoryAddRequestDto.Name);
                var addedCategory = await _categoryRepository.AddAsync(category);

                _logger.Info("Info log: Category added.");

                // Return response DTO
                return new CategoryResponseDto
                {
                    Id = addedCategory.Id,
                    Name = addedCategory.Name
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

        }

        // Delete a category
        public async Task<CategoryResponseDto> DeleteAsync(CategoryRequestDto categoryRequestDto, bool permanent = false)
        {
            try
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
                    await _categoryRepository.DeleteAsync(category);
                }

                _logger.Info("Info log: Category deleted.");

                return new CategoryResponseDto
                {
                    Id = category.Id,
                    Name = category.Name
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

        }

        // Find a category by ID
        public async Task<CategoryResponseDto> FindCategoryAsync(CategoryRequestDto categoryRequestDto)
        {
            try
            {
                var category = await _categoryRepository.GetAsync(x => x.Id == categoryRequestDto.Id);

                if (category == null)
                {
                    _logger.Warn("Category not found.");
                    throw new ApplicationException("Category not found.");
                }


                return new CategoryResponseDto
                {
                    Id = category.Id,
                    Name = category.Name
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

        }

        // Get a single category with optional filters
        public async Task<CategoryResponseDto?> GetAsync(
            Expression<Func<Category, bool>> predicate,
            bool withDeleted = false, 
            CancellationToken cancellationToken = default
        )
        {
            try
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
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

        }

        // Get all categories with optional filters
        public async Task<List<CategoryResponseDto>> GetListAsync(
            Expression<Func<Category, bool>>? predicate = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            try
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
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

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
            try
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
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

        }

        // Update a category
        public async Task<CategoryResponseDto> UpdateAsync(CategoryUpdateRequestDto categoryUpdateRequestDto)
        {
            try
            {
                var category = await _categoryRepository.GetAsync(x => x.Id == categoryUpdateRequestDto.Id);

                if (category == null)
                {
                    _logger.Warn("Warn log: Category not found..");
                    throw new ApplicationException("Category not found.");
                }


                // Update fields
                category.Name = categoryUpdateRequestDto.Name;
                await _categoryRepository.UpdateAsync(category);

                _logger.Info("Info log: Category updated.");

                return new CategoryResponseDto
                {
                    Id = category.Id,
                    Name = category.Name
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

        }
    }
}
