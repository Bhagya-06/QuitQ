using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using QuitQ.API.Models;
using QuitQ.API.Services;
using QuitQ.API.DTOs.Request;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace QuitQ.Tests.NUnit
{
    public class CartServiceTests
    {
        private QuitQDbContext _context;
        private CartService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<QuitQDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new QuitQDbContext(options);
            _service = new CartService(_context);

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
                Name = "iPhone",
                Price = 1000,
                Stock = 10
            });

            _context.SaveChanges();
        }

        [Test]
        public async Task AddToCart_Valid_AddsItem()
        {
            await _service.AddToCart(1, new AddToCartRequest
            {
                ProductId = 1,
                Quantity = 2
            });

            Assert.AreEqual(1, _context.ShoppingCarts.Count());
        }

        [Test]
        public void AddToCart_InvalidProduct_ThrowsException()
        {
            Assert.ThrowsAsync<Exception>(() =>
                _service.AddToCart(1, new AddToCartRequest
                {
                    ProductId = 999,
                    Quantity = 1
                }));
        }

        [Test]
        public async Task AddToCart_Existing_UpdatesQuantity()
        {
            await _service.AddToCart(1, new AddToCartRequest { ProductId = 1, Quantity = 1 });
            await _service.AddToCart(1, new AddToCartRequest { ProductId = 1, Quantity = 2 });

            var item = _context.ShoppingCarts.First();
            Assert.AreEqual(3, item.Quantity);
        }

        [Test]
        public async Task GetCart_ReturnsItems()
        {
            await _service.AddToCart(1, new AddToCartRequest { ProductId = 1, Quantity = 2 });

            var result = await _service.GetCart(1);

            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task RemoveFromCart_RemovesItem()
        {
            await _service.AddToCart(1, new AddToCartRequest { ProductId = 1, Quantity = 2 });

            await _service.RemoveFromCart(1, 1);

            Assert.AreEqual(0, _context.ShoppingCarts.Count());
        }

        [Test]
        public void RemoveFromCart_Invalid_Throws()
        {
            Assert.ThrowsAsync<Exception>(() => _service.RemoveFromCart(1, 999));
        }

        [Test]
        public async Task ClearCart_RemovesAll()
        {
            await _service.AddToCart(1, new AddToCartRequest { ProductId = 1, Quantity = 2 });

            await _service.ClearCart(1);

            Assert.AreEqual(0, _context.ShoppingCarts.Count());
        }

        [Test]
        public async Task UpdateQuantity_UpdatesItemQuantity()
        {
            await _service.AddToCart(1, new AddToCartRequest { ProductId = 1, Quantity = 1 });
            await _service.UpdateQuantity(1, 3, 1);

            Assert.AreEqual(3, _context.ShoppingCarts.First().Quantity);
        }

        [Test]
        public void UpdateQuantity_OutOfStockProduct_ThrowsException()
        {
            Assert.ThrowsAsync<Exception>(() =>
                _service.UpdateQuantity(1, 999, 1));
        }

        [Test]
        public async Task GetCartTotal_CalculatesCorrectly()
        {
            await _service.AddToCart(1,new AddToCartRequest { ProductId = 1, Quantity = 2 });
            var total = await _service.GetCartTotal(1);

            Assert.AreEqual(2000, total);
        }
    }
}