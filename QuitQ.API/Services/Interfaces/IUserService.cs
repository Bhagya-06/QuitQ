using QuitQ.API.DTOs.Request;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;

namespace QuitQ.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterRequest dto);
        Task<string> LoginAsync(LoginRequest dto);
        Task<UserProfileResponse> GetUserProfile(int userId);
        Task UpdateProfile(int userId, UpdateProfileRequest dto);
        Task<object> GetSalesReport(int sellerId);
        Task<List<Address>> GetAddresses(int userId);
        Task<string> GoogleLogin(string idToken);
        Task<object> GetSellerProducts(int userId);
        Task<object> GetSellerOrders(int userId);
        Task UpdateOrderStatus(int orderId, string status);
    }
}