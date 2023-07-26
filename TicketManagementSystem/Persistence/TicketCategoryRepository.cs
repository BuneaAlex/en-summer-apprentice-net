using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Models;

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

        public TicketCategory Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TicketCategory> GetAll()
        {
            throw new NotImplementedException();
        }

        public TicketCategory GetById(int id)
        {
            throw new NotImplementedException();
        }

        public TicketCategory Update(TicketCategory entity)
        {
            throw new NotImplementedException();
        }

        public TicketCategory GetTicketCategoryByEventIdAndDescription(int eventId, string description)
        {
            var ticketCategory = _dbcontext.TicketCategories
                .Include(t => t.Event)
                .FirstOrDefault(t => t.Eventid == eventId && t.Description == description);
            return ticketCategory;
        }

    }
}
