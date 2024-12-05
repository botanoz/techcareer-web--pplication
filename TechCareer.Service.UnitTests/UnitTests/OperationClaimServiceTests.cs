using System.Linq.Expressions;
using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Concretes;
using Core.Security.Entities;
using System.Collections.Generic;
using FluentAssertions;
using TechCareer.Models.Dtos.OperationClaim;
using TechCareer.DataAccess.Repositories.Abstracts;

namespace TechCareer.Service.Tests.UnitTests
{
    public class OperationClaimServiceTests
    {
        private readonly Mock<IOperationClaimRepository> _mockOperationClaimRepository;
        private readonly OperationClaimService _operationClaimService;

        public OperationClaimServiceTests()
        {
            _mockOperationClaimRepository = new Mock<IOperationClaimRepository>();
            _operationClaimService = new OperationClaimService(_mockOperationClaimRepository.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnAddedOperationClaim()
        {
            // Arrange
            var operationClaim = new OperationClaim { Id = 1, Name = "Admin" };
            _mockOperationClaimRepository.Setup(repo => repo.AddAsync(It.IsAny<OperationClaim>()))
                                         .ReturnsAsync(operationClaim);

            // Act
            var result = await _operationClaimService.AddAsync(new OperationClaimAddRequestDto { Name = "Admin" });

            // Assert
            result.Should().BeEquivalentTo(new OperationClaimResponseDto { Id = 1, Name = "Admin" });
            _mockOperationClaimRepository.Verify(repo => repo.AddAsync(It.IsAny<OperationClaim>()), Times.Once);
        }

        [Fact]
        public async Task GetListAsync_ShouldReturnOperationClaimList()
        {
            // Arrange
            var operationClaims = new List<OperationClaim>
            {
                new OperationClaim { Id = 1, Name = "Admin" },
                new OperationClaim { Id = 2, Name = "User" }
            };

            _mockOperationClaimRepository.Setup(repo => repo.GetListAsync(
                    It.IsAny<Expression<Func<OperationClaim, bool>>>(),
                    null,
                    true,
                    false,
                    true,
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(operationClaims);

            // Act
            var result = await _operationClaimService.GetListAsync();

            // Assert
            result.Should().BeEquivalentTo(operationClaims.Select(c => new OperationClaimResponseDto { Id = c.Id, Name = c.Name }).ToList());
            _mockOperationClaimRepository.Verify(repo => repo.GetListAsync(
                It.IsAny<Expression<Func<OperationClaim, bool>>>(),
                null,
                true,
                false,
                true,
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldMarkOperationClaimAsDeleted()
        {
            // Arrange
            var operationClaim = new OperationClaim { Id = 1, Name = "Admin", IsDeleted = false };

            _mockOperationClaimRepository.Setup(repo => repo.GetAsync(
                    It.IsAny<Expression<Func<OperationClaim, bool>>>(),
                    true,
                    false,
                    true,
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(operationClaim);

            var deleteRequestDto = new OperationClaimRequestDto { Id = 1 };

            _mockOperationClaimRepository.Setup(repo => repo.UpdateAsync(It.IsAny<OperationClaim>()))
                                          .ReturnsAsync(operationClaim);

            // Act
            var result = await _operationClaimService.DeleteAsync(deleteRequestDto);

            // Assert
            result.Name.Should().Be(operationClaim.Name);  // Name'ı doğrulama
            _mockOperationClaimRepository.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<OperationClaim, bool>>>(),
                                                                         true, false, true, It.IsAny<CancellationToken>()), Times.Once);
            _mockOperationClaimRepository.Verify(repo => repo.UpdateAsync(It.IsAny<OperationClaim>()), Times.Once);
        }


        [Fact]
        public async Task GetAsync_ShouldReturnOperationClaimById()
        {
            // Arrange
            var operationClaim = new OperationClaim { Id = 1, Name = "Admin" };

            _mockOperationClaimRepository.Setup(repo => repo.GetAsync(
                    It.IsAny<Expression<Func<OperationClaim, bool>>>(),
                    true,
                    false,
                    true,
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(operationClaim);

            // Act
            var result = await _operationClaimService.GetAsync(x => x.Id == 1);

            // Assert
            result.Should().BeEquivalentTo(new OperationClaimResponseDto { Id = 1, Name = "Admin" });
            _mockOperationClaimRepository.Verify(repo => repo.GetAsync(
                    It.IsAny<Expression<Func<OperationClaim, bool>>>(),
                    true,
                    false,
                    true,
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedOperationClaim()
        {
            // Arrange
            var updateRequestDto = new OperationClaimUpdateRequestDto { Id = 1, Name = "UpdatedAdmin" };
            var existingClaim = new OperationClaim { Id = 1, Name = "Admin" };

            _mockOperationClaimRepository.Setup(repo => repo.GetAsync(
                    It.IsAny<Expression<Func<OperationClaim, bool>>>(),
                    true,
                    false,
                    true,
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(existingClaim);

            _mockOperationClaimRepository.Setup(repo => repo.UpdateAsync(It.IsAny<OperationClaim>()))
                                          .ReturnsAsync(existingClaim);

            // Act
            var result = await _operationClaimService.UpdateAsync(updateRequestDto);

            // Assert
            result.Name.Should().Be("UpdatedAdmin");
            _mockOperationClaimRepository.Verify(repo => repo.UpdateAsync(It.IsAny<OperationClaim>()), Times.Once);
        }

        [Fact]
        public async Task GetPaginateAsync_ShouldReturnPaginatedOperationClaims()
        {
            // Arrange
            var operationClaims = new List<OperationClaim>
            {
                new OperationClaim { Id = 1, Name = "Admin" },
                new OperationClaim { Id = 2, Name = "User" }
            };

            _mockOperationClaimRepository.Setup(repo => repo.GetListAsync(
                    It.IsAny<Expression<Func<OperationClaim, bool>>>(),
                    null,
                    true,
                    false,
                    true,
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(operationClaims);

            // Act
            var result = await _operationClaimService.GetPaginateAsync(index: 0, size: 2);

            // Assert
            result.Items.Should().BeEquivalentTo(operationClaims.Select(c => new OperationClaimResponseDto { Id = c.Id, Name = c.Name }).ToList());
            result.TotalItems.Should().Be(2);
            result.TotalPages.Should().Be(1);
        }
    }
}
