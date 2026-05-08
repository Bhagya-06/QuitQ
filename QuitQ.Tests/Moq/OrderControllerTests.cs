using global::Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuitQ.API.Controllers;
using QuitQ.API.Services.Interfaces;
using System.Security.Claims;
using QuitQ.API.Models;

namespace QuitQ.Tests.Moq
{
    public class OrderControllerTests
    {
        private Mock<IOrderService> _serviceMock;
        private OrderController _controller;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IOrderService>();

            _controller = new OrderController(_serviceMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, "1")
        }));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };
        }

        [Test]
        public async Task GetOrderHistory_ReturnsOk()
        {
            _serviceMock.Setup(x => x.GetOrderHistory(1)).ReturnsAsync(new List<Order>());

            var result = await _controller.GetOrderHistory();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
