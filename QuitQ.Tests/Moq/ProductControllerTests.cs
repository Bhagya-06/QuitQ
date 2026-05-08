using global::Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuitQ.API.Controllers;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;
using QuitQ.API.Services.Interfaces;


namespace QuitQ.Tests.Moq
{
    public class ProductControllerTests
    {
        private Mock<IProductService> _serviceMock;
        private QuitQDbContext _context;
        private ProductController _controller;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IProductService>();

            var options = new DbContextOptionsBuilder<QuitQDbContext>()
                .UseInMemoryDatabase("ProductDb")
                .Options;

            _context = new QuitQDbContext(options);

            _controller = new ProductController(_serviceMock.Object, _context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Get_ReturnsOk()
        {
            _serviceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<ProductResponse>());

            var result = await _controller.Get();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
