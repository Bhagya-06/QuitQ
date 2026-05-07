using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;
using QuitQ.API.Services.Interfaces;

namespace QuitQ.API.Services
{
    public class BrandService : IBrandService
    {
        private readonly QuitQDbContext _context;

        public BrandService(QuitQDbContext context)
        {
            _context = context;
        }

        public async Task<List<BrandResponse>> GetAll()
        {
            return await _context.Brands
                .Include(b => b.Products)
                .Select(b => new BrandResponse
                {
                    Id = b.Id,
                    Name = b.Name,
                    Products = b.Products.Select(p => new ProductMiniDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task Add([FromBody] string name)
        {
            if (await _context.Brands.AnyAsync(b => b.Name == name))
                throw new Exception("Brand already exists");

            var brand = new Brand { Name = name };

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, [FromBody] string name)
        {
            var brand = await _context.Brands.FindAsync(id);

            if (brand == null)
                throw new Exception("Brand not found");

            if (await _context.Brands.AnyAsync(b => b.Name == name && b.Id != id))
            {
                throw new Exception("Brand name already exists");
            }

            brand.Name = name;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var brand = await _context.Brands.Include(b => b.Products).FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null)
                throw new Exception("Brand not found");

            if (brand.Products.Any())
                throw new Exception("Cannot delete brand with existing products");

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
        }
    }
}
