using System.Linq.Expressions;
using Core.Persistence.Extensions;
using Core.Security.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace TechCareer.Service.Abstracts;

public interface IUserService
{
    Task<User?> GetAsync(
        Expression<Func<User, bool>> predicate,
        bool include = false,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    
    
    Task<Paginate<User>> GetPaginateAsync(Expression<Func<User, bool>>? predicate = null,
        Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
        bool include = false,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default);
    
    
    Task<List<User>> GetListAsync(Expression<Func<User, bool>>? predicate = null,
        Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
        bool include = false,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default);
    
    
    Task<User> AddAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<User> DeleteAsync(User user, bool permanent = false);
}