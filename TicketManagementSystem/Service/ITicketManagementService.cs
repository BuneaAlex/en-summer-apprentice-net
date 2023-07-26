using TicketManagementSystem.Models;
using TicketManagementSystem.Models.DTOs;

namespace TicketManagementSystem.Service
{
    public interface ITicketManagementService
    {
        List<Event> GetEvents();
        List<OrderDTO> GetOrderDTOs();
        OrderDTO UpdateOrder(Order order);
    }
}
