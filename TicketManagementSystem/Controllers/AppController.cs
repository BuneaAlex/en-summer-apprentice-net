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
    }
}
