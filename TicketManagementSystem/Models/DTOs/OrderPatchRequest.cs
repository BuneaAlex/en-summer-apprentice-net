using System.Net.Sockets;

namespace TicketManagementSystem.Models.DTOs
{
    public record OrderPatchRequest(int numberOfTickets,
                                    String ticketType)                              
    {
        public override string ToString()
        {
            return $"NumberOfTickets: {numberOfTickets}, TicketType: {ticketType}";
        }
    }
}
