using log4net;
using Microsoft.EntityFrameworkCore;
using QuitQ.API.DTOs.Request;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;
using QuitQ.API.Services.Interfaces;

namespace QuitQ.API.Services
{
    public class ProductService : IProductService
    {
        private readonly QuitQDbContext _context;
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductService));
        public ProductService(QuitQDbContext context)
        {
            _context = context;
        }

        public async Task AddProduct(AddProductRequest dto, int userId)
        {
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (seller == null)
            {
                log.Error($"Seller with User ID {userId} not found when adding product");
                throw new Exception("Unauthorized");
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == dto.CategoryId);

            if (category == null) {
                log.Error($"Category with ID {dto.CategoryId} not found when adding product for user {userId}"); ;
                throw new Exception("Invalid Category");
            }

            Brand? brand = null;

            if (dto.BrandId.HasValue)
            {
                brand = await _context.Brands
                    .FirstOrDefaultAsync(b => b.Id == dto.BrandId);

                if (brand == null)
                {
                    log.Error($"Brand with ID {dto.BrandId} not found when adding product for user {userId}");
                    throw new Exception("Invalid Brand");
                }
            }

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                ImageUrl = dto.ImageUrl,
                SellerId = seller.Id,
                CategoryId = category.Id,
                BrandId = brand?.Id,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();
            log.Info($"Product '{product.Name}' added successfully by user {userId}");
        }

        public async Task<List<ProductResponse>> GetAll(int? categoryId = null)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Seller)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId);

            return await query.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category.Name,
                BrandName = p.Brand != null ? p.Brand.Name : null,
                StoreName = p.Seller.StoreName,

                AverageRating = p.ProductReviews.Any()? p.ProductReviews.Average(r => r.Rating): 0,

                ReviewCount = p.ProductReviews.Count()
            }).ToListAsync();
        }

        public async Task<ProductResponse> GetById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                throw new Exception("Product not found");

            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                CategoryName = product.Category.Name,
                BrandName = product.Brand.Name,
                StoreName = product.Seller.StoreName,

                AverageRating = product.ProductReviews.Any()? product.ProductReviews.Average(r => r.Rating): 0,

                ReviewCount = product.ProductReviews.Count()
            };
        }

        public async Task<bool> UpdateProduct(int productId, AddProductRequest dto, int userId)
        {
            var product = await _context.Products
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                throw new Exception("Product not found");

            if (product.Seller.UserId != userId)
                throw new Exception("Unauthorized");

            if (!await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId))
                throw new Exception("Invalid Category");

            if (dto.BrandId.HasValue &&
                !await _context.Brands.AnyAsync(b => b.Id == dto.BrandId))
                throw new Exception("Invalid Brand");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.ImageUrl = dto.ImageUrl;
            product.CategoryId = dto.CategoryId;
            product.BrandId = dto.BrandId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProduct(int productId, int userId)
        {
            var product = await _context.Products
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                throw new Exception("Product not found");

            if (product.Seller.UserId != userId)
                throw new Exception("Unauthorized");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object> SearchProducts(string? name,string? storeName,string? categoryName,string? brandName,decimal? minPrice,
            decimal? maxPrice,int? minStock,string? sortBy,bool isDescending,int page = 1,int pageSize = 10)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Seller)
                .AsQueryable();


            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.ToLower().Contains(name.ToLower()));
            }

            if (!string.IsNullOrEmpty(storeName))
            {
                query = query.Where(p =>
                    p.Seller.StoreName.ToLower().Contains(storeName.ToLower()));
            }

            if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(p =>
                    p.Category.Name.ToLower().Contains(categoryName.ToLower()));
            }

            if (!string.IsNullOrEmpty(brandName))
            {
                query = query.Where(p => p.Brand != null && p.Brand.Name.ToLower().Contains(brandName.ToLower()));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (minStock.HasValue)
            {
                query = query.Where(p => p.Stock >= minStock.Value);
            }


            query = sortBy?.ToLower() switch
            {
                "newest" => query.OrderByDescending(p => p.CreatedDate),

                "price_low_high" => query.OrderBy(p => p.Price),

                "price_high_low" => query.OrderByDescending(p => p.Price),

                "rating" => query.OrderByDescending(p =>
                    p.ProductReviews.Any()
                        ? p.ProductReviews.Average(r => r.Rating)
                        : 0),

                "popular" => query.OrderByDescending(p =>
                    p.OrderItems.Count()),

                "relevant" => query.OrderByDescending(p =>
                    p.Name.Contains(name ?? "")),

                _ => query.OrderByDescending(p => p.CreatedDate)
            };

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    ImageUrl = p.ImageUrl,

                    CategoryName = p.Category.Name,

                    BrandName = p.Brand != null? p.Brand.Name: null,

                    StoreName = p.Seller.StoreName,

                    AverageRating = p.ProductReviews.Any()? p.ProductReviews.Average(r => r.Rating): 0,

                    ReviewCount = p.ProductReviews.Count()
                })
                .ToListAsync();

            return new
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Data = products
            };
        }
    }
}