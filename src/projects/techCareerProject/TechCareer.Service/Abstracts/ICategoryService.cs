using Core.Persistence.Extensions;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TechCareer.Service.Abstracts
{
    public interface ICategoryService
    {
        Task<Category?> GetAsync(
            Expression <Func<Category,bool>> predicate,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
      );

        Task<Paginate<Category?>> GetPaginateAsync
            (Expression<Func<Category, bool>>? predicate = null,
            bool include = false,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cansellationToken = default);

        Task<List<Category>> GetListAsync(Expression<Func<Category, bool>>? predicate = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);

        Task<Category> AddAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task<Category> DeleteAsync(Category category, bool permanent = false);
    }
}
