using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using System.Text.Json;
using TicketManagementSystem.Models;
using TicketManagementSystem.Service;
using TicketManagementSystem.Models.DTOs;
using TicketManagementSystem.Exceptions;

namespace TicketManagementSystem.Controllers
{
    [Route("management/")]
    [ApiController]
    public class AppController : Controller
    {
        private readonly ITicketManagementService _service;
        private readonly ILogger<AppController> _logger;

        public AppController(ITicketManagementService service,ILogger<AppController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("orders")]
        public async Task<ActionResult<List<OrderDTO>>> GetOrders()
        {
            var orders = await _service.GetOrderDTOs();
            return Ok(orders);
        }

        [HttpPatch("orders/{id}")]
        public async Task<ActionResult<OrderDTO>> UpdateOrder(int id, [FromBody] OrderPatchRequest orderPatchRequest)
        {
            Order order = await _service.GetOrderById(id);
            _logger.LogInformation(orderPatchRequest.ToString());
            
            if (orderPatchRequest.numberOfTickets <= 0)
                throw new ArgumentException("Number of tickets can't be 0 or negative");


            if (order.TicketCategory.Description != orderPatchRequest.ticketType)
            {
                TicketCategory ticket = _service.GetTicketCategoryByEventIdAndDescription(order.TicketCategory.Event.Eventid, orderPatchRequest.ticketType);
                ticket.Event = order.TicketCategory.Event;
                order.TicketCategory = ticket;   
            }

            var currentNumberOfTickets = order.NumberOfTickets;
            var newNumberOfTickets = orderPatchRequest.numberOfTickets;
            var differeceNumberOfTickets = currentNumberOfTickets - newNumberOfTickets;
            if (differeceNumberOfTickets < 0)
            {
                if (order.TicketCategory.NoAvailable < -differeceNumberOfTickets)
                    throw new NotEnoughTicketsException("Cannot update because there are not enough tickets left!");   
            }
            order.TicketCategory.NoAvailable += differeceNumberOfTickets;
            await _service.UpdateTicketCategory(order.TicketCategory);

            order.NumberOfTickets = newNumberOfTickets;


            order.TotalPrice = order.NumberOfTickets * order.TicketCategory.Price;
            var orderUpdated = await _service.UpdateOrder(order);
            return Ok(orderUpdated);
            
            
        }

        [HttpDelete("orders/{id}")]
        public async Task<ActionResult<OrderDTO>> DeleteOrder(int id)
        {
            var orderDeleted = await _service.DeleteOrder(id);
            return Ok(orderDeleted);
            
        }
    }
}
