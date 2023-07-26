namespace TicketManagementSystem.Models.DTOs
{
    using AutoMapper;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.orderID, opt => opt.MapFrom(src => src.Orderid))
                .ForMember(dest => dest.eventID, opt => opt.MapFrom(src => src.TicketCategoryid))
                .ForMember(dest => dest.ticketCategory, opt => opt.MapFrom(src => src.TicketCategory))
                .ForMember(dest => dest.orderedAt, opt => opt.MapFrom(src => src.OrderedAt))
                .ForMember(dest => dest.numberOfTickets, opt => opt.MapFrom(src => src.NumberOfTickets ?? 0))
                .ForMember(dest => dest.totalPrice, opt => opt.MapFrom(src => src.TotalPrice ?? 0.0f));

            CreateMap<TicketCategory, TicketCategoryDTO>();
        }
    }

}
