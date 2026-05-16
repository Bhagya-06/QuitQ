using Microsoft.AspNetCore.Mvc;
using QuitQ.API.DTOs.Request;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;

namespace QuitQ.API.Services.Interfaces
{
    public interface IReviewService
    {
        Task AddProductReview(int userId, AddReviewRequest dto);
        Task<List<ProductReviewResponse>> GetProductReviews(int productId);
        Task UpdateReview(int id, int userId, int rating, string comment);
        Task DeleteReview(int id, int userId);
        Task<double> GetAverageProductRating(int productId);
        Task<bool> HasPurchasedProduct(int userId, int productId);
    }
}
