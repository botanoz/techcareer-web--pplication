using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Core.Security.Entities;
using TechCareer.Service.Concretes;
using TechCareer.Service.Abstracts;
using Xunit;
using Core.Persistence.Extensions;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Dtos.VideoEducation;

namespace TechCareer.Tests.UnitTests
{
    public class VideoEducationServiceTests
    {
        private readonly Mock<IVideoEducationRepository> _mockVideoEducationRepository;
        private readonly VideoEducationService _videoEducationService;

        public VideoEducationServiceTests()
        {
            _mockVideoEducationRepository = new Mock<IVideoEducationRepository>();
            _videoEducationService = new VideoEducationService(_mockVideoEducationRepository.Object);
        }
        [Fact]
        public async Task AddAsync_ShouldReturnAddedVideoEducation()
        {
            // Arrange
            var newVideoEducationDto = new VideoEducationAddRequestDto
            {
                Title = "C# Tutorial",
                Description = "Learn C# from basics to advanced",
                TotalHour = 10,
                IsCertified = true,
                Level = 1,
                ImageUrl = "image_url",
                InstructorId = Guid.NewGuid(),
                ProgrammingLanguage = "C#"
            };

            _mockVideoEducationRepository
                .Setup(repository => repository.AddAsync(It.IsAny<VideoEducation>()))
                .ReturnsAsync(new VideoEducation
                {
                    Id = 1,
                    Title = "C# Tutorial",
                    Description = "Learn C# from basics to advanced",
                    TotalHour = 10,
                    IsCertified = true,
                    Level = 1,
                    ImageUrl = "image_url",
                    InstructorId = Guid.NewGuid(),
                    ProgrammingLanguage = "C#"
                });

            // Act
            var result = await _videoEducationService.AddAsync(newVideoEducationDto); // Pass DTO here

            // Assert
            Assert.NotNull(result);
            Assert.Equal("C# Tutorial", result.Title);
        }
        [Fact]
        public async Task DeleteAsync_ShouldMarkAsDeleted_WhenNotPermanent()
        {
            // Arrange
            var videoEducation = new VideoEducation
            {
                Id = 1,
                Title = "JavaScript Tutorial",
                Description = "Learn JavaScript",
                TotalHour = 8,
                IsCertified = false,
                Level = 2,
                ImageUrl = "image_url",
                InstructorId = Guid.NewGuid(),
                ProgrammingLanguage = "JavaScript",
                IsDeleted = false // Initially not deleted
            };

            // Mock GetAsync to return the videoEducation entity
            _mockVideoEducationRepository
                .Setup(repository => repository.GetAsync(It.IsAny<Expression<Func<VideoEducation, bool>>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(videoEducation);

            // Mock DeleteAsync to simulate marking the entity as deleted (without permanent delete)
            _mockVideoEducationRepository
                .Setup(repository => repository.DeleteAsync(It.IsAny<VideoEducation>(), false)) // Passing 'false' for permanent
                .ReturnsAsync(new VideoEducation { Id = 1, IsDeleted = true });

            // Act
            var result = await _videoEducationService.DeleteAsync(new VideoEducationRequestDto { Id = 1 });
            Assert.NotNull(result); 
            Assert.Equal(1, result.Id);
            Assert.Equal("JavaScript Tutorial", result.Title); 
                                                            
                                                            
            _mockVideoEducationRepository.Verify(repo => repo.DeleteAsync(It.Is<VideoEducation>(ve => ve.IsDeleted == true), false), Times.Once);
        }




        [Fact]
        public async Task GetAsync_ShouldReturnVideoEducation()
        {
            // Arrange
            var videoEducation = new VideoEducation
            {
                Id = 1,
                Title = "Python Tutorial",
                Description = "Learn Python programming",
                TotalHour = 12,
                IsCertified = true,
                Level = 1,
                ImageUrl = "image_url",
                InstructorId = Guid.NewGuid(),
                ProgrammingLanguage = "Python"
            };

            _mockVideoEducationRepository
                .Setup(repository => repository.GetAsync(It.IsAny<Expression<Func<VideoEducation, bool>>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(videoEducation);

            // Act
            var result = await _videoEducationService.GetAsync(x => x.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Python Tutorial", result.Title);
        }

        [Fact]
        public async Task GetPaginateAsync_ShouldReturnPaginatedResults()
        {
            // Arrange
            var videoEducations = new List<VideoEducation>
            {
                new VideoEducation { Id = 1, Title = "C# Tutorial" },
                new VideoEducation { Id = 2, Title = "JavaScript Tutorial" },
                new VideoEducation { Id = 3, Title = "Python Tutorial" }
            };

            _mockVideoEducationRepository
                .Setup(repository => repository.GetPaginateAsync(It.IsAny<Expression<Func<VideoEducation, bool>>>(), It.IsAny<Func<IQueryable<VideoEducation>, IOrderedQueryable<VideoEducation>>>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Paginate<VideoEducation>
                {
                    Items = videoEducations.Take(2).ToList(),
                    TotalItems = videoEducations.Count,
                    TotalPages = 2,
                    Index = 0,
                    Size = 2
                });

            // Act
            var result = await _videoEducationService.GetPaginateAsync(index: 0, size: 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TotalItems);
            Assert.Equal(2, result.Items.Count);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowApplicationException_WhenVideoEducationNotFound()
        {
            // Arrange
            var videoEducationUpdateDto = new VideoEducationUpdateRequestDto
            {
                Id = 1,
                Title = "C# Tutorial"
                // Diğer özellikleri burada ekleyebilirsiniz.
            };

            // _videoEducationRepository.GetAsync metodunu, null dönecek şekilde ayarlıyoruz.
            _mockVideoEducationRepository.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<VideoEducation, bool>>>(),
                true,  // include parametresi
                false, // withDeleted parametresi
                true,  // enableTracking parametresi
                It.IsAny<CancellationToken>() // cancellationToken parametresi
            ))
            .ReturnsAsync((VideoEducation?)null);  // Burada null döndürülmesi sağlanıyor.

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _videoEducationService.UpdateAsync(videoEducationUpdateDto));

            // Assert: Hata mesajını kontrol ediyoruz.
            Assert.Equal("Video Education not found.", exception.Message);
        }



    }
}
