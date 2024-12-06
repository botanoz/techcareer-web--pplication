using Moq;
using System.Linq.Expressions;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Concretes;
using Core.Security.Entities;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Service.Rules;
using Core.Persistence.Extensions;

namespace TechCareer.Service.Tests.UnitTests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IUserBusinessRules> _mockUserBusinessRules;
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockUserBusinessRules = new Mock<IUserBusinessRules>();
        _userService = new UserService(_mockUserBusinessRules.Object, _mockUserRepository.Object);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnUser_WhenUserExists()
    {
       
        var userPredicate = (Expression<Func<User, bool>>)(u => u.Email == "test@example.com");
        var expectedUser = new User { Id = 1, Email = "test@example.com" };

        _mockUserRepository.Setup(repo => repo.GetAsync(
            userPredicate,
            false,
            false, 
            true,  
            default)) 
            .ReturnsAsync(expectedUser);

      
        var result = await _userService.GetAsync(userPredicate);

        Assert.NotNull(result);
        Assert.Equal(expectedUser.Email, result?.Email);
    }

    [Fact]
    public async Task GetPaginateAsync_ShouldReturnPaginatedUsers_WhenUsersExist()
    {
      
        var userList = new List<User>
        {
            new User { Id = 1, Email = "user1@example.com" },
            new User { Id = 2, Email = "user2@example.com" }
        };

        var paginateResult = new Paginate<User>
        {
            Items = userList,
            Index = 0,
            Size = 10,
            Count = userList.Count,
            Pages = 1,
            TotalItems = userList.Count,
            TotalPages = 1
        };

        _mockUserRepository.Setup(repo => repo.GetPaginateAsync(
            null, 
            null, 
            false, 
            0,    
            10,    
            false, 
            true, 
            default)) 
            .ReturnsAsync(paginateResult);

    
        var result = await _userService.GetPaginateAsync();

       
        Assert.NotNull(result);
        Assert.Equal(userList.Count, result.Items.Count);
        Assert.Equal(1, result.Pages);
        Assert.Equal(0, result.Index);
    }

    [Fact]
    public async Task AddAsync_ShouldAddUser_WhenValidUserIsProvided()
    {
      
        var newUser = new User { Id = 1, Email = "newuser@example.com" };

        _mockUserBusinessRules
            .Setup(r => r.UserEmailShouldNotExistWhenInsert(newUser.Email))
            .Verifiable();

        _mockUserRepository
            .Setup(r => r.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(newUser);

     
        var result = await _userService.AddAsync(newUser);

      
        _mockUserBusinessRules.Verify();
        _mockUserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(newUser.Email, result.Email);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser_WhenValidUserIsProvided()
    {
      
        var updatedUser = new User { Id = 1, Email = "updateduser@example.com" };

        _mockUserBusinessRules
            .Setup(r => r.UserEmailShouldNotExistWhenUpdate(updatedUser.Id, updatedUser.Email))
            .Verifiable();

        _mockUserRepository
            .Setup(r => r.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(updatedUser);


        var result = await _userService.UpdateAsync(updatedUser);

        _mockUserBusinessRules.Verify();
        _mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(updatedUser.Email, result.Email);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteUser_WhenUserIsValid()
    {
      
        var userToDelete = new User { Id = 1, Email = "deleteuser@example.com" };

        _mockUserRepository
            .Setup(repo => repo.DeleteAsync(It.IsAny<User>(), false)) 
            .ReturnsAsync(userToDelete);

   
        var result = await _userService.DeleteAsync(userToDelete);

      
        Assert.NotNull(result);
        Assert.Equal(userToDelete.Id, result.Id);
        Assert.Equal(userToDelete.Email, result.Email);
    }
}
