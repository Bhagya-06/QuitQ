using QuitQ.API.Models;

namespace QuitQ.API.Services.Interfaces
{
    public interface IWishlistService
    {
        Task AddToWishlist(int productId, int userId);
        Task<List<Product>> GetWishlist(int userId);
        Task RemoveFromWishlist(int productId, int userId);
        Task<bool> IsProductInWishlist(int productId, int userId);
    }
}
