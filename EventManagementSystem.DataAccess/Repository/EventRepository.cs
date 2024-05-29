﻿using EventManagementSystem.DataAccess.Data;
using EventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

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
            return await _eventManagementSystemDbContext.Events.FindAsync(eventId);
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

        public Task<TicketType?> GetTicketTypeByIdAsync(int eventId, int ticketTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<string?> EventInformationAsync(int eventId)
        {
            throw new NotImplementedException();
        }
    }
}
