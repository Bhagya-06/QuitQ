using global::Moq;
using Microsoft.AspNetCore.Mvc;
using QuitQ.API.Controllers;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;
using QuitQ.API.Services.Interfaces;

namespace QuitQ.Tests.Moq
{


    public class BrandControllerTests
    {
        private Mock<IBrandService> _serviceMock;
        private BrandController _controller;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IBrandService>();
            _controller = new BrandController(_serviceMock.Object);
        }

        [Test]
        public async Task Get_ReturnsOk()
        {
            _serviceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<BrandResponse>());

            var result = await _controller.Get();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
