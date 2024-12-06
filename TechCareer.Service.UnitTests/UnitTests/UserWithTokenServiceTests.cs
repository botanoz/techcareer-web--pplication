using Core.Security.Entities;
using Core.Security.JWT;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Service.Concretes;
using Xunit;

namespace TechCareer.Service.Tests.UnitTests
{
    public class UserWithTokenServiceTests
    {
        private readonly Mock<ITokenHelper> _mockTokenHelper;
        private readonly Mock<IUserOperationClaimRepository> _mockUserOperationClaimRepository;
        private readonly UserWithTokenService _userWithTokenService;

        public UserWithTokenServiceTests()
        {
            _mockTokenHelper = new Mock<ITokenHelper>();
            _mockUserOperationClaimRepository = new Mock<IUserOperationClaimRepository>();
            _userWithTokenService = new UserWithTokenService(_mockTokenHelper.Object, _mockUserOperationClaimRepository.Object);
        }

        [Fact]
        public async Task CreateAccessToken_ShouldReturnAccessToken()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@example.com"
            };

            var userOperationClaims = new List<UserOperationClaim>
    {
        new UserOperationClaim(1, 1)
        {
            User = user,
            OperationClaim = new OperationClaim { Id = 1, Name = "Admin" },
            IsDeleted = false
        }
    };

            // Mock the repository to return the userOperationClaims as an async list
            _mockUserOperationClaimRepository
                .Setup(repo => repo.Query())
                .Returns(userOperationClaims.AsQueryable().AsNoTracking()); // This line might cause issues if not async, so we handle the issue below

            // Mock the helper method
            var expectedAccessToken = new AccessToken
            {
                Token = "mockedToken",
                Expiration = DateTime.Now.AddMinutes(15)
            };

            _mockTokenHelper
                .Setup(helper => helper.CreateToken(It.IsAny<User>(), It.IsAny<List<OperationClaim>>()))
                .Returns(expectedAccessToken);

            // Act
            var result = await _userWithTokenService.CreateAccessToken(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAccessToken.Token, result.Token);
            Assert.Equal(expectedAccessToken.Expiration, result.Expiration);

            // Verify the method calls
            _mockUserOperationClaimRepository.Verify(repo => repo.Query(), Times.Once);
            _mockTokenHelper.Verify(helper => helper.CreateToken(user, It.IsAny<List<OperationClaim>>()), Times.Once);
        }

    }
}
