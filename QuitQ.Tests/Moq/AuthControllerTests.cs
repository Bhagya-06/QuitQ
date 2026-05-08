using global::Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuitQ.API.Models;
using QuitQ.API.Services.Interfaces;
using QuitQ.API.DTOs.Request;

namespace QuitQ.Tests.Moq
{
    public class AuthControllerTests
    {
        private Mock<IUserService> _serviceMock;
        private QuitQDbContext _context;
        private AuthController _controller;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IUserService>();

            var options = new DbContextOptionsBuilder<QuitQDbContext>()
                .UseInMemoryDatabase(databaseName: "AuthDb")
                .Options;

            _context = new QuitQDbContext(options);

            _controller = new AuthController(_serviceMock.Object, _context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Login_ReturnsOk()
        {
            var dto = new LoginRequest
            {
                Email = "test@gmail.com",
                Password = "123"
            };

            _serviceMock
                .Setup(x => x.LoginAsync(dto))
                .ReturnsAsync("fake-jwt-token");

            var result = await _controller.Login(dto);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
