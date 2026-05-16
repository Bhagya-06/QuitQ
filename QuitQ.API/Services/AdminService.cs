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
            .Where(u => u.Role == "Buyer" || u.Role == "Seller")
            .ToListAsync();
    }

    public async Task<object> GetAllSellers()
    {
        return await _context.Sellers
            .Include(s => s.User)
            .Select(s => new
            {
                Id = s.Id,
                Name = s.User.Name,
                Email = s.User.Email,
                Phone = s.User.Phone,
                StoreName = s.StoreName,
                City = s.City,
                Country = s.Country,
                Gstin = s.Gstin,
                VerificationStatus = s.VerificationStatus,
                VerificationDate = s.VerificationDate,
                IsActive = s.User.IsActive,
                ShopReviews = s.ShopReviews,
                IdProofDocument = s.IdProofDocument,
                IdProofNumber = s.IdProofNumber,
                BusinessLicense = s.BusinessLicense
            })
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
        var seller = await _context.Sellers
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == sellerId);

        if (seller == null)
            throw new Exception("Seller not found");

        seller.User.IsActive = false;

        await _context.SaveChangesAsync();
    }

    public async Task VerifySeller(int sellerId, int adminId, string status)
    {
        var seller = await _context.Sellers
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == sellerId);

        if (seller == null)
            throw new Exception("Seller not found");

        var validStatuses = new[]{"Pending","Verified","Rejected"};

        if (!validStatuses.Contains(status))
        {
            throw new Exception("Invalid status");
        }

        seller.VerificationStatus = status;
        seller.VerificationDate = DateTime.UtcNow;
        seller.AdminApprovedBy = adminId;

        if (status == "Rejected")
            seller.User.IsActive = false;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.InnerException?.Message);
        }
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