using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TicketManagementSystem.Models;
using TicketManagementSystem.Models.DTOs;
using TicketManagementSystem.Persistence;

namespace TicketManagementSystem.Service
{
    public class TicketManagementService :ITicketManagementService
    {
        private IEventRepository _eventRepository;
        private IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public TicketManagementService(IEventRepository eventRepository,IOrderRepository orderRepository,IMapper mapper) { 
            _eventRepository = eventRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public List<Event> GetEvents()
        {
            return _eventRepository.GetAll().ToList();
        }

        public List<OrderDTO> GetOrderDTOs()
        {
            List<Order> orders = _orderRepository.GetAll().ToList();
            List<OrderDTO> orderDtos = _mapper.Map<List<OrderDTO>>(orders);
            return orderDtos;
        }
    }
}
