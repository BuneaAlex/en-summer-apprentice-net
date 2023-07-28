using TicketManagementSystem.Persistence;
using TicketManagementSystem.Service;
using Moq;
using AutoMapper;
using TicketManagementSystem.Models.DTOs;
using TicketManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using TicketManagementSystem.Controllers;
using NLog;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using TicketManagementSystem.Exceptions;

namespace TMSUnitTests
{
    [TestClass]
    public class AppControllerTest
    {
        Mock<ITicketManagementService> _service;
        List<OrderDTO> _mockOrders;
        Mock<ILogger<AppController>> _logger;

        [TestInitialize]
        public void SetupMoqData()
        {
            _service = new Mock<ITicketManagementService>();
            _logger = new Mock<ILogger<AppController>>();
            TicketCategoryDTO ticketCategory1 = new TicketCategoryDTO(1, "Standard", 200.0f);

            OrderDTO order1 = new OrderDTO(
                orderID: 101,
                eventID: 2001,
                ticketCategory: ticketCategory1,
                orderedAt: DateTime.Now,
                numberOfTickets: 2,
                totalPrice: 50.0f
            );

            TicketCategoryDTO ticketCategory2 = new TicketCategoryDTO(2, "VIP", 400.0f);

            OrderDTO order2 = new OrderDTO(
                orderID: 102,
                eventID: 2002,
                ticketCategory: ticketCategory2,
                orderedAt: DateTime.Now.AddDays(-1),
                numberOfTickets: 4,
                totalPrice: 150.0f
            );
            _mockOrders = new List<OrderDTO>
            {
                order1,order2
            };
        }

        [TestMethod]
        public async Task GetOrdersTestReturnListOfOrders()
        {
            //Arrange
            _service.Setup(moq => moq.GetOrderDTOs()).ReturnsAsync(_mockOrders);

            var controller = new AppController(_service.Object, _logger.Object);

            //Act
            var orders = await controller.GetOrders();
            var ordersResult = orders.Result as OkObjectResult;
            var ordersList = ordersResult.Value as List<OrderDTO>;
            var ordersCount = ordersList.Count;

            //Assert
            Assert.IsNotNull(orders);
            Assert.AreEqual(2, ordersCount);
            CollectionAssert.AreEqual(_mockOrders, ordersList);
        }

        [TestMethod]
        public async Task GetOrdersTestReturnEmptyList()
        {
            //Arrange
            _service.Setup(moq => moq.GetOrderDTOs()).ReturnsAsync(new List<OrderDTO>() { });

            //Act
            var controller = new AppController(_service.Object, _logger.Object);
            var orders = await controller.GetOrders();
            var ordersResult = orders.Result as OkObjectResult;
            var ordersList = ordersResult.Value as List<OrderDTO>;
            var ordersCount = ordersList.Count;

            //Assert
            Assert.IsNotNull(orders);
            Assert.AreEqual(0, ordersCount);
        }


        [TestMethod]
        public async Task DeleteOrderSuccededTest()
        {
            //Arrange
            TicketCategoryDTO ticketCategory1 = new TicketCategoryDTO(1, "Standard", 200.0f);
            OrderDTO orderToDelete = new OrderDTO(
                orderID: 101,
                eventID: 2001,
                ticketCategory: ticketCategory1,
                orderedAt: DateTime.Now,
                numberOfTickets: 2,
                totalPrice: 50.0f
            );

            _service.Setup(moq => moq.DeleteOrder(101)).ReturnsAsync(orderToDelete);

            //Act
            var controller = new AppController(_service.Object, _logger.Object);
            var result = await controller.DeleteOrder(101);

            var orderResult = (OkObjectResult)result.Result;
            var orderDeletedResult = orderResult.Value as OrderDTO;

            //Assert
            Assert.IsTrue(result.Result is OkObjectResult);
            Assert.IsTrue(orderResult.StatusCode == 200);
            Assert.AreEqual(orderToDelete, orderDeletedResult);
        }

