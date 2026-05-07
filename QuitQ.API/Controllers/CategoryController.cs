using Microsoft.AspNetCore.Mvc;
using QuitQ.API.DTOs.Response;

[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _service.GetAllCategories();
        return Ok(ApiResponse<object>.Success(result, "Categories retrieved successfully"));
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(string name, string description)
    {
        await _service.AddCategory(name, description);
        return Ok(ApiResponse<string>.Success(null, "Category created"));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, string? name, string? description)
    {
        await _service.UpdateCategory(id, name, description);
        return Ok(ApiResponse<string>.Success(null, "Category updated"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _service.DeleteCategory(id);
        return Ok(ApiResponse<string>.Success(null, "Category deleted"));
    }
}