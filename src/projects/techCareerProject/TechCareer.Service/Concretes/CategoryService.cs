using Core.Persistence.Extensions;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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
            //List<Category> listt = await _categoryRepository.GetListAsync(predicate, orderBy, include, withDeleted, enableTracking, cancellationToken);

            List<Category> list = await _categoryRepository.GetListAsync();

            return list;
        }


        public async Task<Category> AddAsync(Category category)
        {
           
          Category addedCategory = await _categoryRepository.AddAsync(category);
         
          return addedCategory;
        }



        public Task<Category> DeleteAsync(Category category, bool permanent = false)
        {

            //var categories = GetListAsync(x => x.Id == category.Id);


            //var deletedCategory = categories;
            //if (deletedCategory != null)
            //{
            //    if (permanent)
            //    {

            //        await PermanentDeleteAsync(deletedCategory);
            //    }
            //    else
            //    {

            //        deletedCategory.IsDeleted = true;
            //        await UpdateAsync(deletedCategory);
            //    }
            //}

            //return deletedCategory;

            return null;
        }

        private async Task<Category> FindCategoryAsync(Category category)
        {
            if (category != null)
            {
                var selectedCategory = await _categoryRepository.GetAsync(x => x.Id == category.Id);
                return selectedCategory;
            }
            else
            {
                return NullReferenceException("Aradığınız kategori bulunamamıştır.");
            }
           
        }

        private Category NullReferenceException(string v)
        {
            throw new NotImplementedException();
        }

        public Task<Category?> GetAsync(Expression<Func<Category, bool>> predicate, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }



        public Task<Paginate<Category?>> GetPaginateAsync(Expression<Func<Category, bool>>? predicate = null, bool include = false, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cansellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Category> UpdateAsync(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
