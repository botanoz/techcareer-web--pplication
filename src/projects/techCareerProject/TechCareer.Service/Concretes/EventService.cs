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
        //private readonly EventBusinessRules _eventBusinessRules;

        //public EventService(IEventRepository eventRepository, EventBusinessRules eventBusinessRules)
        //{
        //    _eventRepository = eventRepository;
        //    _eventBusinessRules = eventBusinessRules;
        //}


        public async Task<Event> AddAsync(Event Event)
        {
            Event addedEvent = await _eventRepository.AddAsync(Event);

            return addedEvent;
        }

        public async Task<Event> DeleteAsync(Event Event, bool permanent = false)
        {
            var selectedEvent = (await GetListAsync(x => x.Id == Event.Id)).FirstOrDefault();

            selectedEvent.IsDeleted = true;

            return selectedEvent;
        }

        public async Task<Event?> GetAsync(Expression<Func<Event, bool>> predicate, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            //var selectedEvent = (await _eventRepository.GetListAsync(x => x.Id == Event.Id)).FirstOrDefault();

            //return selectedEvent;
            return null;
        }

        public async Task<List<Event>> GetListAsync(Expression<Func<Event, bool>>? predicate = null, Func<IQueryable<Event>, IOrderedQueryable<Event>>? orderBy = null, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            List<Event> list = await _eventRepository.GetListAsync();

            return list;
        }

        public async Task<Paginate<Event>> GetPaginateAsync(Expression<Func<Event, bool>>? predicate = null, Func<IQueryable<Event>, IOrderedQueryable<Event>>? orderBy = null, bool include = false, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<Event> query = (IQueryable<Event>)_eventRepository.GetListAsync();


            if (!withDeleted)
            {
                query = query.Where(c => !c.IsDeleted);
            }


            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (!enableTracking)
            {
                query = query.AsNoTracking();
            }

            int totalItems = await query.CountAsync(cancellationToken);


            List<Event?> items = await query
                .Skip(index * size)
                .Take(size)
                .ToListAsync(cancellationToken);


            return new Paginate<Event?>
            {
                Items = items,
                Index = index,
                Size = size,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)size)
            };
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
