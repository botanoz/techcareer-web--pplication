using Core.Persistence.Extensions;
using Core.Security.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
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

        public async Task<List<Category>> GetListAsync(Expression<Func<Category, bool>>? predicate = null, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            try
            {
                List<Category> list = await _categoryRepository.GetListAsync();
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException("Kategori listesi getirilemedi.", ex);
            }
        }

        public async Task<Category> AddAsync(Category category)
        {
            try
            {
                Category addedCategory = await _categoryRepository.AddAsync(category);

                return addedCategory;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException("Kategori eklenemedi.", ex);
            }

        }

        public async Task<Category> DeleteAsync(Category category, bool permanent = false)
        {
            try
            {
                var selectedCategory = (await GetListAsync(x => x.Id == category.Id)).FirstOrDefault();

                selectedCategory.IsDeleted = true;

                return selectedCategory;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException("Kategori silinemedi.", ex);
            }
        }

        public async Task<Category?> GetAsync(Expression<Func<Category, bool>> predicate, Category category, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            try
            {
                var selectedCategory = (await _categoryRepository.GetListAsync(x => x.Id == category.Id)).FirstOrDefault();

                return selectedCategory;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException("Kategori getirilemedi.", ex);
            }

        }

        public async Task<Paginate<Category?>> GetPaginateAsync(
                          Expression<Func<Category, bool>>? predicate = null,
                          bool include = false,
                          int index = 0,
                          int size = 10,
                          bool withDeleted = false,
                          bool enableTracking = true,
                          CancellationToken cancellationToken = default)
        {
            try
            {
                IQueryable<Category> query = (IQueryable<Category>)_categoryRepository.GetListAsync();

                if (!withDeleted)
                    query = query.Where(c => !c.IsDeleted);
                if (predicate != null)
                    query = query.Where(predicate);
                if (!enableTracking)
                    query = query.AsNoTracking();


                int totalItems = await query.CountAsync(cancellationToken);
                List<Category> items = await query
                    .Skip(index * size)
                    .Take(size)
                    .ToListAsync(cancellationToken);

                return new Paginate<Category?>
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

        public async Task<Category> UpdateAsync(Category category)
        {
            try
            {
                var updatedCategory = (await GetListAsync(x => x.Id == category.Id)).FirstOrDefault();
                if (updatedCategory != null)
                {
                    updatedCategory = category;
                    return updatedCategory;
                }
                else
                {
                    return NullReferenceException("Aradığınız kategori bulunamamıştır.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException(ex.Message, ex);
            }
        }

        public async Task<Category> FindCategoryAsync(Category category)
        {
            try
            {
                if (category != null)
                {
                    var deletedCategory = (await GetListAsync(x => x.Id == category.Id)).FirstOrDefault();
                    return deletedCategory;
                }
                else
                {
                    return NullReferenceException("Aradığınız kategori bulunamamıştır.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException(ex.Message, ex);
            }
        }
        private Category NullReferenceException(string v)
        {
            throw new NotImplementedException();
        }
    }
}
