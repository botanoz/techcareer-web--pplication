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
        // Arrange
        var userPredicate = (Expression<Func<User, bool>>)(u => u.Email == "test@example.com");
        var expectedUser = new User { Id = 1, Email = "test@example.com" };

        _mockUserRepository.Setup(repo => repo.GetAsync(
            userPredicate,
            false, // include default değeri
            false, // withDeleted default değeri
            true,  // enableTracking default değeri
            default)) // CancellationToken default
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _userService.GetAsync(userPredicate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUser.Email, result?.Email);
    }

    [Fact]
    public async Task GetPaginateAsync_ShouldReturnPaginatedUsers_WhenUsersExist()
    {
        // Arrange
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
            null, // predicate
            null, // orderBy
            false, // include
            0,     // index
            10,    // size
            false, // withDeleted
            true,  // enableTracking
            default)) // CancellationToken default
            .ReturnsAsync(paginateResult);

        // Act
        var result = await _userService.GetPaginateAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userList.Count, result.Items.Count);
        Assert.Equal(1, result.Pages);
        Assert.Equal(0, result.Index);
    }

    [Fact]
    public async Task AddAsync_ShouldAddUser_WhenValidUserIsProvided()
    {
        // Arrange
        var newUser = new User { Id = 1, Email = "newuser@example.com" };

        _mockUserBusinessRules
            .Setup(r => r.UserEmailShouldNotExistWhenInsert(newUser.Email))
            .Verifiable();

        _mockUserRepository
            .Setup(r => r.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(newUser);

        // Act
        var result = await _userService.AddAsync(newUser);

        // Assert
        _mockUserBusinessRules.Verify();
        _mockUserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(newUser.Email, result.Email);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser_WhenValidUserIsProvided()
    {
        // Arrange
        var updatedUser = new User { Id = 1, Email = "updateduser@example.com" };

        _mockUserBusinessRules
            .Setup(r => r.UserEmailShouldNotExistWhenUpdate(updatedUser.Id, updatedUser.Email))
            .Verifiable();

        _mockUserRepository
            .Setup(r => r.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(updatedUser);

        // Act
        var result = await _userService.UpdateAsync(updatedUser);

        // Assert
        _mockUserBusinessRules.Verify();
        _mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(updatedUser.Email, result.Email);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteUser_WhenUserIsValid()
    {
        // Arrange
        var userToDelete = new User { Id = 1, Email = "deleteuser@example.com" };

        _mockUserRepository
            .Setup(repo => repo.DeleteAsync(It.IsAny<User>(), false)) // `permanent` default değer
            .ReturnsAsync(userToDelete);

        // Act
        var result = await _userService.DeleteAsync(userToDelete);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userToDelete.Id, result.Id);
        Assert.Equal(userToDelete.Email, result.Email);
    }
}
