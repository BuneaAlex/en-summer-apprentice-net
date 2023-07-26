namespace TicketManagementSystem.Models.DTOs
{
    [Serializable]
    public record TicketCategoryDTO(int id,
                                String description,
                                float price)
    {
        public TicketCategoryDTO() : this(0, string.Empty, 0.0f)
        {
        }
    }
}
