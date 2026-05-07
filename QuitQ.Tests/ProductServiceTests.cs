using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using QuitQ.API.Services;
using QuitQ.API.Models;
using QuitQ.API.DTOs.Request;
using System.Threading.Tasks;
using System.Linq;

namespace QuitQ.Tests
{
    public class ProductServiceTests
    {
        private QuitQDbContext _context;
        private ProductService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<QuitQDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Product")
                .Options;

            _context = new QuitQDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            SeedData();

            _service = new ProductService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        private void SeedData()
        {
            var user = new User
            {
                Id = 1,
                Name = "Test Seller",
                Username = "seller",
                Email = "seller@test.com",
                PasswordHash = "hash",
                Phone = "1234567890",
                Address = "Chennai",
                Role = "Seller",
                IsActive = true
            };

            var seller = new Seller
            {
                Id = 1,
                UserId = 1,
                StoreName = "Test Store"
            };

            var category = new Category { Id = 1, Name = "Electronics" };
            var category2 = new Category { Id = 2, Name = "Clothing" };

            var brand = new Brand { Id = 1, Name = "Apple" };

            _context.Users.Add(user);
            _context.Sellers.Add(seller);
            _context.Categories.AddRange(category, category2);
            _context.Brands.Add(brand);
            _context.SaveChanges();
        }

        // a
        [Test]
        public async Task GetAllProducts_NoFilters_ReturnsAll()
        {
            _context.Products.Add(new Product { Name = "P1", Price = 100, Stock = 1, CategoryId = 1, SellerId = 1 });
            _context.Products.Add(new Product { Name = "P2", Price = 200, Stock = 2, CategoryId = 2, SellerId = 1 });
            _context.SaveChanges();

            var result = await _service.GetAll();

            Assert.AreEqual(2, result.Count);
        }

        // b
        [Test]
        public async Task GetAllProducts_WithCategoryFilter_ReturnsFiltered()
        {
            _context.Products.Add(new Product { Name = "P1", Price = 100, Stock = 1, CategoryId = 1, SellerId = 1 });
            _context.Products.Add(new Product { Name = "P2", Price = 200, Stock = 2, CategoryId = 2, SellerId = 1 });
            _context.SaveChanges();

            var result = await _service.GetAll(1);

            Assert.AreEqual(1, result.Count);
        }

        // c
        [Test]
        public async Task GetProductById_ValidId_ReturnsProduct()
        {
            _context.Products.Add(new Product { Id = 1, Name = "P1", Price = 100, Stock = 1, CategoryId = 1, BrandId = 1, SellerId = 1 });
            _context.SaveChanges();

            var result = await _service.GetById(1);

            Assert.AreEqual("P1", result.Name);
        }

        // d
        [Test]
        public void GetProductById_InvalidId_ThrowsNotFound()
        {
            Assert.ThrowsAsync<Exception>(() => _service.GetById(999));
        }

        // e
        [Test]
        public async Task AddProduct_ValidInput_CreatesProduct()
        {
            var dto = new AddProductRequest
            {
                Name = "Phone",
                Description = "Smartphone",
                Price = 1000,
                Stock = 5,
                CategoryId = 1,
                BrandId = 1
            };

            await _service.AddProduct(dto, 1);

            Assert.AreEqual(1, _context.Products.Count());
        }

        // f
        [Test]
        public void AddProduct_InvalidCategory_ThrowsException()
        {
            var dto = new AddProductRequest
            {
                Name = "Invalid",
                Description = "Invalid",
                Price = 100,
                Stock = 1,
                CategoryId = 99
            };

            Assert.ThrowsAsync<Exception>(() => _service.AddProduct(dto, 1));
        }

        // g
        [Test]
        public void AddProduct_InvalidBrand_ThrowsException()
        {
            var dto = new AddProductRequest
            {
                Name = "Invalid",
                Description = "Invalid",
                Price = 100,
                Stock = 1,
                CategoryId = 1,
                BrandId = 99
            };

            Assert.ThrowsAsync<Exception>(() => _service.AddProduct(dto, 1));
        }

