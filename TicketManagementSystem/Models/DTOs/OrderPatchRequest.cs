namespace TicketManagementSystem.Models.DTOs
{
    public record OrderPatchRequest(int numberOfTickets,
                                    String ticketType)                              
    {
    }
}
