using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuitQ.API.DTOs.Response;
using System.Security.Claims;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _service;

    public AdminController(IAdminService service)
    {
        _service = service;
    }

    [HttpGet("sellers")]
    public async Task<IActionResult> GetAllSellers()
    {
        return Ok(ApiResponse<object>.Success(await _service.GetAllSellers(), "Sellers retrieved successfully"));
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var data = await _service.GetAllUsers();
        return Ok(ApiResponse<object>.Success(data, "Users retrieved successfully"));
    }

    [HttpDelete("user/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            await _service.DeleteUser(id);

            return Ok(
                ApiResponse<string>.Success(
                    null,
                    "User deleted"
                )
            );
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                ex.Message,
                Inner = ex.InnerException?.Message
            });
        }
    }

    [HttpDelete("seller/{id}")]
    public async Task<IActionResult> DeleteSeller(int id)
    {
        try {
        await _service.DeleteSeller(id);
        return Ok(ApiResponse<string>.Success(null, "Seller deactivated"));
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                ex.Message,
                Inner = ex.InnerException?.Message
            });
        }
    }

    [HttpPut("seller/{id}/verify")]
    public async Task<IActionResult> VerifySeller(int id, string status)
    {
        var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await _service.VerifySeller(id, adminId, status);

        return Ok(ApiResponse<string>.Success(null, $"Seller {status} successfully"));
    }

    [HttpGet("reports/sales")]
    public async Task<IActionResult> GetSalesReport()
    {
        var report = await _service.GetSalesReport();

        return Ok(ApiResponse<object>.Success(report, "Report generated"));
    }
}