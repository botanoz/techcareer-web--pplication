using Core.Persistence.Extensions;
using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.DataAccess.Repositories.Concretes;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Rules;

namespace TechCareer.Service.Concretes
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
       
        }

        public async Task<Event> AddAsync(Event Event)
        {
            try
            {
                Event addedEvent = await _eventRepository.AddAsync(Event);
                return addedEvent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException("Etkinlik eklenemedi", ex);
            }

        }

        public async Task<Event> DeleteAsync(Event Event, bool permanent = false)
        {
            try
            {
                var selectedEvent = (await GetListAsync(x => x.Id == Event.Id)).FirstOrDefault();
                selectedEvent.IsDeleted = true;

                return selectedEvent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException("Etkinlik silinemedi", ex);
            }

        }

        public async Task<Event?> GetAsync(
            Expression<Func<Event, bool>> predicate,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var query = _eventRepository.Query();

                if (!enableTracking)                {
                    query = query.AsNoTracking();                }

                if (!withDeleted)
                    query = query.Where(e => !e.IsDeleted);
                
                var selectedEvent = await query.FirstOrDefaultAsync(predicate, cancellationToken);

                return selectedEvent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException("Etkinlik getirilemedi", ex);
            }

        }


        public async Task<List<Event>> GetListAsync(Expression<Func<Event, bool>>? predicate = null, Func<IQueryable<Event>, IOrderedQueryable<Event>>? orderBy = null, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            try
            {
                List<Event> list = await _eventRepository.GetListAsync();

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException("Etkinlikler getirilemedi", ex);
            }

        }

        public async Task<Paginate<Event>> GetPaginateAsync(Expression<Func<Event, bool>>? predicate = null, Func<IQueryable<Event>, IOrderedQueryable<Event>>? orderBy = null, bool include = false, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            try
            {
                IQueryable<Event> query = (IQueryable<Event>)_eventRepository.GetListAsync();

                if (!withDeleted)
                    query = query.Where(c => !c.IsDeleted);

                if (predicate != null)
                    query = query.Where(predicate);

                if (!enableTracking)
                    query = query.AsNoTracking();

                int totalItems = await query.CountAsync(cancellationToken);
                List<Event> items = await query
                    .Skip(index * size)
                    .Take(size)
                    .ToListAsync(cancellationToken);

                return new Paginate<Event>
                {
                    Items = items,
                    Index = index,
                    Size = size,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)size)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                throw new ApplicationException(ex.Message, ex);
            }
        }

        public async Task<Event> UpdateAsync(Event Event)
        {
            var updatedEvent = (await GetListAsync(x => x.Id == Event.Id)).FirstOrDefault();
            if (updatedEvent != null)
            {
                updatedEvent = Event;
                return updatedEvent;
            }
           else
            {
                return NullReferenceException("Aradığınız kategori bulunamamıştır.");
            }
        }

        private Event NullReferenceException(string v)
        {
            throw new NotImplementedException();
        }
    }
}
