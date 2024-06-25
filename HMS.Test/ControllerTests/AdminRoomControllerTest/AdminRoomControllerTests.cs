using AutoMapper;
using HMS.Models.Admin;
using HMS.Controllers.Admin;
using HMS.Services.Repository_Service;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.DTOs.Admin;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Test.ControllerTests.AdminRoomControllerTest
{
    [TestFixture]
    public class AdminRoomControllerTests
    {
        private Mock<IRepositoryService<AdminRoom>> _mockRepositoryService;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<AdminRoomController>> _mockLogger;
        private AdminRoomController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRepositoryService = new Mock<IRepositoryService<AdminRoom>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<AdminRoomController>>();

            _controller = new AdminRoomController(
                _mockLogger.Object,
                _mockRepositoryService.Object,
                _mockMapper.Object
            );
        }

        [Test]
        public async Task GetAdminRooms_ReturnsOk_WithAdminRooms()
        {
            // Arrange
            var adminRooms = new List<AdminRoom> { new AdminRoom { Id = new Guid(), Title = "Room 1" } };
            var adminRoomDTOs = new List<AdminRoomDTO> { new AdminRoomDTO { Title = "Room 1" } };

            _mockRepositoryService.Setup(repo => repo.GetAllAsync()).ReturnsAsync(adminRooms);
            _mockMapper.Setup(m => m.Map<AdminRoomDTO>(It.IsAny<AdminRoom>())).Returns(adminRoomDTOs.First());

            // Act
            var result = await _controller.GetAdminRooms();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                var okResult = result.Result as OkObjectResult;
                Assert.IsNotNull(okResult);
                Assert.IsAssignableFrom<IEnumerable<AdminRoomDTO>>(okResult.Value);
                var adminRoomsReturned = okResult.Value as IEnumerable<AdminRoomDTO>;
                Assert.IsNotNull(adminRoomsReturned);
                Assert.That(adminRoomsReturned.Count(), Is.EqualTo(1));
                Assert.That(adminRoomsReturned.First().Title, Is.EqualTo("Room 1"));
            });

        }

        [Test]
        public async Task GetAdminRooms_ReturnsNotFound_WhenNoAdminRooms()
        {
            // Arrange
            _mockRepositoryService.Setup(repo => repo.GetAllAsync()).ReturnsAsync((List<AdminRoom>)null);

            // Act
            var result = await _controller.GetAdminRooms();

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            Assert.That(notFoundResult.Value, Is.EqualTo("No AdminRooms available."));
        }

        [Test]
        public async Task GetAdminRooms_ReturnsInternalServerError_OnException()
        {
            // Arrange
            _mockRepositoryService.Setup(repo => repo.GetAllAsync()).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetAdminRooms();

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result.Result);
            var statusCodeResult = result.Result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(statusCodeResult.StatusCode, Is.EqualTo(500));
                Assert.That(statusCodeResult.Value, Is.EqualTo("An error occurred while retrieving AdminRooms."));
            });
        }

        [Test]
        public async Task GetAdminRoomWithId_ReturnsOk_WithAdminRoom()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var adminRoom = new AdminRoom { Id = id, Title = "Room 1" };
            var adminRoomDTO = new AdminRoomDTO { Title = "Room 1" };
            _mockRepositoryService.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(adminRoom);
            _mockMapper.Setup(m => m.Map<AdminRoomDTO>(It.IsAny<AdminRoom>())).Returns(adminRoomDTO);

            //Act
            var result = await _controller.GetAdminRoom(id);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                var okResult = result.Result as OkObjectResult;
                Assert.IsNotNull(okResult);
                Assert.IsAssignableFrom<IEnumerable<AdminRoomDTO>>(okResult.Value);
                var adminRoomReturned = okResult.Value as AdminRoomDTO;
                Assert.IsNotNull(adminRoomReturned);
                Assert.That(adminRoomReturned.Title, Is.EqualTo("Room 1"));

            });
        }
    }
}
