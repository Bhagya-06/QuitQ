using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using QuitQ.API.DTOs.Request;
using QuitQ.API.Models;
using QuitQ.API.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuitQ.Tests.NUnit
{
    [TestFixture]
    public class ReviewServiceTests
    {
        private QuitQDbContext _context;
        private ReviewService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<QuitQDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new QuitQDbContext(options);
            _service = new ReviewService(_context);

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

            _context.Users.Add(new User
            {
                Id = 1,
                Name = "Test User",
                Email = "test@mail.com",
                Username = "testuser",
                PasswordHash = "hashedpassword",
                Phone = "1234567890",
                Address = "Chennai",
                Role = "User"
            });

            _context.SaveChanges();
        }

        [Test]
        public async Task AddProductReview_ValidInput_AddsReview()
        {
            var review = new AddReviewRequest { ProductId = 1, Comment = "Great product", Rating = 5 };

            await _service.AddProductReview(1, review);

            Assert.AreEqual(1, _context.ProductReviews.Count());
        }

        [Test]
        public void AddProductReview_InvalidProduct_ThrowsException()
        {
            var review = new AddReviewRequest { ProductId = 999, Comment = "Invalid product", Rating = 5 };

            Assert.ThrowsAsync<Exception>(() => _service.AddProductReview(1, review));
        }

        [Test]
        public void AddProductReview_AlreadyReviewed_ThrowsException()
        {
            _context.ProductReviews.Add(new ProductReview { ProductId = 1, UserId = 1 });
            _context.SaveChanges();

            var review = new AddReviewRequest { ProductId = 1, Comment = "Already reviewed", Rating = 5 };

            Assert.ThrowsAsync<Exception>(() => _service.AddProductReview(1, review));
        }

        [Test]
        public async Task GetProductReviews_ReturnsAllReviews()
        {
            _context.ProductReviews.Add(new ProductReview { ProductId = 1, UserId = 1 });
            _context.SaveChanges();

            var result = await _service.GetProductReviews(1);

            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task GetProductReviews_NoReviews_ReturnsEmpty()
        {
            var result = await _service.GetProductReviews(1);

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public async Task UpdateReview_OwnerUpdatesSuccessfully()
        {
            _context.ProductReviews.Add(new ProductReview { Id = 1, ProductId = 1, UserId = 1, Rating = 3 });
            _context.SaveChanges();

            await _service.UpdateReview(1, 1, 5, "Updated");

            var review = _context.ProductReviews.First();
            Assert.AreEqual(5, review.Rating);
        }

        [Test]
        public void UpdateReview_NotOwner_ThrowsUnauthorized()
        {
            _context.ProductReviews.Add(new ProductReview { Id = 1, ProductId = 1, UserId = 1 });
            _context.SaveChanges();

            Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.UpdateReview(1, 2, 5, "Test"));
        }

        [Test]
        public async Task DeleteReview_OwnerDeletesSuccessfully()
        {
            _context.ProductReviews.Add(new ProductReview { Id = 1, ProductId = 1, UserId = 1 });
            _context.SaveChanges();

            await _service.DeleteReview(1, 1);

            Assert.AreEqual(0, _context.ProductReviews.Count());
        }

        [Test]
        public void DeleteReview_NotOwner_ThrowsUnauthorized()
        {
            _context.ProductReviews.Add(new ProductReview { Id = 1, ProductId = 1, UserId = 1 });
            _context.SaveChanges();

            Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.DeleteReview(1, 2));
        }

        [Test]
        public async Task GetAverageProductRating_CalculatesCorrectly()
        {
            _context.ProductReviews.Add(new ProductReview { ProductId = 1, Rating = 4 });
            _context.ProductReviews.Add(new ProductReview { ProductId = 1, Rating = 2 });
            _context.SaveChanges();

            var avg = await _service.GetAverageProductRating(1);

            Assert.AreEqual(3, avg);
        }

    }
}
