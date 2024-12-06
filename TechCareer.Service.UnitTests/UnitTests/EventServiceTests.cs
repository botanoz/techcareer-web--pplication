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
            )
            {
                Id = Guid.NewGuid() // Mock ID
            };

            _mockEventRepository.Setup(repo => repo.AddAsync(It.IsAny<Event>()))
                .ReturnsAsync(newEvent);

            // Act
            var result = await _eventService.AddAsync(eventAddRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newEvent.Id, result.Id);
            Assert.Equal(newEvent.Title, result.Title);
            _mockEventRepository.Verify(repo => repo.AddAsync(It.Is<Event>(e =>
                e.Title == eventAddRequestDto.Title &&
                e.Description == eventAddRequestDto.Description
            )), Times.Once);
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
            var eventId = Guid.Parse("7ffc3c76-59b2-4c4e-b1e1-0c6de0e3629b");
            var eventToDelete = new Event
            {
                Id = eventId,
                Title = "Event to Delete",
                IsDeleted = false
            };

            // Mock: ID'ye göre doğru etkinliği döndürme
            _mockEventRepository.Setup(repo => repo.GetAsync(
                    It.Is<Expression<Func<Event, bool>>>(predicate => predicate.Compile()(eventToDelete)), // ID eşleştirme
                    true, false, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(eventToDelete);

            // Mock: Güncelleme metodunun çağrılması
            _mockEventRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Event>()))
                .ReturnsAsync(eventToDelete);

            var eventRequestDto = new EventRequestDto { Id = eventId };

            // Act
            var result = await _eventService.DeleteAsync(eventRequestDto);

            // Assert
            Assert.NotNull(result); // Sonucun boş olmadığını doğrula
            Assert.True(eventToDelete.IsDeleted); // Etkinliğin silindi olarak işaretlendiğini kontrol et
            _mockEventRepository.Verify(repo => repo.UpdateAsync(
                It.Is<Event>(e => e.Id == eventId && e.IsDeleted)), Times.Once); // Güncellemenin doğru parametrelerle çağrıldığını doğrula
        }




        [Fact]
        public async Task UpdateAsync_ShouldUpdateEvent_WhenEventExists()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            var existingEvent = new Event
            {
                Id = eventId,
                Title = "Old Title",
                Description = "Old Description",
                ImageUrl = "old.jpg",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                ApplicationDeadline = DateTime.Now.AddDays(2),
                ParticipationText = "Old Participation Text",
                CategoryId = 1
            };

            var updateRequest = new EventUpdateRequestDto
            {
                Id = eventId,
                Title = "New Title",
                Description = "New Description",
                ImageUrl = "new.jpg",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(3),
                ApplicationDeadline = DateTime.Now.AddDays(4),
                ParticipationText = "New Participation Text",
                CategoryId = 2
            };

            _mockEventRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Event, bool>>>(), true, false, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvent);

            _mockEventRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Event>()))
                .ReturnsAsync((Event e) => e); // Simulate returning the updated event

            // Act
            var result = await _eventService.UpdateAsync(updateRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateRequest.Title, result.Title);
            Assert.Equal(updateRequest.Description, result.Description);
            _mockEventRepository.Verify(repo => repo.UpdateAsync(It.Is<Event>(e =>
                e.Title == updateRequest.Title &&
                e.Description == updateRequest.Description
            )), Times.Once);
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
