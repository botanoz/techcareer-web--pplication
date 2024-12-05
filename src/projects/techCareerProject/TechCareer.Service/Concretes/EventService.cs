using Core.Persistence.Extensions;
using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.DataAccess.Repositories.Concretes;
using TechCareer.Models.Dtos.Category;
using TechCareer.Models.Dtos.Event;
using TechCareer.Service.Abstracts;

namespace TechCareer.Service.Concretes
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // Etkinlik eklemek için AddAsync metodu
        public async Task<EventResponseDto> AddAsync(EventAddRequestDto eventAddRequestDto)
        {
            try
            {
                Event eventEntity = new Event(
                    eventAddRequestDto.Title,
                    eventAddRequestDto.Description,
                    eventAddRequestDto.ImageUrl,
                    eventAddRequestDto.StartDate,
                    eventAddRequestDto.EndDate,
                    eventAddRequestDto.ApplicationDeadline,
                    eventAddRequestDto.ParticipationText,
                    eventAddRequestDto.CategoryId);

                // Etkinliği veritabanına ekleyin
                var addedEvent = await _eventRepository.AddAsync(eventEntity);

                // Dönen etkinlik bilgilerini içeren DTO'yu döndürün
                return new EventResponseDto
                {
                    Id = addedEvent.Id,
                    Title = addedEvent.Title,
                    Description = addedEvent.Description,
                    ImageUrl = addedEvent.ImageUrl,
                    StartDate = addedEvent.StartDate,
                    EndDate = addedEvent.EndDate,
                    ApplicationDeadline = addedEvent.ApplicationDeadline,
                    ParticipationText = addedEvent.ParticipationText,
                    CategoryId = addedEvent.CategoryId
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException("Etkinlik eklenemedi", ex);
            }
        }

        // Etkinlik silme metodu
        public async Task<EventResponseDto> DeleteAsync(EventRequestDto eventRequestDto, bool permanent = false)
        {
            var selectedEvent = await _eventRepository.GetAsync(x => x.Id == eventRequestDto.Id, withDeleted: true);

            if (selectedEvent == null)
                throw new ApplicationException("Event not found.");

            if (permanent)
            {
                await _eventRepository.DeleteAsync(selectedEvent, true);
            }
            else
            {
                selectedEvent.IsDeleted = true;
                await _eventRepository.UpdateAsync(selectedEvent);
            }

            return new EventResponseDto
            {
                Id = selectedEvent.Id,
                Title = selectedEvent.Title,
                Description = selectedEvent.Description,
                ImageUrl = selectedEvent.ImageUrl,
                StartDate = selectedEvent.StartDate,
                EndDate = selectedEvent.EndDate,
                ApplicationDeadline = selectedEvent.ApplicationDeadline,
                ParticipationText = selectedEvent.ParticipationText,
                CategoryId = selectedEvent.CategoryId
            };
        }


        public async Task<EventResponseDto> FindEventAsync(EventRequestDto eventRequestDto)
        {
            // 'await' anahtar kelimesi ile asenkron metod çağrısı yapılmalı
            var eventEntity = await _eventRepository.GetAsync(x => x.Id == eventRequestDto.Id);

            // Eğer etkinlik bulunamazsa, hata fırlatıyoruz
            if (eventEntity == null)
                throw new ApplicationException("Event not found.");

            // EventResponseDto'yu döndürüyoruz
            return new EventResponseDto
            {
                Id = eventEntity.Id,
                Title = eventEntity.Title
            };
        }


        // Tek bir etkinliği getirme metodu
        public async Task<EventResponseDto?> GetAsync(
            Expression<Func<Event, bool>> predicate,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        )
        {
            // Event repository ile etkinlik verisini alıyoruz
            var selectedEvent = await _eventRepository.GetAsync(predicate, withDeleted: withDeleted);

            if (selectedEvent == null)
                return null;

            // EventResponseDto döndürüyoruz
            return new EventResponseDto
            {
                Id = selectedEvent.Id,
                Title = selectedEvent.Title,
                Description = selectedEvent.Description,
                ImageUrl = selectedEvent.ImageUrl,
                StartDate = selectedEvent.StartDate,
                EndDate = selectedEvent.EndDate,
                ApplicationDeadline = selectedEvent.ApplicationDeadline,
                ParticipationText = selectedEvent.ParticipationText,
                CategoryId = selectedEvent.CategoryId
            };
        }


        // Etkinlikleri listeleme metodu
        public async Task<List<EventResponseDto>> GetListAsync(
            Expression<Func<Event, bool>>? predicate = null,
            Func<IQueryable<Event>, IOrderedQueryable<Event>>? orderBy = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        )
        {
            var events = await _eventRepository.GetListAsync(
                predicate,
                enableTracking: enableTracking,
                withDeleted: withDeleted
            );

            var filteredEvents = withDeleted
                ? events
                : events.Where(e => !e.IsDeleted).ToList();

            return filteredEvents.Select(e => new EventResponseDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                ImageUrl = e.ImageUrl,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                ApplicationDeadline = e.ApplicationDeadline,
                ParticipationText = e.ParticipationText,
                CategoryId = e.CategoryId
            }).ToList();
        }



        // Sayfalı etkinlik listesi getirme metodu
        public async Task<Paginate<EventResponseDto>> GetPaginateAsync(
            Expression<Func<Event, bool>>? predicate = null,
            Func<IQueryable<Event>, IOrderedQueryable<Event>>? orderBy = null,
            bool include = false,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        )
        {
            var paginateResult = await _eventRepository.GetPaginateAsync(
                predicate,
                orderBy: orderBy,
                index: index,
                size: size,
                enableTracking: enableTracking,
                withDeleted: withDeleted
            );

            return new Paginate<EventResponseDto>
            {
                Items = paginateResult.Items.Select(e => new EventResponseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    ImageUrl = e.ImageUrl,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    ApplicationDeadline = e.ApplicationDeadline,
                    ParticipationText = e.ParticipationText,
                    CategoryId = e.CategoryId
                }).ToList(),
                Index = paginateResult.Index,
                Size = paginateResult.Size,
                TotalItems = paginateResult.TotalItems,
                TotalPages = paginateResult.TotalPages
            };
        }


        // Etkinlik güncelleme metodu
        public async Task<EventResponseDto> UpdateAsync(EventUpdateRequestDto eventUpdateRequestDto)
        {

            var updatedEvent = await _eventRepository.GetAsync(x => x.Id == eventUpdateRequestDto.Id);

            if (updatedEvent == null)
                throw new ApplicationException("Event not found.");

            updatedEvent.Title = eventUpdateRequestDto.Title;
            updatedEvent.Description = eventUpdateRequestDto.Description;
            updatedEvent.ImageUrl = eventUpdateRequestDto.ImageUrl;
            updatedEvent.StartDate = eventUpdateRequestDto.StartDate;
            updatedEvent.EndDate = eventUpdateRequestDto.EndDate;
            updatedEvent.ApplicationDeadline = eventUpdateRequestDto.ApplicationDeadline;
            updatedEvent.ParticipationText = eventUpdateRequestDto.ParticipationText;
            updatedEvent.CategoryId = eventUpdateRequestDto.CategoryId;

            await _eventRepository.UpdateAsync(updatedEvent);

            return new EventResponseDto
            {
                Id = updatedEvent.Id,
                Title = updatedEvent.Title,
                Description = updatedEvent.Description,
                ImageUrl = updatedEvent.ImageUrl,
                StartDate = updatedEvent.StartDate,
                EndDate = updatedEvent.EndDate,
                ApplicationDeadline = updatedEvent.ApplicationDeadline,
                ParticipationText = updatedEvent.ParticipationText,
                CategoryId = updatedEvent.CategoryId
            };
        }
    }
}
