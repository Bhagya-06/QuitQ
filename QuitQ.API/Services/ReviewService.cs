using Microsoft.EntityFrameworkCore;
using QuitQ.API.DTOs.Request;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;
using QuitQ.API.Services.Interfaces;

namespace QuitQ.API.Services
{
    public class ReviewService : IReviewService
    {
        private readonly QuitQDbContext _context;

        public ReviewService(QuitQDbContext context)
        {
            _context = context;
        }

        public async Task AddProductReview(int userId, AddReviewRequest dto)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == dto.ProductId);

            if (product == null)
                throw new Exception("Invalid product");

            var alreadyReviewed = await _context.ProductReviews
                .AnyAsync(r => r.ProductId == dto.ProductId && r.UserId == userId);

            if (alreadyReviewed)
                throw new Exception("Already reviewed");

            var review = new ProductReview
            {
                ProductId = dto.ProductId,
                UserId = userId,
                Comment = dto.Comment,
                Rating = dto.Rating,
                ReviewImages = dto.ReviewImages,
                ReviewVideo = dto.ReviewVideo,
                CreatedDate = DateTime.UtcNow
            };

            _context.ProductReviews.Add(review);
            await _context.SaveChangesAsync();

        }

        public async Task<List<ProductReviewResponse>> GetProductReviews(int productId)
        {
            return await _context.ProductReviews
                .Where(r => r.ProductId == productId)
                .Include(r => r.User)
                .Select(r => new ProductReviewResponse
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewImages = r.ReviewImages,
                    ReviewVideo = r.ReviewVideo,
                    CreatedDate = r.CreatedDate,
                    UserName = r.User.Username
                })
                .ToListAsync();
        }

        public async Task UpdateReview(int id, int userId, int rating, string comment)
        {
            var review = await _context.ProductReviews
                .Include(r => r.Product)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
                throw new Exception("Review not found");

            if (review.UserId != userId)
                throw new UnauthorizedAccessException("You can only update your own review");

            review.Rating = rating;
            review.Comment = comment;

            await _context.SaveChangesAsync();

        }

        public async Task DeleteReview(int id, int userId)
        {
            var review = await _context.ProductReviews
                .Include(r => r.Product)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
                throw new Exception("Review not found");

            if (review.UserId != userId)
                throw new UnauthorizedAccessException("You can only delete your own review");

            _context.ProductReviews.Remove(review);
            await _context.SaveChangesAsync();

        }

        public async Task<double> GetAverageProductRating(int productId)
        {
            return await _context.ProductReviews
                .Where(x => x.ProductId == productId)
                .AverageAsync(x => (double?)x.Rating) ?? 0;
        }

    }
}
