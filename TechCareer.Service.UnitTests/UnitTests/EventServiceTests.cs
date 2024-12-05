using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TechCareer.Service.Concretes;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;
using Xunit;
using TechCareer.Models.Dtos.Event;

namespace TechCareer.Service.Tests.UnitTests
{
    public class EventServiceTests
    {
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly EventService _eventService;

        public EventServiceTests()
        {
            _mockEventRepository = new Mock<IEventRepository>();
            _eventService = new EventService(_mockEventRepository.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEvent()
        {
            // Arrange
            var eventAddRequestDto = new EventAddRequestDto
            {
                Title = "Test Event",
                Description = "Test Description",
                ImageUrl = "test.jpg",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                ApplicationDeadline = DateTime.Now.AddDays(2),
                ParticipationText = "Join us!",
                CategoryId = 1
            };

            var newEvent = new Event(
                eventAddRequestDto.Title,
                eventAddRequestDto.Description,
                eventAddRequestDto.ImageUrl,
                eventAddRequestDto.StartDate,
                eventAddRequestDto.EndDate,
                eventAddRequestDto.ApplicationDeadline,
                eventAddRequestDto.ParticipationText,
                eventAddRequestDto.CategoryId
            );

            _mockEventRepository.Setup(repo => repo.AddAsync(It.IsAny<Event>())).ReturnsAsync(newEvent);

            // Act
            var result = await _eventService.AddAsync(eventAddRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Event", result.Title);
            _mockEventRepository.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnEvent_WhenEventExists()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var existingEvent = new Event
            {
                Id = eventId,
                Title = "Existing Event",
                Description = "Description",
                ImageUrl = "image.jpg",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                ApplicationDeadline = DateTime.Now.AddDays(2),
                ParticipationText = "Join us!",
                CategoryId = 1
            };

            // Update the mock to avoid optional parameters
            _mockEventRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Event, bool>>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvent);

            // Act
            var result = await _eventService.GetAsync(e => e.Id == eventId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Existing Event", result?.Title);
        }


        [Fact]
        public async Task DeleteAsync_ShouldMarkEventAsDeleted_WhenEventExists()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventToDelete = new Event
            {
                Id = eventId,
                Title = "Event to Delete",
                Description = "Description",
                ImageUrl = "image.jpg",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                ApplicationDeadline = DateTime.Now.AddDays(2),
                ParticipationText = "Join us!",
                CategoryId = 1
            };

            var eventRequestDto = new EventRequestDto
            {
                Id = eventId
            };

            // Simplified mock setup to bypass optional parameters
            _mockEventRepository.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Event, bool>>>(), // Expression predicate
                true, // include (default value)
                false, // withDeleted (default value)
                true, // enableTracking (default value)
                It.IsAny<CancellationToken>() // cancellation token
            ))
            .ReturnsAsync(eventToDelete);

            // Act
            var result = await _eventService.DeleteAsync(eventRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(eventToDelete.IsDeleted);
            _mockEventRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Once);
        }



        [Fact]
        public async Task UpdateAsync_ShouldUpdateEvent_WhenEventExists()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            var existingEvent = new Event
            {
                Id = eventId,
                Title = "Event to Update",
                Description = "Description",
                ImageUrl = "image.jpg",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                ApplicationDeadline = DateTime.Now.AddDays(2),
                ParticipationText = "Join us!",
                CategoryId = 1
            };

            var updatedEventDto = new EventUpdateRequestDto
            {
                Id = eventId,
                Title = "Updated Event",
                Description = "Updated Description",
                ImageUrl = "updated_image.jpg",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                ApplicationDeadline = DateTime.Now.AddDays(3),
                ParticipationText = "Updated Participation Text",
                CategoryId = 2
            };

            // Mock GetAsync to return the existing event with explicit values for optional parameters
            _mockEventRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Event, bool>>>(), true, false, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvent);

            // Mock UpdateAsync to return the updated event
            _mockEventRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Event>()))
                .ReturnsAsync((Event updatedEvent) =>
                {
                    // Simulate the event being updated with the new values
                    existingEvent.Title = updatedEvent.Title;
                    existingEvent.Description = updatedEvent.Description;
                    existingEvent.ImageUrl = updatedEvent.ImageUrl;
                    existingEvent.StartDate = updatedEvent.StartDate;
                    existingEvent.EndDate = updatedEvent.EndDate;
                    existingEvent.ApplicationDeadline = updatedEvent.ApplicationDeadline;
                    existingEvent.ParticipationText = updatedEvent.ParticipationText;
                    existingEvent.CategoryId = updatedEvent.CategoryId;

                    return existingEvent;
                });

            // Act
            var result = await _eventService.UpdateAsync(updatedEventDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Event", result.Title);
            Assert.Equal("Updated Description", result.Description);
            Assert.Equal("updated_image.jpg", result.ImageUrl);
            Assert.Equal(DateTime.Now.AddDays(2), result.EndDate);
            Assert.Equal("Updated Participation Text", result.ParticipationText);
            _mockEventRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Once);
            _mockEventRepository.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Never);
        }


        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenEventNotFound()
        {
            // Arrange
            var updatedEventDto = new EventUpdateRequestDto
            {
                Id = Guid.NewGuid(),
                Title = "Updated Event",
                Description = "Updated Description",
                ImageUrl = "updated_image.jpg",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                ApplicationDeadline = DateTime.Now.AddDays(3),
                ParticipationText = "Updated Participation Text",
                CategoryId = 2
            };

            // Mock GetAsync to return null, indicating event not found
            _mockEventRepository.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Event, bool>>>(),  // Expression predicate
                true,   // include (default value)
                false,  // withDeleted (default value)
                true,   // enableTracking (default value)
                It.IsAny<CancellationToken>() // cancellation token
            ))
            .ReturnsAsync((Event)null);  // Simulate event not found (null)

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _eventService.UpdateAsync(updatedEventDto));
            Assert.Equal("Event not found.", exception.Message);
        }

    }
}
