namespace TicketManagementSystem.Models.DTOs
{
    [Serializable]
    public record VenueDTO(String type,
                       int capacity,
                       String location)
    { }
}
