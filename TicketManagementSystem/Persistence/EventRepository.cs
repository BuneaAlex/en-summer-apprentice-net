﻿using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Persistence
{
    public class EventRepository : IEventRepository
    {
        private readonly TicketManagementSystemContext _dbcontext;
        public EventRepository(TicketManagementSystemContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public void Add(Event entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Event> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Event>> GetAll()
        {
            return _dbcontext.Events
                .Include(e => e.EventType) // Include related EventType
                .Include(e => e.Venue) // Include related Venue
                .Include(e => e.TicketCategories)
                .ToList(); // Include related TicketCategories
                
        }

        public async Task<Event> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Event> Update(Event entity)
        {
            throw new NotImplementedException();
        }
    }
}