        [TestMethod]
        public async Task UpdateOrder_SuccessfulUpdate_ReturnsOkResult()
        {
            //Arrange
            int orderId = 1;
            OrderPatchRequest orderPatchRequest = new OrderPatchRequest(5, "VIP");
            var order = new Order
            {
                Orderid = orderId,
                NumberOfTickets = 3,
                TicketCategory = new TicketCategory
                {
                    Description = "Standard",
                    Price = 100
                }
            };

            _service.Setup(moq => moq.GetOrderById(orderId)).ReturnsAsync(order);

            var updatedTicketCategory = new TicketCategory
            {
                Description = "VIP",
                Price = 150
            };

            order.TicketCategory.Event = new Event { Eventid = 123 };

            _service.Setup(moq => moq.GetTicketCategoryByEventIdAndDescription(order.TicketCategory.Event.Eventid, orderPatchRequest.ticketType)).Returns(updatedTicketCategory);

            _service.Setup(moq => moq.UpdateOrder(It.IsAny<Order>())).ReturnsAsync(new OrderDTO
            {
                orderID = order.Orderid,
                eventID = order.TicketCategory.Event.Eventid,
                ticketCategory = new TicketCategoryDTO
                {
                    description = updatedTicketCategory.Description,
                    price = (float)updatedTicketCategory.Price
                },
                orderedAt = DateTime.Now,
                numberOfTickets = orderPatchRequest.numberOfTickets,
                totalPrice = (float)(orderPatchRequest.numberOfTickets * updatedTicketCategory.Price)
            });

            //Act
            var controller = new AppController(_service.Object, _logger.Object);
            var result = await controller.UpdateOrder(orderId, orderPatchRequest);

            var okResult = (OkObjectResult)result.Result;
            var orderDTO = (OrderDTO)okResult.Value;

            //Assert
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(orderId, orderDTO.orderID);
            Assert.AreEqual(orderPatchRequest.numberOfTickets, orderDTO.numberOfTickets);
            Assert.AreEqual(updatedTicketCategory.Description, orderDTO.ticketCategory.description);
            Assert.AreEqual(orderPatchRequest.numberOfTickets * updatedTicketCategory.Price, orderDTO.totalPrice);
        }

        [TestMethod]
        public async Task UpdateOrder_NegativeNumberOfTickets_ThrowsException()
        {
            // Arrange
            int orderId = 1;
            var orderPatchRequest = new OrderPatchRequest(-5, "VIP");

            var order = new Order
            {
                Orderid = orderId,
                NumberOfTickets = 3,
                TicketCategory = new TicketCategory
                {
                    Description = "Standard",
                    Price = 100
                }
            };

            _service.Setup(moq => moq.GetOrderById(orderId)).ReturnsAsync(order);
            _service.Setup(moq => moq.GetTicketCategoryByEventIdAndDescription(It.IsAny<int>(), It.IsAny<string>())).Returns((TicketCategory)null);
            //Act
            var controller = new AppController(_service.Object, _logger.Object);

            //Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => controller.UpdateOrder(orderId, orderPatchRequest));
        }

        [TestMethod]
        public async Task UpdateOrder_SameTicketType_NoTicketCategoryUpdate()
        {
            //Arrange
            int orderId = 1;
            OrderPatchRequest orderPatchRequest = new OrderPatchRequest(5, "VIP");
            var order = new Order
            {
                Orderid = orderId,
                NumberOfTickets = 3,
                TicketCategory = new TicketCategory
                {
                    Description = "Standard",
                    Price = 100
                }
            };

            _service.Setup(moq => moq.GetOrderById(orderId)).ReturnsAsync(order);

            var updatedTicketCategory = new TicketCategory
            {
                Description = "Standard",
                Price = 100
            };

            order.TicketCategory.Event = new Event { Eventid = 123 };

            _service.Setup(moq => moq.GetTicketCategoryByEventIdAndDescription(order.TicketCategory.Event.Eventid, orderPatchRequest.ticketType)).Returns(updatedTicketCategory);

            _service.Setup(moq => moq.UpdateOrder(It.IsAny<Order>())).ReturnsAsync(new OrderDTO
            {
                orderID = order.Orderid,
                eventID = order.TicketCategory.Event.Eventid,
                ticketCategory = new TicketCategoryDTO
                {
                    description = updatedTicketCategory.Description,
                    price = (float)updatedTicketCategory.Price
                },
                orderedAt = DateTime.Now,
                numberOfTickets = orderPatchRequest.numberOfTickets,
                totalPrice = (float)(orderPatchRequest.numberOfTickets * updatedTicketCategory.Price)
            });

            //Act
            var controller = new AppController(_service.Object, _logger.Object);
            var result = await controller.UpdateOrder(orderId, orderPatchRequest);

            var okResult = (OkObjectResult)result.Result;
            var orderDTO = (OrderDTO)okResult.Value;

            //Assert
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(orderId, orderDTO.orderID);
            Assert.AreEqual(orderPatchRequest.numberOfTickets, orderDTO.numberOfTickets);
            Assert.AreEqual(updatedTicketCategory.Description, orderDTO.ticketCategory.description);
            Assert.AreEqual(orderPatchRequest.numberOfTickets * updatedTicketCategory.Price, orderDTO.totalPrice);
        }

    }
}