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
}