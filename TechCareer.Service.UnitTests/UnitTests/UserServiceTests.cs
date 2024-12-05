using Core.Persistence.Extensions;
using Core.Security.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Concretes;
using TechCareer.Service.Rules;

namespace TechCareer.Service.Tests.UnitTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<UserBusinessRules> _mockUserBusinessRules;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUserBusinessRules = new Mock<UserBusinessRules>();
            _userService = new UserService(_mockUserBusinessRules.Object, _mockUserRepository.Object);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnUser_WhenUserExists()
        {
            var userPredicate = (Expression<Func<User, bool>>)(u => u.Email == "test@example.com");
            var expectedUser = new User { Id = 1, Email = "test@example.com" };

            _mockUserRepository
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedUser);

            var result = await _userService.GetAsync(userPredicate);

            Assert.NotNull(result);
            Assert.Equal(expectedUser.Email, result?.Email);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var userPredicate = (Expression<Func<User, bool>>)(u => u.Email == "nonexistent@example.com");
            _mockUserRepository
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            var result = await _userService.GetAsync(userPredicate);

            Assert.Null(result);
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
                Index = 1,
                Size = 10,
                Count = userList.Count,
                Pages = 1,
                TotalItems = userList.Count,
                TotalPages = 1
            };

            _mockUserRepository
                .Setup(r => r.GetPaginateAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginateResult);

            var result = await _userService.GetPaginateAsync();

            Assert.NotNull(result);
            Assert.Equal(userList.Count, result.Items.Count);
            Assert.Equal(1, result.Pages);
            Assert.Equal(10, result.Size);
            Assert.Equal(1, result.Index);
            Assert.False(result.HasNext);
            Assert.False(result.HasPrevious);
        }

        [Fact]
        public async Task AddAsync_ShouldAddUser_WhenValidUserIsProvided()
        {
            var newUser = new User { Id = 1, Email = "newuser@example.com" };

            _mockUserBusinessRules.Setup(r => r.UserEmailShouldNotExistsWhenInsert(It.IsAny<string>())).Verifiable();
            _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync(newUser);

            var result = await _userService.AddAsync(newUser);

            _mockUserBusinessRules.Verify();
            _mockUserRepository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(newUser.Email, result.Email);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUser_WhenValidUserIsProvided()
        {
            var updatedUser = new User { Id = 1, Email = "updateduser@example.com" };

            _mockUserBusinessRules.Setup(r => r.UserEmailShouldNotExistsWhenUpdate(It.IsAny<int>(), It.IsAny<string>())).Verifiable();
            _mockUserRepository.Setup(r => r.UpdateAsync(It.IsAny<User>())).ReturnsAsync(updatedUser);

            var result = await _userService.UpdateAsync(updatedUser);

            _mockUserBusinessRules.Verify();
            _mockUserRepository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(updatedUser.Email, result.Email);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteUser_WhenUserIsValid()
        {
            var userToDelete = new User { Id = 1, Email = "deleteuser@example.com" };
            _mockUserRepository.Setup(r => r.DeleteAsync(It.IsAny<User>(), It.IsAny<bool>())).ReturnsAsync(userToDelete);

            var result = await _userService.DeleteAsync(userToDelete);

            Assert.NotNull(result);
            Assert.Equal(userToDelete.Email, result.Email);
            Assert.Equal(userToDelete.Id, result.Id);
        }
    }
}


