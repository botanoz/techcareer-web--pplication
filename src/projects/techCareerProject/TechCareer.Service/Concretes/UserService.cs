using System.Linq.Expressions;
using Core.AOP.Aspects;
using Core.Persistence.Extensions;
using Core.Security.Entities;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Rules;

namespace TechCareer.Service.Concretes;
public sealed class UserService(IUserRepository _userRepository, UserBusinessRules _userBusinessRules) : IUserService
{
    public async Task<User?> GetAsync(Expression<Func<User, bool>> predicate, bool include = false, bool withDeleted = false, bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return user;
    }

    
    //[CacheAspect(bypassCache:false, cacheKeyTemplate:"Users({index},{size})",cacheGroupKey:"Users",Priority = 3)]
    public async Task<Paginate<User>> GetPaginateAsync(Expression<Func<User, bool>>? predicate = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, bool include = false, int index = 0,
        int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        Paginate<User> userList = await _userRepository.GetPaginateAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return userList;
    }

    public async Task<List<User>> GetListAsync(Expression<Func<User, bool>>? predicate = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, bool include = false, bool withDeleted = false,
        bool enableTracking = false, CancellationToken cancellationToken = default)
    {
        List<User> userList = await _userRepository.GetListAsync(
            predicate,orderBy,include,withDeleted,enableTracking,cancellationToken
        );
        return userList;
    }

    public async Task<User> AddAsync(User user)
    {
        await _userBusinessRules.UserEmailShouldNotExistsWhenInsert(user.Email);
        
        User addedUser = await _userRepository.AddAsync(user);
        
        return addedUser;
    }

    public async Task<User> UpdateAsync(User user)
    {
        await _userBusinessRules.UserEmailShouldNotExistsWhenUpdate(user.Id, user.Email);

        User updatedUser = await _userRepository.UpdateAsync(user);

        return updatedUser;
    }
    
    public async Task<User> DeleteAsync(User user, bool permanent = false)
    {
        User deletedUser = await _userRepository.DeleteAsync(user,permanent);

        return deletedUser;
    }
}