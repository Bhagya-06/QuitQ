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
    public class OrderServiceTests
    {
        private QuitQDbContext _context;
        private OrderService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<QuitQDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new QuitQDbContext(options);
            _service = new OrderService(_context);

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

            _context.ShoppingCarts.Add(new ShoppingCart
            {
                Id = 1,
                UserId = 1,
                ProductId = 1,
                Quantity = 2
            });

            _context.Addresses.Add(new Address
            {
                Id = 1,
                UserId = 1,
                AddressName = "Home",
                Address1 = "Test Address",
                City = "Chennai",
                Pincode = "600001",
                IsDefault = true
            });

            _context.SaveChanges();
        }

        [Test]
        public async Task PlaceOrder_ValidCartAndAddress_CreatesOrder()
        {
            await _service.PlaceOrder(new PlaceOrderRequest { AddressId = 1, PaymentMethod = (PaymentMethod)3 }, 1);

            Assert.AreEqual(1, _context.Orders.Count());
        }

        [Test]
        public void PlaceOrder_EmptyCart_ThrowsException()
        {
            Assert.ThrowsAsync<Exception>(() =>
                _service.PlaceOrder(new PlaceOrderRequest { AddressId = 1, PaymentMethod = (PaymentMethod)3 }, 999));
        }

        [Test]
        public void PlaceOrder_AddressNotOwnedByUser_ThrowsException()
        {
            Assert.ThrowsAsync<Exception>(() =>
                _service.PlaceOrder(new PlaceOrderRequest { AddressId = 999, PaymentMethod = (PaymentMethod)3 }, 1));
        }

        [Test]
        public async Task PlaceDirectProductOrder_ValidInput_CreatesOrder()
        {
            await _service.PlaceDirectProductOrder(1, 1, 1, 1);
            Assert.AreEqual(1, _context.Orders.Count());
        }

        [Test]
        public void PlaceDirectProductOrder_AddressNotOwnedByUser_ThrowsException()
        {
            Assert.ThrowsAsync<Exception>(() =>
                _service.PlaceDirectProductOrder(1, 1, 999, 1));
        }

        [Test]
        public async Task GetOrderHistory_ReturnsAllUserOrders()
        {
            await _service.PlaceDirectProductOrder(1, 1, 1, 1);

            var result = await _service.GetOrderHistory(1);

            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task GetOrderHistory_OtherUserOrders_NotReturned()
        {
            await _service.PlaceDirectProductOrder(1, 1, 1, 1);

            var result = await _service.GetOrderHistory(999);

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public async Task GetOrderById_ValidUser_ReturnsOrder()
        {
            await _service.PlaceDirectProductOrder(1, 1, 1, 1);
            var order = _context.Orders.First();

            var result = await _service.GetOrderById(order.Id, 1);

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetOrderById_OtherUser_ThrowsUnauthorized()
        {
            Assert.ThrowsAsync<Exception>(() =>
                _service.GetOrderById(1, 999));
        }

    }
}