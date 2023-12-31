﻿using AutoMapper;
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
        private ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IMapper _mapper;

        public TicketManagementService(IEventRepository eventRepository,
                                       IOrderRepository orderRepository,
                                       ITicketCategoryRepository ticketCategoryRepository,
                                       IMapper mapper) { 
            _eventRepository = eventRepository;
            _orderRepository = orderRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _mapper = mapper;
        }

        public async Task<List<Event>> GetEvents()
        {
            return (List<Event>)await _eventRepository.GetAll();
        }

        public async Task<List<OrderDTO>> GetOrderDTOs()
        {
            List<Order> orders = (List<Order>)await _orderRepository.GetAll();
            List<OrderDTO> orderDtos = _mapper.Map<List<OrderDTO>>(orders);
            return orderDtos;
        }

        

        public async Task<OrderDTO> UpdateOrder(Order order)
        {
            Order orderUpdated = await _orderRepository.Update(order);
            OrderDTO orderDTO = _mapper.Map<OrderDTO>(orderUpdated);
            return orderDTO;
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _orderRepository.GetById(id);
        }

        public TicketCategory GetTicketCategoryByEventIdAndDescription(int eventId, string description)
        {
            return _ticketCategoryRepository.GetTicketCategoryByEventIdAndDescription(eventId,description);
        }

        public async Task<OrderDTO> DeleteOrder(int id)
        {
            Order orderDeleted = await _orderRepository.Delete(id);
            OrderDTO orderDTO = _mapper.Map<OrderDTO>(orderDeleted);
            TicketCategory ticketCategory = orderDeleted.TicketCategory;
            ticketCategory.NoAvailable += orderDeleted.NumberOfTickets;
            _ticketCategoryRepository.Update(ticketCategory);

            return orderDTO;
        }

        public async Task<TicketCategory> UpdateTicketCategory(TicketCategory ticketCategory)
        {
            var ticketUpdated = await _ticketCategoryRepository.Update(ticketCategory);
            return ticketUpdated;
        }
    }
}
