using QuitQ.API.DTOs.Request;
using QuitQ.API.DTOs.Response;

namespace QuitQ.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterRequest dto);
        Task<string> LoginAsync(LoginRequest dto);
        Task<UserProfileResponse> GetUserProfile(int userId);
        Task UpdateProfile(int userId, UpdateProfileRequest dto);
        Task<object> GetSalesReport(int sellerId);
    }
}