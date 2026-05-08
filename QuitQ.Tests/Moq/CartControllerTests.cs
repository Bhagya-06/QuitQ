using global::Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuitQ.API.Controllers;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Services.Interfaces;
using System.Security.Claims;

namespace QuitQ.Tests.Moq
{
    public class CartControllerTests
    {
        private Mock<ICartService> _serviceMock;
        private CartController _controller;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<ICartService>();

            _controller = new CartController(_serviceMock.Object);

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
        public async Task GetCart_ReturnsOk()
        {
            _serviceMock.Setup(x => x.GetCart(1)).ReturnsAsync(new List<CartResponse>());

            var result = await _controller.GetCart();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
