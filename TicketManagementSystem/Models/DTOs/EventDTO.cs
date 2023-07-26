namespace TicketManagementSystem.Models.DTOs
{
    [Serializable]
    public record EventDTO(int eventID,
                        VenueDTO venue,
                        String eventType,
                        String name,
                        String description,
                        DateTime startDate,
                        DateTime endDate,
                        List<TicketCategoryDTO> ticketCategories,
                        String image)
    { }
}
