using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Models;
using TicketManagementSystem.Exceptions;

namespace TicketManagementSystem.Persistence
{
    public class TicketCategoryRepository : ITicketCategoryRepository
    {
        private readonly TicketManagementSystemContext _dbcontext;
        public TicketCategoryRepository(TicketManagementSystemContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public void Add(TicketCategory entity)
        {
            throw new NotImplementedException();
        }

        public async Task<TicketCategory> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TicketCategory>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<TicketCategory> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<TicketCategory> Update(TicketCategory entity)
        {
            throw new NotImplementedException();
        }

        public TicketCategory GetTicketCategoryByEventIdAndDescription(int eventId, string description)
        {
            var ticketCategory = _dbcontext.TicketCategories
                .Include(t => t.Event)
                .FirstOrDefault(t => t.Eventid == eventId && t.Description == description);
            if (ticketCategory == null)
            {
                throw new EntityNotFoundException("Ticket category does not exist!");
            }
            return ticketCategory;
        }

    }
}
