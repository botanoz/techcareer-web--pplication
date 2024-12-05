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
using Microsoft.EntityFrameworkCore;
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
            var result = await _operationClaimService.AddAsync(operationClaim);

            // Assert
            result.Should().BeEquivalentTo(operationClaim);
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

            // Setup GetListAsync to return the operation claims list, matching the method signature with default parameters
            _mockOperationClaimRepository.Setup(repo => repo.GetListAsync(
                    It.IsAny<Expression<Func<OperationClaim, bool>>>(),  // Allow any predicate (filter)
                    null,                                                 // No ordering (orderBy is null)
                    true,                                                 // Include related data (default is true)
                    false,                                                // withDeleted is false by default
                    true,                                                 // Enable tracking (default is true)
                    It.IsAny<CancellationToken>()                         // CancellationToken (default)
                ))
                .ReturnsAsync(operationClaims);

            // Act
            var result = await _operationClaimService.GetListAsync();

            // Assert
            result.Should().BeEquivalentTo(operationClaims);  // Assert that the result matches the expected list
            _mockOperationClaimRepository.Verify(repo => repo.GetListAsync(
                It.IsAny<Expression<Func<OperationClaim, bool>>>(),
                null,
                true,
                false,
                true,
                It.IsAny<CancellationToken>()), Times.Once);  // Ensure GetListAsync was called once with the expected parameters
        }

        [Fact]
        public async Task DeleteAsync_ShouldMarkOperationClaimAsDeleted()
        {
            // Arrange
            var operationClaim = new OperationClaim { Id = 1, Name = "Admin", IsDeleted = false };

            // Setup for GetAsync: Returns the operation claim based on the predicate (with tracking)
            _mockOperationClaimRepository.Setup(repo => repo.GetAsync(
                    It.IsAny<Expression<Func<OperationClaim, bool>>>(),  // Any predicate
                    true,                                                 // Include (true by default)
                    false,                                                // withDeleted (false by default)
                    true,                                                 // EnableTracking (true by default)
                    It.IsAny<CancellationToken>()                         // CancellationToken (default)
                ))
                .ReturnsAsync(operationClaim);

            var deleteRequestDto = new OperationClaimDeleteRequestDto { Id = 1, Permanent = false };

            // Setup for UpdateAsync: Should update the IsDeleted flag when called
            _mockOperationClaimRepository.Setup(repo => repo.UpdateAsync(It.IsAny<OperationClaim>()))
                                          .ReturnsAsync(operationClaim);  // Return the updated claim

            // Act
            var result = await _operationClaimService.DeleteAsync(deleteRequestDto);

            // Assert
            result.IsDeleted.Should().BeTrue();  // Assert that IsDeleted is marked as true
            _mockOperationClaimRepository.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<OperationClaim, bool>>>(),
                                                                         true, false, true, It.IsAny<CancellationToken>()), Times.Once);  // Verify GetAsync was called once
            _mockOperationClaimRepository.Verify(repo => repo.UpdateAsync(It.IsAny<OperationClaim>()), Times.Once);  // Verify UpdateAsync was called once to update IsDeleted
        }

        [Fact]
        public async Task GetAsync_ShouldReturnOperationClaimById()
        {
            // Arrange
            var operationClaim = new OperationClaim { Id = 1, Name = "Admin" };

            // Explicitly pass the optional arguments
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
            result.Should().BeEquivalentTo(operationClaim);
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

            // Set up GetAsync to return the existing claim
            _mockOperationClaimRepository.Setup(repo => repo.GetAsync(
                    It.IsAny<Expression<Func<OperationClaim, bool>>>(),  
                    true,                                               
                    false,                                               
                    true,                                               
                    It.IsAny<CancellationToken>()                         
                ))
                .ReturnsAsync(existingClaim);

            // Set up UpdateAsync to return the updated claim
            _mockOperationClaimRepository.Setup(repo => repo.UpdateAsync(It.IsAny<OperationClaim>()))
                                          .ReturnsAsync(existingClaim);

            // Act
            var result = await _operationClaimService.UpdateAsync(updateRequestDto);

            // Assert
            result.Name.Should().Be(updateRequestDto.Name);  // Ensure the name is updated
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

            // Set up GetListAsync to return a list of operation claims
            _mockOperationClaimRepository.Setup(repo => repo.GetListAsync(
                    It.IsAny<Expression<Func<OperationClaim, bool>>>(),  // predicate (allow any filter)
                    null,                                                 // orderBy (no ordering)
                    true,                                                 // include (default: true)
                    false,                                                // withDeleted (default: false)
                    true,                                              
                    It.IsAny<CancellationToken>()                       
                ))
                .ReturnsAsync(operationClaims);

            // Act
            var result = await _operationClaimService.GetPaginateAsync(index: 0, size: 2);

            // Assert
            result.Items.Should().BeEquivalentTo(operationClaims);  
            result.TotalItems.Should().Be(2);  
            result.TotalPages.Should().Be(1);  
        }

    }
}
