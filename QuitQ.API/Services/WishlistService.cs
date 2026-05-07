using Microsoft.EntityFrameworkCore;
using QuitQ.API.Models;
using QuitQ.API.Services.Interfaces;

namespace QuitQ.API.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly QuitQDbContext _context;

        public WishlistService(QuitQDbContext context)
        {
            _context = context;
        }

        public async Task AddToWishlist(int productId, int userId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new Exception("Product not found");

            var exists = await _context.Wishlists
                .AnyAsync(x => x.ProductId == productId && x.UserId == userId);

            if (exists) return;

            _context.Wishlists.Add(new Wishlist
            {
                ProductId = productId,
                UserId = userId,
                CreatedDate = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetWishlist(int userId)
        {
            return await _context.Wishlists
                .Where(x => x.UserId == userId)
                .Include(x => x.Product)
                .Select(x => x.Product!)
                .ToListAsync();
        }

        public async Task RemoveFromWishlist(int productId, int userId)
        {
            var item = await _context.Wishlists
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.UserId == userId);

            if (item == null)
                throw new Exception("Wishlist item not found");

            _context.Wishlists.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsProductInWishlist(int productId, int userId)
        {
            return await _context.Wishlists
                .AnyAsync(x => x.ProductId == productId && x.UserId == userId);
        }
    }
}
