using Microsoft.EntityFrameworkCore;
using QuitQ.API.DTOs.Request;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;
using QuitQ.API.Services.Interfaces;

namespace QuitQ.API.Services
{
    public class CartService : ICartService
    {
        private readonly QuitQDbContext _context;

        public CartService(QuitQDbContext context)
        {
            _context = context;
        }

        public async Task AddToCart(int userId, AddToCartRequest dto)
        {
            var product = await _context.Products.FindAsync(dto.ProductId);

            if (product == null)
                throw new Exception("Product not found");

            if (product.Stock < dto.Quantity)
                throw new Exception("Insufficient stock");

            var existing = await _context.ShoppingCarts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == dto.ProductId);

            if (existing != null)
            {
                existing.Quantity += dto.Quantity;
            }
            else
            {
                var cart = new ShoppingCart
                {
                    UserId = userId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                };

                _context.ShoppingCarts.Add(cart);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<CartResponse>> GetCart(int userId)
        {
            return await _context.ShoppingCarts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .Select(c => new CartResponse
                {
                    Id = c.Id,
                    ProductId = c.ProductId,
                    ProductName = c.Product.Name,
                    UnitPrice = c.Product.Price,
                    Quantity = c.Quantity
                })
                .ToListAsync();
        }

        public async Task RemoveFromCart(int userId, int productId)
        {
            var item = await _context.ShoppingCarts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (item == null)
                throw new Exception("Item not found");

            _context.ShoppingCarts.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task ClearCart(int userId)
        {
            var items = _context.ShoppingCarts.Where(c => c.UserId == userId);

            _context.ShoppingCarts.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuantity(int productId, int quantity, int userId)
        {
            var cart = await _context.ShoppingCarts
                .FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId);

            if (cart == null)
                throw new Exception("Cart item not found");

            var product = await _context.Products.FindAsync(productId);

            if (product.Stock < quantity)
                throw new Exception("Out of stock");

            cart.Quantity = quantity;
            await _context.SaveChangesAsync();
        }
        public async Task<decimal> GetCartTotal(int userId)
        {
            return await _context.ShoppingCarts
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .SumAsync(c => c.Product.Price * c.Quantity);
        }
    }
}