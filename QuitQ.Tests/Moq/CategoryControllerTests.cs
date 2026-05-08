using global::Moq;
using Microsoft.AspNetCore.Mvc;
using QuitQ.API.DTOs.Response;

namespace QuitQ.Tests.Moq
{
    public class CategoryControllerTests
    {
        private Mock<ICategoryService> _serviceMock;
        private CategoryController _controller;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<ICategoryService>();
            _controller = new CategoryController(_serviceMock.Object);
        }

        [Test]
        public async Task GetAllCategories_ReturnsOk()
        {
            _serviceMock.Setup(x => x.GetAllCategories()).ReturnsAsync(new List<CategoryResponse>());

            var result = await _controller.GetAllCategories();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
