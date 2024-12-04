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
namespace TechCareer.Service.Tests.UnitTests
{ 
public class OperationClaimServiceTests
{
    private readonly Mock<IOperationClaimService> _mockOperationClaimService;
    private readonly OperationClaimService _operationClaimService;

    public OperationClaimServiceTests()
    {
        _mockOperationClaimService = new Mock<IOperationClaimService>();
        _operationClaimService = new OperationClaimService(_mockOperationClaimService.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnAddedOperationClaim()
    {
        // Arrange
        var operationClaim = new OperationClaim { Id = 1, Name = "Admin" };
        _mockOperationClaimService.Setup(service => service.AddAsync(It.IsAny<OperationClaim>()))
                                  .ReturnsAsync(operationClaim);

        // Act
        var result = await _operationClaimService.AddAsync(operationClaim);

        // Assert
        result.Should().BeEquivalentTo(operationClaim);
        _mockOperationClaimService.Verify(service => service.AddAsync(It.IsAny<OperationClaim>()), Times.Once);
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
        _mockOperationClaimService.Setup(service => service.GetListAsync(It.IsAny<Expression<Func<OperationClaim, bool>>>(), null, false, false, true, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationClaims);

        // Act
        var result = await _operationClaimService.GetListAsync();

        // Assert
        result.Should().BeEquivalentTo(operationClaims);
        _mockOperationClaimService.Verify(service => service.GetListAsync(It.IsAny<Expression<Func<OperationClaim, bool>>>(), null, false, false, true, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldMarkOperationClaimAsDeleted()
    {
        // Arrange
        var operationClaim = new OperationClaim { Id = 1, Name = "Admin", IsDeleted = false };
        _mockOperationClaimService.Setup(service => service.GetListAsync(It.IsAny<Expression<Func<OperationClaim, bool>>>(), null, false, false, true, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(new List<OperationClaim> { operationClaim });

        // Act
        var result = await _operationClaimService.DeleteAsync(operationClaim);

        // Assert
        result.IsDeleted.Should().BeTrue();
        _mockOperationClaimService.Verify(service => service.GetListAsync(It.IsAny<Expression<Func<OperationClaim, bool>>>(), null, false, false, true, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnOperationClaimById()
    {
        // Arrange
        var operationClaim = new OperationClaim { Id = 1, Name = "Admin" };
        _mockOperationClaimService.Setup(service => service.GetAsync(It.IsAny<Expression<Func<OperationClaim, bool>>>(), false, false, true, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationClaim);

        // Act
        var result = await _operationClaimService.GetAsync(x => x.Id == 1);

        // Assert
        result.Should().BeEquivalentTo(operationClaim);
        _mockOperationClaimService.Verify(service => service.GetAsync(It.IsAny<Expression<Func<OperationClaim, bool>>>(), false, false, true, It.IsAny<CancellationToken>()), Times.Once);
    }
}
}