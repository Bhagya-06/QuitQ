using Microsoft.EntityFrameworkCore;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;

public class CategoryService : ICategoryService
{
    private readonly QuitQDbContext _context;

    public CategoryService(QuitQDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryResponse>> GetAllCategories()
    {
        return await _context.Categories
            .Include(c => c.Products)
            .Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Products = c.Products.Select(p => new ProductMiniDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task AddCategory(string name, string description)
    {
        if (await _context.Categories.AnyAsync(c => c.Name == name))
            throw new Exception("Category already exists");

        var category = new Category
        {
            Name = name,
            Description = description
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategory(int id, string? name, string? description)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            throw new Exception("Category not found");

        if (await _context.Categories.AnyAsync(c => c.Name == name && c.Id != id))
            throw new Exception("Category name already exists");

        if (name != null)
            category.Name = name;

        if (description != null)
            category.Description = description;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategory(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            throw new Exception("Category not found");

        if (category.Products.Any())
            throw new Exception("Cannot delete category with existing products");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
}