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
            // Mocking dependencies
            _mockEventRepository = new Mock<IEventRepository>();
            _mockLogger = new Mock<LoggerServiceBase>();

            // Creating the EventService instance with mocked dependencies
            _eventService = new EventService(_mockEventRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEvent()
        {
            // Arrange
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
                Id = Guid.NewGuid(), // Updated to Guid
                Title = eventAddRequestDto.Title,
                Description = eventAddRequestDto.Description,
                ImageUrl = eventAddRequestDto.ImageUrl,
                StartDate = eventAddRequestDto.StartDate,
                EndDate = eventAddRequestDto.EndDate,
                ApplicationDeadline = eventAddRequestDto.ApplicationDeadline,
                ParticipationText = eventAddRequestDto.ParticipationText,
                CategoryId = eventAddRequestDto.CategoryId
            };

            // Setup mock repository to return the added event
            _mockEventRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Event>()))
                .ReturnsAsync(eventEntity);

            // Act
            var result = await _eventService.AddAsync(eventAddRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Event", result.Title);
            _mockLogger.Verify(logger => logger.Info(It.IsAny<string>()), Times.Once); // Verifying logger call
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteEvent()
        {
            // Arrange
            var eventRequestDto = new EventRequestDto { Id = Guid.NewGuid() }; // Updated to Guid
            var eventEntity = new Event { Id = eventRequestDto.Id, Title = "Test Event", IsDeleted = false };

            _mockEventRepository
          .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Event, bool>>>(), true, false, true, It.IsAny<CancellationToken>()))
          .ReturnsAsync(eventEntity);


            _mockEventRepository
          .Setup(repo => repo.DeleteAsync(It.IsAny<Event>(), It.IsAny<bool>()))
          .ReturnsAsync(It.IsAny<Event>());


            // Act
            var result = await _eventService.DeleteAsync(eventRequestDto);

            // Assert
            Assert.Equal(eventRequestDto.Id, result.Id);
            _mockEventRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Event>(), false), Times.Once); // Verifying delete
            _mockLogger.Verify(logger => logger.Info(It.IsAny<string>()), Times.Once); // Verifying logger call
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEvent()
        {
            // Arrange
            var eventUpdateRequestDto = new EventUpdateRequestDto
            {
                Id = Guid.NewGuid(), // Updated to Guid
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
                Id = eventUpdateRequestDto.Id, // Updated to Guid
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
            

            // Act
            var result = await _eventService.UpdateAsync(eventUpdateRequestDto);

            // Assert
            Assert.Equal("Updated Event", result.Title);
            Assert.Equal("Updated Description", result.Description);
            _mockEventRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Once); // Verifying update
            _mockLogger.Verify(logger => logger.Info(It.IsAny<string>()), Times.Once); // Verifying logger call
        }

        [Fact]
        public async Task FindEventAsync_ShouldReturnEvent()
        {
            // Arrange
            var eventRequestDto = new EventRequestDto { Id = Guid.NewGuid() }; // Updated to Guid
            var eventEntity = new Event
            {
                Id = eventRequestDto.Id, // Updated to Guid
                Title = "Test Event",
                Description = "Description",
                ImageUrl = "Image URL",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                ApplicationDeadline = DateTime.Now.AddDays(-1),
                ParticipationText = "Join us",
                CategoryId = 1
            };

            var mockLogger = new Mock<LoggerServiceBase>(MockBehavior.Strict); // Use MockBehavior.Strict
            mockLogger.Setup(logger => logger.Info(It.IsAny<string>())).Verifiable(); // Setup Info method for logging

            _mockEventRepository
                .Setup(repo => repo.GetAsync(It.Is<Expression<Func<Event, bool>>>(expr => expr.Compile().Invoke(eventEntity)), true, false, true, default))
                .ReturnsAsync(eventEntity);

            var eventService = new EventService(_mockEventRepository.Object, mockLogger.Object); // Inject mock logger

            // Act
            var result = await eventService.FindEventAsync(eventRequestDto);

            // Assert
            Assert.Equal("Test Event", result.Title); // Verify the event title

            // Verify that the GetAsync method was called with the correct parameters
            _mockEventRepository.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Event, bool>>>(), true, false, true, default), Times.Once);

            // Verify that the Info method was called once
            mockLogger.Verify(logger => logger.Info(It.IsAny<string>()), Times.Once);
        }

    }
}