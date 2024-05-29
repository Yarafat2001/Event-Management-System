﻿using EventManagementSystem.DataAccess.Data;
using EventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EventManagementSystem.DataAccess.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly EventManagementSystemDbContext _eventManagementSystemDbContext;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public EventRepository(EventManagementSystemDbContext eventManagementSystemDbContext)
        {
            _eventManagementSystemDbContext = eventManagementSystemDbContext;
        }

        public async Task<Event?> GetByIdAsync(int eventId)
        {
            return await _eventManagementSystemDbContext.Events
                .Include(e => e.TicketTypes)
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == eventId);
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _eventManagementSystemDbContext.Events
                .OrderBy(e => e.Id)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetAllByTypeAsync(string? typeName)
        {
            var toDay = DateOnly.FromDateTime(DateTime.Now);
            if (Enum.TryParse(typeof(Category), typeName, true, out var existingCategory))
            {
                return await _eventManagementSystemDbContext.Events
                    .Where(e => e.Category.Equals((Category)existingCategory))
                    .Where(e => e.StartDate >= toDay)
                    .AsNoTracking()
                    .ToListAsync();
            }
            return await GetAllAsync();
        }

        public Task<IEnumerable<Ticket>> GetAllTicketAsync(int eventId, int ticketTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TicketType>> GetAllTicketTypeAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        public Task<Ticket?> GetTicketByIdAsync(int eventId, int ticketTypeId, int ticketId)
        {
            throw new NotImplementedException();
        }

        public async Task<TicketType?> GetTicketTypeByIdAsync(int ticketTypeId)
        {
            return await _eventManagementSystemDbContext.TicketTypes
                .Include(tt => tt.Event)
                .FirstOrDefaultAsync(tt => tt.Id == ticketTypeId);
        }

        public Task<string?> EventInformationAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        // Use on test.
        public async Task<int> UpdateTicketTypeAsync(Event updateEvent)
        {
            bool ticketTypeWithSameName = _eventManagementSystemDbContext.TicketTypes
                .AsEnumerable()
                .Any(tt => updateEvent.TicketTypes
                .Any(ett => ett.Name == tt.Name && ett.Id != tt.Id && ett.EventId == tt.EventId));

            if (ticketTypeWithSameName)
                throw new Exception("A Tickket type with the same name already exists");


            var eventToUpdate = await _eventManagementSystemDbContext.Events.FindAsync(updateEvent.Id);
            if (eventToUpdate is not null)
            {
                eventToUpdate.TicketTypes = updateEvent.TicketTypes;
                _eventManagementSystemDbContext.Events.Update(eventToUpdate);
                return await _eventManagementSystemDbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("The event to update can't be find");
            }
        }
    }
}