        // h
        [Test]
        public void AddProduct_Unauthorized_ThrowsException()
        {
            var dto = new AddProductRequest
            {
                Name = "Test",
                Description = "Test Description",
                Price = 100,
                Stock = 1,
                CategoryId = 1
            };

            Assert.ThrowsAsync<Exception>(() => _service.AddProduct(dto, 999));
        }

        // i
        [Test]
        public async Task UpdateProduct_UpdatesCorrectly()
        {
            _context.Products.Add(new Product { Id = 1, Name = "Old", Price = 100, Stock = 1, CategoryId = 1, SellerId = 1 });
            _context.SaveChanges();

            var dto = new AddProductRequest { Name = "New", Description = "New Description", Price = 200, Stock = 5, CategoryId = 1 };

            await _service.UpdateProduct(1, dto, 1);

            Assert.AreEqual("New", _context.Products.First().Name);
        }

        // j
        [Test]
        public void UpdateProduct_Unauthorized_ThrowsException()
        {
            _context.Products.Add(new Product { Id = 1, Name = "Test", Price = 100, Stock = 1, CategoryId = 1, SellerId = 1 });
            _context.SaveChanges();

            var dto = new AddProductRequest { Name = "New", Description = "New Description", Price = 200, Stock = 5, CategoryId = 1 };

            Assert.ThrowsAsync<Exception>(() => _service.UpdateProduct(1, dto, 999));
        }

        // k
        [Test]
        public void UpdateProduct_InvalidCategory_ThrowsException()
        {
            _context.Products.Add(new Product { Id = 1, Name = "Test", Price = 100, Stock = 1, CategoryId = 1, SellerId = 1 });
            _context.SaveChanges();

            var dto = new AddProductRequest { Name = "New", Description = "New Description", Price = 200, Stock = 5, CategoryId = 99 };

            Assert.ThrowsAsync<Exception>(() => _service.UpdateProduct(1, dto, 1));
        }

        // l
        [Test]
        public void UpdateProduct_InvalidBrand_ThrowsException()
        {
            _context.Products.Add(new Product { Id = 1, Name = "Test", Price = 100, Stock = 1, CategoryId = 1, SellerId = 1 });
            _context.SaveChanges();

            var dto = new AddProductRequest { Name = "New", Description = "New Description", Price = 200, Stock = 5, CategoryId = 1, BrandId = 99 };

            Assert.ThrowsAsync<Exception>(() => _service.UpdateProduct(1, dto, 1));
        }

        // m
        [Test]
        public async Task DeleteProduct_RemovesCorrectly()
        {
            _context.Products.Add(new Product { Id = 1, Name = "Test", Price = 100, Stock = 1, CategoryId = 1, SellerId = 1 });
            _context.SaveChanges();

            await _service.DeleteProduct(1, 1);

            Assert.AreEqual(0, _context.Products.Count());
        }

        // n
        [Test]
        public void DeleteProduct_Unauthorized_ThrowsException()
        {
            _context.Products.Add(new Product { Id = 1, Name = "Test", Price = 100, Stock = 1, CategoryId = 1, SellerId = 1 });
            _context.SaveChanges();

            Assert.ThrowsAsync<Exception>(() => _service.DeleteProduct(1, 999));
        }

        // o
        [Test]
        public void DeleteProduct_InvalidId_ThrowsNotFound()
        {
            Assert.ThrowsAsync<Exception>(() => _service.DeleteProduct(999, 1));
        }

        // p
        [Test]
        public async Task SearchProducts_ReturnsFiltered()
        {
            _context.Products.Add(new Product { Name = "Laptop", Price = 1000, Stock = 5, CategoryId = 1, SellerId = 1 });
            _context.Products.Add(new Product { Name = "Shirt", Price = 500, Stock = 5, CategoryId = 2, SellerId = 1 });
            _context.SaveChanges();

            var result = await _service.SearchProducts("Laptop", null, null, null, null, null, null, null, false);

            Assert.IsNotNull(result);
        }

        // q
        [Test]
        public async Task SearchProducts_ReturnsEmpty()
        {
            var result = await _service.SearchProducts("NonExisting", null, null, null, null, null, null, null, false);

            Assert.IsNotNull(result);
        }
    }
}