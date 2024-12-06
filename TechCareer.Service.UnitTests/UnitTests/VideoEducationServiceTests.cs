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
using Core.CrossCuttingConcerns.Serilog;

namespace TechCareer.Tests.UnitTests
{
    public class VideoEducationServiceTests
    {
        private readonly Mock<IVideoEducationRepository> _mockVideoEducationRepository;
        private readonly Mock<LoggerServiceBase> _mockLogger;
        private readonly VideoEducationService _videoEducationService;

        public VideoEducationServiceTests()
        {
         
            _mockVideoEducationRepository = new Mock<IVideoEducationRepository>();
            _mockLogger = new Mock<LoggerServiceBase>();
            _videoEducationService = new VideoEducationService(_mockVideoEducationRepository.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task AddAsync_ShouldReturnAddedVideoEducation()
        {
          
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

        
            var result = await _videoEducationService.AddAsync(newVideoEducationDto); 

         
            Assert.NotNull(result);
            Assert.Equal("C# Tutorial", result.Title);
        }
        [Fact]
        public async Task DeleteAsync_ShouldMarkAsDeleted_WhenNotPermanent()
        {
         
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
                IsDeleted = false 
            };

       
            _mockVideoEducationRepository
                .Setup(repository => repository.GetAsync(It.IsAny<Expression<Func<VideoEducation, bool>>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(videoEducation);

         
            _mockVideoEducationRepository
                .Setup(repository => repository.DeleteAsync(It.IsAny<VideoEducation>(), false)) 
                .ReturnsAsync(new VideoEducation { Id = 1, IsDeleted = true });

       
            var result = await _videoEducationService.DeleteAsync(new VideoEducationRequestDto { Id = 1 });
            Assert.NotNull(result); 
            Assert.Equal(1, result.Id);
            Assert.Equal("JavaScript Tutorial", result.Title); 
                                                            
                                                            
            _mockVideoEducationRepository.Verify(repo => repo.DeleteAsync(It.Is<VideoEducation>(ve => ve.IsDeleted == true), false), Times.Once);
        }




        [Fact]
        public async Task GetAsync_ShouldReturnVideoEducation()
        {
          
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

           
            var result = await _videoEducationService.GetAsync(x => x.Id == 1);

           
            Assert.NotNull(result);
            Assert.Equal("Python Tutorial", result.Title);
        }

        [Fact]
        public async Task GetPaginateAsync_ShouldReturnPaginatedResults()
        {
         
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

         
            var result = await _videoEducationService.GetPaginateAsync(index: 0, size: 2);

      
            Assert.NotNull(result);
            Assert.Equal(3, result.TotalItems);
            Assert.Equal(2, result.Items.Count);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowApplicationException_WhenVideoEducationNotFound()
        {
         
            var videoEducationUpdateDto = new VideoEducationUpdateRequestDto
            {
                Id = 1,
                Title = "C# Tutorial"
               
            };

         
            _mockVideoEducationRepository.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<VideoEducation, bool>>>(),
                true,  
                false, 
                true, 
                It.IsAny<CancellationToken>() 
            ))
            .ReturnsAsync((VideoEducation?)null);  

    
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _videoEducationService.UpdateAsync(videoEducationUpdateDto));

        
            Assert.Equal("Video Education not found.", exception.Message);
        }



    }
}
