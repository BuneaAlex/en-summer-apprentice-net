using TicketManagementSystem.Models;
using TicketManagementSystem.Models.DTOs;

namespace TicketManagementSystem.Service
{
    public interface ITicketManagementService
    {
        List<Event> GetEvents();
        List<OrderDTO> GetOrderDTOs();
        OrderDTO UpdateOrder(Order order);
        Order GetOrderById(int id);
        TicketCategory GetTicketCategoryByEventIdAndDescription(int eventId, String description);

        OrderDTO DeleteOrder(int id);
    }
}
