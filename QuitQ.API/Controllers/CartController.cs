using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuitQ.API.DTOs.Request;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Services.Interfaces;
using System.Security.Claims;

namespace QuitQ.API.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(AddToCartRequest dto)
        {
            await _service.AddToCart(GetUserId(), dto);
            return Ok(ApiResponse<string>.Success(null, "Added to cart"));
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var cart = await _service.GetCart(GetUserId());
            return Ok(ApiResponse<object>.Success(cart, "Cart retrieved successfully"));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            await _service.UpdateQuantity(productId, quantity, GetUserId());
            return Ok(ApiResponse<string>.Success(null, "Cart updated"));
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Remove(int productId)
        {
            await _service.RemoveFromCart(GetUserId(), productId);
            return Ok(ApiResponse<string>.Success(null, "Removed from cart"));
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            await _service.ClearCart(GetUserId());
            return Ok(ApiResponse<string>.Success(null, "Cart cleared"));
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetCartTotal()
        {
            var userId = GetUserId();
            var total = await _service.GetCartTotal(userId);

            return Ok(ApiResponse<object>.Success(new { total }, "Cart total retrieved successfully"));
        }
    }
}