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
    public class WishlistServiceTests
    {
        private QuitQDbContext _context;
        private WishlistService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<QuitQDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new QuitQDbContext(options);
            _service = new WishlistService(_context);

            SeedData();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        private void SeedData()
        {
            _context.Products.Add(new Product { Id = 1, Name = "P1" });
            _context.Products.Add(new Product { Id = 2, Name = "P2" });
            _context.SaveChanges();
        }

        [Test]
        public async Task AddToWishlist_NewProduct_AddsSuccessfully()
        {
            await _service.AddToWishlist(1, 1);

            Assert.AreEqual(1, _context.Wishlists.Count());
        }

        [Test]
        public async Task AddToWishlist_AlreadyExists_DoesNotDuplicate()
        {
            await _service.AddToWishlist(1, 1);
            await _service.AddToWishlist(1, 1);

            Assert.AreEqual(1, _context.Wishlists.Count());
        }

        [Test]
        public void AddToWishlist_InvalidProduct_ThrowsException()
        {
            Assert.ThrowsAsync<Exception>(() => _service.AddToWishlist(999, 1));
        }

        [Test]
        public async Task GetWishlist_WithItems_ReturnsProducts()
        {
            await _service.AddToWishlist(1, 1);

            var result = await _service.GetWishlist(1);

            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task GetWishlist_Empty_ReturnsEmptyList()
        {
            var result = await _service.GetWishlist(2);

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public async Task RemoveFromWishlist_ExistingItem_RemovesSuccessfully()
        {
            await _service.AddToWishlist(1, 1);

            await _service.RemoveFromWishlist(1, 1);

            Assert.AreEqual(0, _context.Wishlists.Count());
        }

        [Test]
        public void RemoveFromWishlist_InvalidProduct_ThrowsException()
        {
            Assert.ThrowsAsync<Exception>(() => _service.RemoveFromWishlist(1, 1));
        }

        [Test]
        public async Task IsProductInWishlist_ReturnsTrueIfExists()
        {
            await _service.AddToWishlist(1, 1);

            var result = await _service.IsProductInWishlist(1, 1);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsProductInWishlist_ReturnsFalseIfNotExists()
        {
            var result = await _service.IsProductInWishlist(1, 1);

            Assert.IsFalse(result);
        }
    }
}
