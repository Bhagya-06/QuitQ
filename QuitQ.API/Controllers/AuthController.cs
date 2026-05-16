using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuitQ.API.DTOs.Request;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;
using QuitQ.API.Services.Interfaces;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _service;
    private readonly QuitQDbContext _context;

    public AuthController(IUserService service, QuitQDbContext context)
    {
        _service = service;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest dto)
    {
        var result = await _service.RegisterAsync(dto);
        return Ok(ApiResponse<object>.Success(result, "User registered"));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest dto)
    {
        var token = await _service.LoginAsync(dto);
        return Ok(ApiResponse<object>.Success(new { token }, "Login successful"));
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var result = await _service.GetUserProfile(userId);

        return Ok(ApiResponse<object>.Success(result, "User profile retrieved successfully"));
    }

    [Authorize]
    [HttpPut("updateprofile")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequest dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await _service.UpdateProfile(userId, dto);

        return Ok(ApiResponse<string>.Success(null, "Profile updated"));
    }

    [Authorize(Roles = "Seller")]
    [HttpGet("sales-report")]
    public async Task<IActionResult> GetReport()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var seller = await _context.Sellers
            .FirstOrDefaultAsync(s => s.UserId == userId);

        var report = await _service.GetSalesReport(seller.Id);

        return Ok(ApiResponse<object>.Success(report, "Report generated"));
    }

    [HttpGet("address")]
    [Authorize]
    public async Task<IActionResult> GetAddresses()
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        );

        var addresses = await _service.GetAddresses(userId);

        return Ok(
            ApiResponse<object>.Success(
                addresses,
                "Addresses retrieved successfully"
            )
        );
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin(
    GoogleLoginRequest request)
    {
        try
        {
            var token =
                await _service.GoogleLogin(
                    request.IdToken
                );

            return Ok(
                ApiResponse<object>.Success(
                    new { token },
                    "Google login successful"
                )
            );
        }
        catch (Exception ex)
        {
            return BadRequest(
                ApiResponse<object>.Fail(
                    new List<string>
                    {
                    ex.Message,
                    ex.InnerException?.Message
                    }
                )
            );
        }
    }

    [Authorize(Roles = "Seller")]
    [HttpGet("seller-products")]
    public async Task<IActionResult> GetSellerProducts()
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );

        var products = await _service
            .GetSellerProducts(userId);

        return Ok(
            ApiResponse<object>.Success(
                products,
                "Seller products retrieved"
            )
        );
    }

    [Authorize(Roles = "Seller")]
    [HttpGet("seller-orders")]
    public async Task<IActionResult> GetSellerOrders()
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );

        var orders = await _service
            .GetSellerOrders(userId);

        return Ok(
            ApiResponse<object>.Success(
                orders,
                "Seller orders retrieved"
            )
        );
    }

    [Authorize(Roles = "Seller")]
    [HttpPut("order-status/{id}")]
    public async Task<IActionResult> UpdateOrderStatus(
    int id,
    [FromQuery] string status
)
    {
        await _service.UpdateOrderStatus(id, status);

        return Ok(
            ApiResponse<object>.Success(
                null,
                "Order status updated"
            )
        );
    }
}