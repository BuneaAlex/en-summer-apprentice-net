using TicketManagementSystem.Models;

namespace TicketManagementSystem.Persistence
{
    public interface ITicketCategoryRepository : IRepository<TicketCategory, int>
    {
        TicketCategory GetTicketCategoryByEventIdAndDescription(int eventId, String description);
    }
}
