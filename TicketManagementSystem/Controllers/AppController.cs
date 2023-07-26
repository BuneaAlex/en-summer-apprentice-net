using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using System.Text.Json;
using TicketManagementSystem.Models;
using TicketManagementSystem.Service;
using TicketManagementSystem.Models.DTOs;

namespace TicketManagementSystem.Controllers
{
    [Route("management/")]
    [ApiController]
    public class AppController : Controller
    {
        private readonly ITicketManagementService _service;

        public AppController(ITicketManagementService service)
        {
            _service = service;
        }

        [HttpGet("orders")]
        public ActionResult<List<OrderDTO>> GetOrders()
        {
            var orders = _service.GetOrderDTOs();
            return Ok(orders);
        }

        [HttpPut("orders/{id}")]
        public ActionResult<OrderDTO> UpdateOrder(int id, [FromBody] OrderPatchRequest orderPatchRequest)
        {
            Order order = _service.GetOrderById(id);

            if(order != null)
            {
                if (orderPatchRequest.numberOfTickets <= 0)
                    return BadRequest("Number of tickets can't be 0 or negative");

                order.NumberOfTickets = orderPatchRequest.numberOfTickets;

                if (order.TicketCategory.Description != orderPatchRequest.ticketType)
                {
                    TicketCategory ticket = _service.GetTicketCategoryByEventIdAndDescription(order.TicketCategory.Event.Eventid, orderPatchRequest.ticketType);
                    if(ticket!=null)
                    {
                        ticket.Event = order.TicketCategory.Event;
                        order.TicketCategory = ticket;
                    }
                    else
                    {
                        return NotFound("Ticket Category does not exist!");
                    }
                }

                order.TotalPrice = order.NumberOfTickets * order.TicketCategory.Price;
                var orderUpdated = _service.UpdateOrder(order);
                return Ok(orderUpdated);
            }

            return NotFound("Order does not exist!");
            
        }

        [HttpDelete("orders/{id}")]
        public ActionResult<OrderDTO> DeleteOrder(int id)
        {
            var orderDeleted = _service.DeleteOrder(id);
            if(orderDeleted != null)
                return Ok(orderDeleted);
            return NotFound("Order does not exist!");
        }
    }
}
