using Core.CrossCuttingConcerns.Exceptions.ExceptionTypes;
using Core.CrossCuttingConcerns.Rules;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Service.Constants;

namespace TechCareer.Service.Rules
{
    public class CategoryBusinessRules(ICategoryRepository _categoryRepository):BaseBusinessRules
    {
        public Task CategoryShouldBeExistsWhenSelected(Category? category)
        {
            if (category == null)
                throw new BusinessException(CategoryMessages.CategoryDontExists);
            return Task.CompletedTask;
        }

        public async Task CategoryIdShouldBeExistsWhenSelected(int id)
        {
            bool doesExist = await _categoryRepository.AnyAsync(predicate: c => c.Id == id, enableTracking:false);

            if (doesExist)
            {
                throw new BusinessException(CategoryMessages.CategoryDontExists);
            }
        }






    }
}
