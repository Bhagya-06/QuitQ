using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuitQ.API.DTOs.Request;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;
using QuitQ.API.Services.Interfaces;
using System.Security.Claims;

namespace QuitQ.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly QuitQDbContext _context;

        public ProductController(IProductService service, QuitQDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Add(AddProductRequest dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _service.AddProduct(dto, userId);

            return Ok(ApiResponse<string>.Success(null, "Product added successfully"));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(ApiResponse<object>.Success(await _service.GetAll(), "Products retrieved successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetById(id);
            return Ok(ApiResponse<object>.Success(product, "Product retrieved successfully"));
        }

        [Authorize(Roles = "Seller")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AddProductRequest dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _service.UpdateProduct(id, dto, userId);

            if (!result)
                return Unauthorized(ApiResponse<string>.Fail(null,"You are not allowed to update this product"));

            return Ok(ApiResponse<string>.Success(null, "Product updated successfully"));
        }

        [Authorize(Roles = "Seller")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _service.DeleteProduct(id, userId);

            if (!result)
                return Unauthorized(ApiResponse<string>.Fail(null,"You are not allowed to delete this product"));

            return Ok(ApiResponse<string>.Success(null, "Product deleted successfully"));
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string? name,string? storeName,string? categoryName,string? brandName,
            decimal? minPrice,decimal? maxPrice,int? minStock,string? sortBy,bool isDescending = false,int page = 1,
            int pageSize = 10)
        {
            var result = await _service.SearchProducts(
                name, storeName, categoryName, brandName,
                minPrice, maxPrice, minStock,
                sortBy, isDescending, page, pageSize);

            return Ok(ApiResponse<object>.Success(result, "Products retrieved successfully"));
        }
    }
}