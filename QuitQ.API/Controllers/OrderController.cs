using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuitQ.API.DTOs.Request;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Services.Interfaces;
using System.Security.Claims;

namespace QuitQ.API.Controllers
{
    [ApiController]
    [Route("api/order")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> PlaceOrder(PlaceOrderRequest request)
        {
            await _service.PlaceOrder(request, GetUserId());
            return Ok(ApiResponse<string>.Success(null, "Order placed successfully"));
        }

        [HttpPost("buy-now")]
        public async Task<IActionResult> PlaceDirectProductOrder(
            int productId, int quantity, int addressId)
        {
                await _service.PlaceDirectProductOrder(productId, quantity, addressId, GetUserId());
                return Ok(ApiResponse<string>.Success(null, "Direct order placed successfully"));
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> GetOrderHistory()
        {
            var orders = await _service.GetOrderHistory(GetUserId());
            return Ok(ApiResponse<object>.Success(orders, "Order history retrieved successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var userId = GetUserId();
                var order = await _service.GetOrderById(id, userId);

                return Ok(ApiResponse<object>.Success(order, "Products retrieved successfully"));
            }
            catch (Exception ex)
            {
                return Unauthorized(ApiResponse<string>.Fail(null,ex.Message));
            }
        }
    }
}