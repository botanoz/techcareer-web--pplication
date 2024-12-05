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
using Microsoft.EntityFrameworkCore;
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

            // DTO'yu kullanarak Event entity oluşturulacak
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

            // Mock: AddAsync metodunun çağrılacağı ve eventEntity döndüreceği ayarlandı
            _mockEventRepository.Setup(repo => repo.AddAsync(It.IsAny<Event>())).ReturnsAsync(newEvent);

            // Act
            var result = await _eventService.AddAsync(eventAddRequestDto);  // EventAddRequestDto kullanılarak metot çağrılıyor

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

            _mockEventRepository.Setup(repo => repo.Query().FirstOrDefaultAsync(It.IsAny<Expression<Func<Event, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvent);

            // Act
            var result = await _eventService.GetAsync(e => e.Id == eventId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Existing Event", result?.Title);
        }

        [Fact]
        public async Task DeleteAsync_ShouldMarkEventAsDeleted()
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

            _mockEventRepository.Setup(repo => repo.Query().FirstOrDefaultAsync(It.IsAny<Expression<Func<Event, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(eventToDelete);

            // Act
            var result = await _eventService.DeleteAsync(eventRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(eventToDelete.IsDeleted);  // eventToDelete nesnesindeki IsDeleted özelliğini kontrol ediyoruz
        }



        [Fact]
        public async Task UpdateAsync_ShouldUpdateEvent()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            // Var olan Event nesnesi
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

            // Güncellenmiş Event nesnesi
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

            // Mock: GetListAsync metodu, mevcut eventi döndürecek şekilde ayarlandı
            _mockEventRepository.Setup(repo => repo.GetListAsync(
                It.IsAny<Expression<Func<Event, bool>>>(), // Predicate
                It.IsAny<Func<IQueryable<Event>, IOrderedQueryable<Event>>>(), // OrderBy
                It.IsAny<bool>(), // Include
                It.IsAny<bool>(), // withDeleted
                It.IsAny<bool>(), // enableTracking
                It.IsAny<CancellationToken>() // CancellationToken
            )).ReturnsAsync(new List<Event> { existingEvent });

            // Mock: UpdateAsync metodu
            _mockEventRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Event>()))
                .ReturnsAsync(existingEvent);

            // Act
            var result = await _eventService.UpdateAsync(updatedEventDto);  // DTO kullanılarak metot çağrılıyor

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Event", result.Title);
            _mockEventRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Once); // UpdateAsync'ın çağrıldığını doğruluyoruz
            _mockEventRepository.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Never); // AddAsync'ın çağrılmadığını doğruluyoruz
        }


    }
}
