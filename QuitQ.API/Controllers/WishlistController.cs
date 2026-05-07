using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Services.Interfaces;
using System.Security.Claims;

namespace QuitQ.API.Controllers
{
    [ApiController]
    [Route("api/wishlist")]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _service;

        public WishlistController(IWishlistService service)
        {
            _service = service;
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)
                     ?? User.FindFirst("id");

            if (claim == null)
                throw new UnauthorizedAccessException("Invalid token");

            return int.Parse(claim.Value);
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> Add(int productId)
        {
            await _service.AddToWishlist(productId, GetUserId());

            return Ok(ApiResponse<string>.Success(null, "Added to wishlist"));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _service.GetWishlist(GetUserId());

            return Ok(ApiResponse<object>.Success(data, "Wishlist retrieved"));
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Remove(int productId)
        {
            await _service.RemoveFromWishlist(productId, GetUserId());

            return Ok(ApiResponse<string>.Success(null, "Removed from wishlist"));
        }

        [HttpGet("check/{productId}")]
        public async Task<IActionResult> Check(int productId)
        {
            var exists = await _service.IsProductInWishlist(productId, GetUserId());

            return Ok(ApiResponse<bool>.Success(exists, "Wishlist check completed"));
        }
    }
}