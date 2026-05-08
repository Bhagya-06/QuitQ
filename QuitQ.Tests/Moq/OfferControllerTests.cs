using global::Moq;
using Microsoft.AspNetCore.Mvc;
using QuitQ.API.Controllers;
using QuitQ.API.Models;
using QuitQ.API.Services.Interfaces;


namespace QuitQ.Tests.Moq
{
    public class OfferControllerTests
    {
        private Mock<IOfferService> _serviceMock;
        private OfferController _controller;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IOfferService>();
            _controller = new OfferController(_serviceMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOk()
        {
            _serviceMock.Setup(x => x.GetAllOffers()).ReturnsAsync(new List<Offer>());

            var result = await _controller.GetAll();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
