using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuitQ.API.DTOs.Request;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;
using QuitQ.API.Services.Interfaces;
using System.Security.Claims;

namespace QuitQ.API.Controllers
{
    [ApiController]
    [Route("api/review")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewController(IReviewService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("product")]
        public async Task<IActionResult> AddProduct(AddReviewRequest dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _service.AddProductReview(userId, dto);

            return Ok(ApiResponse<string>.Success(null, "Review added"));
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetProduct(int productId)
            => Ok(ApiResponse<object>.Success(await _service.GetProductReviews(productId), "Product reviews retrieved"));

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, int rating, string comment)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _service.UpdateReview(id, userId, rating, comment);

            return Ok(ApiResponse<string>.Success(null, "Review updated"));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _service.DeleteReview(id, userId);

            return Ok(ApiResponse<string>.Success(null, "Review deleted"));
        }

    }
}
