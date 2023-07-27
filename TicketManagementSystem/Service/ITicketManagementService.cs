using TicketManagementSystem.Models;
using TicketManagementSystem.Models.DTOs;

namespace TicketManagementSystem.Service
{
    public interface ITicketManagementService
    {
        Task<List<Event>> GetEvents();
        Task<List<OrderDTO>> GetOrderDTOs();
        Task<OrderDTO> UpdateOrder(Order order);
        Task<Order> GetOrderById(int id);
        TicketCategory GetTicketCategoryByEventIdAndDescription(int eventId, String description);

        Task<OrderDTO> DeleteOrder(int id);
    }
}
