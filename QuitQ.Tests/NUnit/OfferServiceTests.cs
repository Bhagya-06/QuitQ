using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using QuitQ.API.Models;
using QuitQ.API.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuitQ.Tests.NUnit
{
    [TestFixture]
    public class OfferServiceTests
    {
        private QuitQDbContext _context;
        private OfferService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<QuitQDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new QuitQDbContext(options);
            _service = new OfferService(_context);

            SeedData();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        private void SeedData()
        {
            _context.Products.Add(new Product
            {
                Id = 1,
                Name = "Test Product", 
                Price = 100,
                Stock = 10  
            });

            _context.SaveChanges();
        }

        [Test]
        public async Task CreateOffer_ValidInput_CreatesOffer()
        {
            var offer = new Offer { DiscountPercentage = 10, IsActive = true };

            await _service.CreateOffer(offer);

            Assert.AreEqual(1, _context.Offers.Count());
        }

        [Test]
        public void CreateOffer_InvalidDiscount_ThrowsException()
        {
            Assert.ThrowsAsync<Exception>(() =>
                _service.CreateOffer(new Offer { DiscountPercentage = 200 }));
        }

        [Test]
        public async Task GetAllOffers_ReturnsAll()
        {
            _context.Offers.Add(new Offer { DiscountPercentage = 10 });
            _context.SaveChanges();

            var result = await _service.GetAllOffers();

            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task GetOfferById_ValidId_ReturnsOffer()
        {
            var offer = new Offer { Id = 1, DiscountPercentage = 10 };
            _context.Offers.Add(offer);
            _context.SaveChanges();

            var result = await _service.GetOfferById(1);

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetOfferById_InvalidId_ThrowsNotFound()
        {
            Assert.ThrowsAsync<Exception>(() => _service.GetOfferById(999));
        }

        [Test]
        public async Task UpdateOffer_ValidInput_UpdatesSuccessfully()
        {
            _context.Offers.Add(new Offer { Id = 1, DiscountPercentage = 10 });
            _context.SaveChanges();

            await _service.UpdateOffer(1, new Offer { DiscountPercentage = 20 });

            Assert.AreEqual(20, _context.Offers.First().DiscountPercentage);
        }

        [Test]
        public void UpdateOffer_InvalidId_ThrowsNotFound()
        {
            Assert.ThrowsAsync<Exception>(() =>
                _service.UpdateOffer(999, new Offer()));
        }

        [Test]
        public async Task DeleteOffer_RemovesSuccessfully()
        {
            _context.Offers.Add(new Offer { Id = 1 });
            _context.SaveChanges();

            await _service.DeleteOffer(1);

            Assert.AreEqual(0, _context.Offers.Count());
        }

        [Test]
        public void DeleteOffer_InvalidId_ThrowsNotFound()
        {
            Assert.ThrowsAsync<Exception>(() => _service.DeleteOffer(999));
        }

        [Test]
        public async Task ApplyOfferToProduct_ValidOffer_AppliesDiscount()
        {
            _context.Offers.Add(new Offer
            {
                DiscountPercentage = 10,
                IsActive = true,
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow.AddDays(1)
            });

            _context.SaveChanges();

            var price = await _service.ApplyOfferToProduct(1, 100);

            Assert.AreEqual(90, price);
        }

        [Test]
        public async Task ApplyOfferToProduct_ExpiredOffer_DoesNotApply()
        {
            _context.Offers.Add(new Offer
            {
                DiscountPercentage = 10,
                IsActive = true,
                StartDate = DateTime.UtcNow.AddDays(-10),
                EndDate = DateTime.UtcNow.AddDays(-1)
            });

            _context.SaveChanges();

            var price = await _service.ApplyOfferToProduct(1, 100);

            Assert.AreEqual(100, price);
        }

        [Test]
        public void ValidateOffer_ActiveOffer_ReturnsTrue()
        {
            var offer = new Offer
            {
                IsActive = true,
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow.AddDays(1)
            };

            Assert.IsTrue(_service.ValidateOffer(offer));
        }

        [Test]
        public void ValidateOffer_ExpiredOffer_ReturnsFalse()
        {
            var offer = new Offer
            {
                IsActive = true,
                StartDate = DateTime.UtcNow.AddDays(-10),
                EndDate = DateTime.UtcNow.AddDays(-1)
            };

            Assert.IsFalse(_service.ValidateOffer(offer));
        }
    }
}
