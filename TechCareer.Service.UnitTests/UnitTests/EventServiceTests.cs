using Moq;
using TechCareer.Service.Concretes;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.CrossCuttingConcerns.Serilog;
using TechCareer.Models.Dtos.Event;
using System;
using System.Threading.Tasks;
using Xunit;
using Core.Persistence.Repositories;
using Core.Security.Entities;
using System.Linq.Expressions;

namespace TechCareer.Service.UnitTests
{
    public class EventServiceTests
    {
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly Mock<LoggerServiceBase> _mockLogger;
        private readonly EventService _eventService;

        public EventServiceTests()
        {
          
            _mockEventRepository = new Mock<IEventRepository>();
            _mockLogger = new Mock<LoggerServiceBase>();

           
            _eventService = new EventService(_mockEventRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEvent()
        {
          
            var eventAddRequestDto = new EventAddRequestDto
            {
                Title = "Test Event",
                Description = "Description",
                ImageUrl = "Image URL",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                ApplicationDeadline = DateTime.Now.AddDays(-1),
                ParticipationText = "Join us",
                CategoryId = 1
            };

            var eventEntity = new Event
            {
                Id = Guid.NewGuid(), 
                Title = eventAddRequestDto.Title,
                Description = eventAddRequestDto.Description,
                ImageUrl = eventAddRequestDto.ImageUrl,
                StartDate = eventAddRequestDto.StartDate,
                EndDate = eventAddRequestDto.EndDate,
                ApplicationDeadline = eventAddRequestDto.ApplicationDeadline,
                ParticipationText = eventAddRequestDto.ParticipationText,
                CategoryId = eventAddRequestDto.CategoryId
            };

            _mockEventRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Event>()))
                .ReturnsAsync(eventEntity);

           
            var result = await _eventService.AddAsync(eventAddRequestDto);

       
            Assert.NotNull(result);
            Assert.Equal("Test Event", result.Title);
            _mockLogger.Verify(logger => logger.Info(It.IsAny<string>()), Times.Once); 
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteEvent()
        {
           
            var eventRequestDto = new EventRequestDto { Id = Guid.NewGuid() }; 
            var eventEntity = new Event { Id = eventRequestDto.Id, Title = "Test Event", IsDeleted = false };

            _mockEventRepository
          .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Event, bool>>>(), true, false, true, It.IsAny<CancellationToken>()))
          .ReturnsAsync(eventEntity);


            _mockEventRepository
          .Setup(repo => repo.DeleteAsync(It.IsAny<Event>(), It.IsAny<bool>()))
          .ReturnsAsync(It.IsAny<Event>());


            var result = await _eventService.DeleteAsync(eventRequestDto);

         
            Assert.Equal(eventRequestDto.Id, result.Id);
            _mockEventRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Event>(), false), Times.Once); 
            _mockLogger.Verify(logger => logger.Info(It.IsAny<string>()), Times.Once); 
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEvent()
        {
            
            var eventUpdateRequestDto = new EventUpdateRequestDto
            {
                Id = Guid.NewGuid(), 
                Title = "Updated Event",
                Description = "Updated Description",
                ImageUrl = "Updated Image URL",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2),
                ApplicationDeadline = DateTime.Now,
                ParticipationText = "Updated Participation Text",
                CategoryId = 2
            };

            var existingEvent = new Event
            {
                Id = eventUpdateRequestDto.Id,
                Title = "Old Title",
                Description = "Old Description",
                ImageUrl = "Old Image URL",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                ApplicationDeadline = DateTime.Now.AddDays(-1),
                ParticipationText = "Old Participation Text",
                CategoryId = 1
            };
            _mockEventRepository
                .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Event, bool>>>(), true, false, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvent);

            _mockEventRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<Event>()))
                .Returns(Task.FromResult(It.IsAny<Event>()));
            

         
            var result = await _eventService.UpdateAsync(eventUpdateRequestDto);

        
            Assert.Equal("Updated Event", result.Title);
            Assert.Equal("Updated Description", result.Description);
            _mockEventRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Once); 
            _mockLogger.Verify(logger => logger.Info(It.IsAny<string>()), Times.Once); 
        }

        [Fact]
        public async Task FindEventAsync_ShouldReturnEvent()
        {
          
            var eventRequestDto = new EventRequestDto { Id = Guid.NewGuid() };
            var eventEntity = new Event
            {
                Id = eventRequestDto.Id,
                Title = "Test Event",
                Description = "Description",
                ImageUrl = "Image URL",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                ApplicationDeadline = DateTime.Now.AddDays(-1),
                ParticipationText = "Join us",
                CategoryId = 1
            };

            var mockLogger = new Mock<LoggerServiceBase>(MockBehavior.Strict); 
            mockLogger.Setup(logger => logger.Info(It.IsAny<string>())).Verifiable(); 

            _mockEventRepository
                .Setup(repo => repo.GetAsync(It.Is<Expression<Func<Event, bool>>>(expr => expr.Compile().Invoke(eventEntity)), true, false, true, default))
                .ReturnsAsync(eventEntity);

            var eventService = new EventService(_mockEventRepository.Object, mockLogger.Object); 

         
            var result = await eventService.FindEventAsync(eventRequestDto);

          
            Assert.Equal("Test Event", result.Title);

           
            _mockEventRepository.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Event, bool>>>(), true, false, true, default), Times.Once);

           
            mockLogger.Verify(logger => logger.Info(It.IsAny<string>()), Times.Once);
        }

    }
}