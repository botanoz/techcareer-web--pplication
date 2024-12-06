using Core.CrossCuttingConcerns.Serilog;
using Core.Persistence.Extensions;
using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Core;
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
        private readonly LoggerServiceBase _logger;

        public EventService(IEventRepository eventRepository, LoggerServiceBase logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
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

                _logger.Info("Info log: Event added.");

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
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }
        }

        // Etkinlik silme metodu
        public async Task<EventResponseDto> DeleteAsync(EventRequestDto eventRequestDto, bool permanent = false)
        {
            try
            {
                var selectedEvent = await _eventRepository.GetAsync(
                    x => x.Id == eventRequestDto.Id,
                    withDeleted: true
                );

                if (selectedEvent == null)
                    throw new ApplicationException("Event not found.");

                if (permanent)
                {
                    await _eventRepository.DeleteAsync(selectedEvent, true);
                }
                else
                {
                    selectedEvent.IsDeleted = true;
                    await _eventRepository.DeleteAsync(selectedEvent);
                }

                _logger.Info("Info log: Event deleted.");

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
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

        }


        public async Task<EventResponseDto> FindEventAsync(EventRequestDto eventRequestDto)
        {
            try
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
                    Title = eventEntity.Title,
                    Description = eventEntity.Description,
                    ImageUrl = eventEntity.ImageUrl,
                    StartDate = eventEntity.StartDate,
                    EndDate = eventEntity.EndDate,
                    ApplicationDeadline = eventEntity.ApplicationDeadline,
                    ParticipationText = eventEntity.ParticipationText,
                    CategoryId = eventEntity.CategoryId
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

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
            try
            {
                // Event repository ile etkinlik verisini alıyoruz
                var selectedEvent = await _eventRepository.GetAsync(predicate, withDeleted: withDeleted);

                if (selectedEvent == null)
                {
                    _logger.Warn("Warn log: Event not found.");
                    throw new ApplicationException("Event not found.");
                }

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
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

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
            try
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
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

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
            try
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
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }

        }


        // Etkinlik güncelleme metodu
        public async Task<EventResponseDto> UpdateAsync(EventUpdateRequestDto eventUpdateRequestDto)
        {
            try
            {
                var updatedEvent = await _eventRepository.GetAsync(x => x.Id == eventUpdateRequestDto.Id);

                if (updatedEvent == null)
                {
                    _logger.Warn("Warn log: Event not found.");
                    throw new ApplicationException("Event not found.");
                }

                updatedEvent.Title = eventUpdateRequestDto.Title;
                updatedEvent.Description = eventUpdateRequestDto.Description;
                updatedEvent.ImageUrl = eventUpdateRequestDto.ImageUrl;
                updatedEvent.StartDate = eventUpdateRequestDto.StartDate;
                updatedEvent.EndDate = eventUpdateRequestDto.EndDate;
                updatedEvent.ApplicationDeadline = eventUpdateRequestDto.ApplicationDeadline;
                updatedEvent.ParticipationText = eventUpdateRequestDto.ParticipationText;
                updatedEvent.CategoryId = eventUpdateRequestDto.CategoryId;

                await _eventRepository.UpdateAsync(updatedEvent);

                _logger.Info("Info log: Event updated.");

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
            catch (Exception ex)
            {
                _logger.Error($"Error log: {ex}");
                throw new Exception("An error occurred. Please try again later.", ex);
            }
        }
    }
}
