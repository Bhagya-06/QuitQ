using global::Moq;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using QuitQ.API.Models;

namespace QuitQ.Tests.Moq
{

    public class AdminControllerTests
    {
        private Mock<IAdminService> _serviceMock;
        private AdminController _controller;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IAdminService>();
            _controller = new AdminController(_serviceMock.Object);
        }

        [Test]
        public async Task GetAllUsers_ReturnsOkResult()
        {
            var users = new List<User>{new User { Id = 1, Name = "Bhagya" }};

            _serviceMock.Setup(x => x.GetAllUsers()).ReturnsAsync(users);

            var result = await _controller.GetAllUsers();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task DeleteUser_CallsService()
        {
            await _controller.DeleteUser(1);

            _serviceMock.Verify(x => x.DeleteUser(1), Times.Once);
        }
    }
}
