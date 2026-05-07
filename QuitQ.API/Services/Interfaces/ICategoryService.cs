using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;

public interface ICategoryService
{
    Task<List<CategoryResponse>> GetAllCategories();
    Task AddCategory(string name, string description);
    Task UpdateCategory(int id, string? name, string? description);
    Task DeleteCategory(int id);
}