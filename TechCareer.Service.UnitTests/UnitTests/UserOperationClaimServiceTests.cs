using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TechCareer.Service.Abstracts;
using Core.Security.Entities;
using Core.Security.Dtos;
using Xunit;
using TechCareer.Service.Concretes;

namespace TechCareer.Service.Tests
{
    public class UserOperationClaimServiceTests
    {
        private readonly Mock<IUserOperationClaimService> _mockUserOperationClaimService;
        private readonly UserOperationClaimService _userOperationClaimService;

        public UserOperationClaimServiceTests()
        {
            _mockUserOperationClaimService = new Mock<IUserOperationClaimService>();
            _userOperationClaimService = new UserOperationClaimService(_mockUserOperationClaimService.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnAddedUserOperationClaim()
        {
            // Arrange
            var newClaim = new UserOperationClaim(1, 2) // UserId, OperationClaimId
            {
                IsDeleted = false,
                User = new User(1, "John", "Doe", "john.doe@example.com", new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6 }, true),
                OperationClaim = new OperationClaim { Id = 2, Name = "Admin" }
            };

            _mockUserOperationClaimService
                .Setup(service => service.AddAsync(It.IsAny<UserOperationClaim>()))
                .ReturnsAsync(newClaim);

            // Act
            var result = await _userOperationClaimService.AddAsync(newClaim);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
            Assert.Equal(2, result.OperationClaimId);
            Assert.False(result.IsDeleted);
            Assert.Equal("John", result.User.FirstName);
            Assert.Equal("Doe", result.User.LastName);
            Assert.Equal("Admin", result.OperationClaim.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnDeletedUserOperationClaim()
        {
            // Arrange
            var existingClaim = new UserOperationClaim(1, 2)
            {
                IsDeleted = false,
                User = new User(1, "John", "Doe", "john.doe@example.com", new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6 }, true),
                OperationClaim = new OperationClaim { Id = 2, Name = "Admin" }
            };

            _mockUserOperationClaimService
                .Setup(service => service.GetListAsync(
                    It.IsAny<Expression<Func<UserOperationClaim, bool>>>(),
                    It.IsAny<Func<IQueryable<UserOperationClaim>, IOrderedQueryable<UserOperationClaim>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(new List<UserOperationClaim> { existingClaim });


            // Act
            var result = await _userOperationClaimService.DeleteAsync(existingClaim);

            // Assert
            Assert.True(result.IsDeleted);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnUserOperationClaim()
        {
            // Arrange
            var existingClaim = new UserOperationClaim(1, 2)
            {
                IsDeleted = false,
                User = new User(1, "John", "Doe", "john.doe@example.com", new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6 }, true),
                OperationClaim = new OperationClaim { Id = 2, Name = "Admin" }
            };

            _mockUserOperationClaimService
        .Setup(service => service.GetAsync(
            It.IsAny<Expression<Func<UserOperationClaim, bool>>>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()
        ))
        .ReturnsAsync(existingClaim);


            // Act
            var result = await _userOperationClaimService.GetAsync(x => x.UserId == 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Admin", result.OperationClaim.Name);
            Assert.Equal("John", result.User.FirstName);
            Assert.Equal("Doe", result.User.LastName);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedUserOperationClaim()
        {
            // Arrange
            var existingClaim = new UserOperationClaim(1, 2)
            {
                IsDeleted = false,
                User = new User(1, "John", "Doe", "john.doe@example.com", new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6 }, true),
                OperationClaim = new OperationClaim { Id = 2, Name = "Admin" }
            };
            var updatedClaim = new UserOperationClaim(1, 3) // Updated OperationClaimId
            {
                IsDeleted = false,
                User = new User(1, "John", "Doe", "john.doe@example.com", new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6 }, true),
                OperationClaim = new OperationClaim { Id = 3, Name = "Super Admin" }
            };
            _mockUserOperationClaimService
                .Setup(service => service.GetListAsync(
                    It.IsAny<Expression<Func<UserOperationClaim, bool>>>(),
                    It.IsAny<Func<IQueryable<UserOperationClaim>, IOrderedQueryable<UserOperationClaim>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(new List<UserOperationClaim> { existingClaim });


            // Act
            var result = await _userOperationClaimService.UpdateAsync(updatedClaim);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Super Admin", result.OperationClaim.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowNotImplementedException_WhenClaimNotFound()
        {
            // Arrange
            var nonExistingClaim = new UserOperationClaim(999, 999)
            {
                IsDeleted = false,
                User = new User(999, "NonExisting", "User", "nonexisting.user@example.com", new byte[] { 7, 8, 9 }, new byte[] { 10, 11, 12 }, true),
                OperationClaim = new OperationClaim { Id = 999, Name = "NonExisting" }
            };

            _mockUserOperationClaimService
         .Setup(service => service.GetListAsync(
             It.IsAny<Expression<Func<UserOperationClaim, bool>>>(),
             It.IsAny<Func<IQueryable<UserOperationClaim>, IOrderedQueryable<UserOperationClaim>>>(),
             It.IsAny<bool>(),
             It.IsAny<bool>(),
             It.IsAny<bool>(),
             It.IsAny<CancellationToken>()
         ))
         .ReturnsAsync(new List<UserOperationClaim>());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotImplementedException>(() => _userOperationClaimService.UpdateAsync(nonExistingClaim));
            Assert.Equal("Aradığınız kategori bulunamamıştır.", exception.Message);
        }
    }
}

