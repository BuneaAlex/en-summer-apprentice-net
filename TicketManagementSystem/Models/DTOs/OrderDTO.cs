using Microsoft.Extensions.Logging;

namespace TicketManagementSystem.Models.DTOs
{
    [Serializable]
    public record OrderDTO(int orderID,
                       int eventID,
                       TicketCategoryDTO ticketCategory,
                       DateTime orderedAt,
                       int numberOfTickets,
                       float totalPrice)
    {
        public OrderDTO() : this(0, 0, new TicketCategoryDTO(), default, 0, 0.0f)
        {
        }
        public override string ToString()
        {
            return $"OrderID: {orderID}, EventID: {eventID}, TicketCategory: {ticketCategory}, OrderedAt: {orderedAt}, NumberOfTickets: {numberOfTickets}, TotalPrice: {totalPrice}";
        }
    }

    
}
