using Microsoft.EntityFrameworkCore;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;

public class AdminService : IAdminService
{
    private readonly QuitQDbContext _context;

    public AdminService(QuitQDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users
            .Where(u => u.Role == "User")
            .ToListAsync();
    }

    public async Task<List<User>> GetAllSellers()
    {
        return await _context.Users
            .Where(u => u.Role == "Seller")
            .ToListAsync();
    }

    public async Task DeleteUser(int userId)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
            throw new Exception("User not found");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSeller(int sellerId)
    {
        var seller = await _context.Users.FindAsync(sellerId);

        if (seller == null)
            throw new Exception("Seller not found");

        seller.IsActive = false;

        await _context.SaveChangesAsync();
    }

    public async Task VerifySeller(int sellerId, int adminId, string status)
    {
        var seller = await _context.Sellers
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == sellerId);

        if (seller == null)
            throw new Exception("Seller not found");

        if (status != "Approved" && status != "Rejected")
            throw new Exception("Invalid status");

        seller.VerificationStatus = status;
        seller.VerificationDate = DateTime.UtcNow;
        seller.AdminApprovedBy = adminId;

        if (status == "Rejected")
            seller.User.IsActive = false;

        await _context.SaveChangesAsync();
    }

    public async Task<SalesReportResponse> GetSalesReport()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ToListAsync();

        return new SalesReportResponse
        {
            TotalOrders = orders.Count,
            TotalRevenue = orders.Sum(o => o.Total),
            TotalProductsSold = orders
                .SelectMany(o => o.OrderItems)
                .Sum(oi => oi.Quantity)
        };
    }
}