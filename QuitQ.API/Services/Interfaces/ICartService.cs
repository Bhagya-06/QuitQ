using QuitQ.API.DTOs.Request;
using QuitQ.API.DTOs.Response;

namespace QuitQ.API.Services.Interfaces
{
    public interface ICartService
    {
        Task AddToCart(int userId, AddToCartRequest dto);
        Task<List<CartResponse>> GetCart(int userId);
        Task RemoveFromCart(int userId, int productId);
        Task ClearCart(int userId);
        Task UpdateQuantity(int productId, int quantity, int userId);
        Task<decimal> GetCartTotal(int userId);
    }
}
