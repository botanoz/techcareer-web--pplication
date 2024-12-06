using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Rules;
using AutoMapper;
using TechCareer.Service.Concretes;
using Xunit;
using FluentAssertions;
using Core.Security.Entities;
using Core.Security.JWT;
using TechCareer.Models.Dtos.Users;
using Core.Persistence.Extensions;
using Core.Security.Dtos;
using System.Linq.Expressions;

namespace TechCareer.Service.Tests.UnitTests
{

    public class AuthServiceTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IUserWithTokenService> _tokenServiceMock;
        private readonly Mock<IUserBusinessRules> _rulesMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _tokenServiceMock = new Mock<IUserWithTokenService>();
            _mapperMock = new Mock<IMapper>();
            _rulesMock = new Mock<IUserBusinessRules>();  

            _authService = new AuthService(
                _rulesMock.Object,        
                _tokenServiceMock.Object,
                _userServiceMock.Object,
                _mapperMock.Object
                    );
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnAccessToken_WhenUserExistsAndPasswordMatches()
        {
        
            var dto = new UserForLoginDto { Email = "test@example.com", Password = "password123" };

          
            var user = new User
            {
                Id = 1, 
                Email = dto.Email,
                PasswordHash = new byte[0],
                PasswordSalt = new byte[0]
            };

            var accessToken = new AccessToken { Token = "test-token", Expiration = DateTime.UtcNow.AddHours(1) };

           
            _userServiceMock.Setup(u => u.GetAsync(
            It.Is<Expression<Func<User, bool>>>(e => e.Compile()(new User { Email = dto.Email })),
           false,  
           false,  
           true,  
            It.IsAny<CancellationToken>() 
            )).ReturnsAsync(user);


            _rulesMock.Setup(r => r.UserShouldBeExistsWhenSelected(It.IsAny<User>())).Returns(Task.CompletedTask);
            _rulesMock.Setup(r => r.UserPasswordShouldBeMatched(user, dto.Password)).Returns(Task.CompletedTask);
            _tokenServiceMock.Setup(t => t.CreateAccessToken(user)).ReturnsAsync(accessToken);

            // Act
            var result = await _authService.LoginAsync(dto, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(accessToken);
            _userServiceMock.Verify(u => u.GetAsync(
             It.IsAny<Expression<Func<User, bool>>>(), 
             It.IsAny<bool>(),                        
             It.IsAny<bool>(),                     
             It.IsAny<bool>(),                       
             It.IsAny<CancellationToken>()         
          ), Times.Once);

            _tokenServiceMock.Verify(t => t.CreateAccessToken(user), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnAccessToken_WhenUserIsCreated()
        {
           
            var dto = new UserForRegisterDto
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Password = "password123"
            };

        
            var user = new User { Id = 1, Email = dto.Email, FirstName = dto.FirstName, LastName = dto.LastName };
            var accessToken = new AccessToken { Token = "test-token", Expiration = DateTime.UtcNow.AddHours(1) };

            _userServiceMock.Setup(u => u.AddAsync(It.IsAny<User>())).ReturnsAsync(user);
            _tokenServiceMock.Setup(t => t.CreateAccessToken(user)).ReturnsAsync(accessToken);

           
            var result = await _authService.RegisterAsync(dto, CancellationToken.None);

            result.Should().BeEquivalentTo(accessToken);
            _userServiceMock.Verify(u => u.AddAsync(It.IsAny<User>()), Times.Once);
            _tokenServiceMock.Verify(t => t.CreateAccessToken(user), Times.Once);
        }

        [Fact]
        public async Task GetAllPaginateAsync_ShouldReturnPaginatedUserResponses()
        {
           
            int page = 1, size = 10;
            var users = new Paginate<User>
            {
                Items = new List<User> { new User { Id = 1, Email = "test@example.com" } },
                TotalItems = 1,
                Size = size,
                Index = page
            };

            var userResponses = new Paginate<UserResponseDto>
            {
                Items = new List<UserResponseDto> { new UserResponseDto { Email = "test@example.com" } },
                TotalItems = 1,
                Size = size,
                Index = page
            };

            _userServiceMock.Setup(u => u.GetPaginateAsync(
                It.IsAny<Expression<Func<User, bool>>>(),  
                 It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(), 
              false,                                
               page,                                    
               size,                                     
             false,                                     
                true,                                     
             It.IsAny<CancellationToken>()            
            )).ReturnsAsync(users);

            _mapperMock.Setup(m => m.Map<Paginate<UserResponseDto>>(users)).Returns(userResponses);

          
            var result = await _authService.GetAllPaginateAsync(page, size);

         
            result.Should().BeEquivalentTo(userResponses);
            _userServiceMock.Verify(u => u.GetPaginateAsync(
               It.IsAny<Expression<Func<User, bool>>>(), 
               It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(), 
               false,                                  
               page,                                  
               size,                                  
               false,                                 
               true,                                   
               It.IsAny<CancellationToken>()            
              ), Times.Once);

            _mapperMock.Verify(m => m.Map<Paginate<UserResponseDto>>(users), Times.Once);
        }
    }
}