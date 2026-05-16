using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;

public interface IAdminService
{
    Task<List<User>> GetAllUsers();
    Task<object> GetAllSellers();
    Task DeleteUser(int userId);
    Task DeleteSeller(int sellerId);
    Task VerifySeller(int sellerId, int adminId, string status);
    Task<SalesReportResponse> GetSalesReport();
}