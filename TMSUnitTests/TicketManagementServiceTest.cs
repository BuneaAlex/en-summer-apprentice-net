using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Models.DTOs;
using TicketManagementSystem.Models;
using TicketManagementSystem.Persistence;
using TicketManagementSystem.Service;
using TicketManagementSystem.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace TMSUnitTests
{
    [TestClass]
    public class TicketManagementServiceTest
    {
        private Mock<IEventRepository> _eventRepositoryMock;
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<ITicketCategoryRepository> _ticketCategoryRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private TicketManagementService _ticketManagementService;

        [TestInitialize]
        public void Setup()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _ticketCategoryRepositoryMock = new Mock<ITicketCategoryRepository>();
            _mapperMock = new Mock<IMapper>();

            _ticketManagementService = new TicketManagementService(
                _eventRepositoryMock.Object,
                _orderRepositoryMock.Object,
                _ticketCategoryRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [TestMethod]
        public async Task GetEvents_ReturnsListOfEvents()
        {
            // Arrange
            List<Event> events = new List<Event>
            {
                new Event { Eventid = 1, Name = "Event 1" },
                new Event { Eventid = 2, Name = "Event 2" }
            };
            _eventRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(events);

            // Act
            var result = await _ticketManagementService.GetEvents();

            // Assert
            Assert.AreEqual(2, result.Count);
            CollectionAssert.AreEqual(events, result);
        }

        [TestMethod]
        public async Task GetEvents_ReturnsEmptyListOfEvents()
        {
            // Arrange
            List<Event> events = new List<Event>();
            
            _eventRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(events);

            // Act
            var result = await _ticketManagementService.GetEvents();

            // Assert
            Assert.AreEqual(0, result.Count);
            CollectionAssert.AreEqual(events, result);
        }

        [TestMethod]
        public async Task GetOrderDTOs_ReturnsListOfOrderDTOs()
        {
            // Arrange
            List<Order> orders = new List<Order>
            {
                new Order { Orderid = 1, NumberOfTickets = 2 },
                new Order { Orderid = 2, NumberOfTickets = 3 }
            };
            List<OrderDTO> orderDTOs = new List<OrderDTO>
            {
                new OrderDTO { orderID = 1, numberOfTickets = 2 },
                new OrderDTO { orderID = 2, numberOfTickets = 3 }
            };

            _orderRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(orders);
            _mapperMock.Setup(mapper => mapper.Map<List<OrderDTO>>(orders)).Returns(orderDTOs);

            // Act
            var result = await _ticketManagementService.GetOrderDTOs();

            // Assert
            Assert.AreEqual(2, result.Count);
            CollectionAssert.AreEqual(orderDTOs, result);
        }

        [TestMethod]
        public async Task GetOrderDTOs_ReturnsEmptyListOfOrderDTOs()
        {
            // Arrange
            List<Order> orders = new List<Order>();
            List<OrderDTO> orderDTOs = new List<OrderDTO>();
            
            _orderRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(orders);
            _mapperMock.Setup(mapper => mapper.Map<List<OrderDTO>>(orders)).Returns(orderDTOs);

            // Act
            var result = await _ticketManagementService.GetOrderDTOs();

            // Assert
            Assert.AreEqual(0, result.Count);
            CollectionAssert.AreEqual(orderDTOs, result);
        }

        [TestMethod]
        public async Task UpdateOrder_ReturnsUpdatedOrderDTO()
        {
            // Arrange
            int orderId = 1;
            Order orderToUpdate = new Order { Orderid = orderId, NumberOfTickets = 3 };
            Order updatedOrder = new Order { Orderid = orderId, NumberOfTickets = 5 };
            OrderDTO expectedOrderDTO = new OrderDTO { orderID = orderId, numberOfTickets = 5 };

            _orderRepositoryMock.Setup(repo => repo.Update(orderToUpdate)).ReturnsAsync(updatedOrder);
            _mapperMock.Setup(mapper => mapper.Map<OrderDTO>(updatedOrder)).Returns(expectedOrderDTO);

            // Act
            var result = await _ticketManagementService.UpdateOrder(orderToUpdate);

            // Assert
            Assert.AreEqual(expectedOrderDTO, result);
        }

        [TestMethod]
        public async Task GetOrderById_ReturnsOrder()
        {
            // Arrange
            int orderId = 1;
            Order expectedOrder = new Order { Orderid = orderId, NumberOfTickets = 3 };

            _orderRepositoryMock.Setup(repo => repo.GetById(orderId)).ReturnsAsync(expectedOrder);

            // Act
            var result = await _ticketManagementService.GetOrderById(orderId);

            // Assert
            Assert.AreEqual(expectedOrder, result);
        }

        [TestMethod]
        public async Task GetOrderById_NoOrderFound()
        {
            // Arrange
            int orderId = 100;

            _orderRepositoryMock.Setup(repo => repo.GetById(orderId)).ThrowsAsync(new EntityNotFoundException());

            // Assert
            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(() => _ticketManagementService.GetOrderById(orderId));
        }

        [TestMethod]
        public void GetTicketCategoryByEventIdAndDescription_ReturnsNoTicketCategory()
        {
            // Arrange

            _ticketCategoryRepositoryMock.Setup(repo => repo.GetTicketCategoryByEventIdAndDescription(It.IsAny<int>(), It.IsAny<string>())).Throws(new EntityNotFoundException());

            // Assert
            Assert.ThrowsException<EntityNotFoundException>(() => _ticketManagementService.GetTicketCategoryByEventIdAndDescription(It.IsAny<int>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void GetTicketCategoryByEventIdAndDescription_ReturnsTicketCategory()
        {
            // Arrange
            int eventId = 1;
            string description = "VIP";
            TicketCategory expectedTicketCategory = new TicketCategory { Description = "VIP", Price = 150 };

            _ticketCategoryRepositoryMock.Setup(repo => repo.GetTicketCategoryByEventIdAndDescription(eventId, description)).Returns(expectedTicketCategory);

            // Act
            var result = _ticketManagementService.GetTicketCategoryByEventIdAndDescription(eventId, description);

            // Assert
            Assert.AreEqual(expectedTicketCategory, result);
        }

        [TestMethod]
        public async Task DeleteOrder_ReturnsDeletedOrderDTO()
        {
            // Arrange
            int orderId = 1;
            Order orderToDelete = new Order { Orderid = orderId, NumberOfTickets = 3 };
            Order deletedOrder = new Order { Orderid = orderId, NumberOfTickets = 0 };
            OrderDTO expectedOrderDTO = new OrderDTO { orderID = orderId, numberOfTickets = 0 };

            _orderRepositoryMock.Setup(repo => repo.Delete(orderId)).ReturnsAsync(deletedOrder);
            _mapperMock.Setup(mapper => mapper.Map<OrderDTO>(deletedOrder)).Returns(expectedOrderDTO);

            // Act
            var result = await _ticketManagementService.DeleteOrder(orderId);

            // Assert
            Assert.AreEqual(expectedOrderDTO, result);
        }
    }
}
