using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuitQ.API.DTOs.Request;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;
using QuitQ.API.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuitQ.API.Services
{
    public class UserService : IUserService
    {
        private readonly QuitQDbContext _context;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public UserService(QuitQDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string> RegisterAsync(RegisterRequest dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email))
                throw new Exception("Email already exists");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Username = dto.Username,
                Phone = dto.Phone,
                Address = dto.Address,
                Role = dto.Role,
                IsActive = true
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var address = new Address
            {
                UserId = user.Id,
                AddressName = dto.AddressName,
                Address1 = dto.Address,
                City = dto.City,
                Pincode = dto.PinCode
            };


            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            if (dto.Role != null && dto.Role.ToLower() == "seller")
            {
                var seller = new Seller
                {
                    UserId = user.Id,
                    StoreName = dto.StoreName ?? $"{user.Username}'s Store"
                };

                _context.Sellers.Add(seller);
                await _context.SaveChangesAsync();
            }

            return "User and Profile created successfully";
        }

        public async Task<string> LoginAsync(LoginRequest dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                throw new Exception("Invalid email");

            if (!user.IsActive)
                throw new Exception("User is inactive");

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password
            );

            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid password");

            return GenerateJwtToken(user);
        }

        public async Task<UserProfileResponse> GetUserProfile(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Addresses).Include(u => u.SellerUser)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found");

            return new UserProfileResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Username = user.Username,
                Phone = user.Phone,
                Role = user.Role,
                Gender = user.Gender,
                Seller = user.SellerUser == null ? null : new SellerDto
                {
                    Id = user.SellerUser.Id,
                    StoreName = user.SellerUser.StoreName,
                    City = user.SellerUser.City,
                    Country = user.SellerUser.Country,
                    VerificationStatus = user.SellerUser.VerificationStatus,
                    Gstin = user.SellerUser.Gstin,
                    IdProofDocument = user.SellerUser.IdProofDocument,
                    IdProofNumber = user.SellerUser.IdProofNumber,
                    BusinessLicense = user.SellerUser.BusinessLicense,
                    BankAccountNumber = user.SellerUser.BankAccountNumber,
                    BankIfsc = user.SellerUser.BankIfsc
                },
                CreatedDate = user.CreatedDate,

                Addresses = user.Addresses.Select(a => new AddressDto
                {
                    Id = a.Id,
                    AddressName = a.AddressName,
                    Address1 = a.Address1,
                    City = a.City,
                    Pincode = a.Pincode
                }).ToList()
            };
        }

        public async Task UpdateProfile(int userId, UpdateProfileRequest dto)
        {
            var user = await _context.Users
                .Include(u => u.SellerUser)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found");

            // Username uniqueness
            if (!string.IsNullOrEmpty(dto.Username) &&
                await _context.Users.AnyAsync(u => u.Username == dto.Username && u.Id != userId))
            {
                throw new Exception("Username already taken");
            }

            // Update USER fields (only if provided)
            if (!string.IsNullOrEmpty(dto.Name))
                user.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.Phone))
                user.Phone = dto.Phone;

            if (!string.IsNullOrEmpty(dto.Gender))
                user.Gender = dto.Gender;

            if (!string.IsNullOrEmpty(dto.Username))
                user.Username = dto.Username;

            if (user.Role.Equals("Seller", StringComparison.OrdinalIgnoreCase))
            {
                var seller = user.SellerUser;

                if (seller == null)
                {
                    seller = new Seller
                    {
                        UserId = userId,
                        StoreName = dto.StoreName ?? "My Store"
                    };
                    _context.Sellers.Add(seller);
                }

                if (!string.IsNullOrEmpty(dto.StoreName))
                    seller.StoreName = dto.StoreName;

                if (!string.IsNullOrEmpty(dto.City))
                    seller.City = dto.City;

                if (!string.IsNullOrEmpty(dto.Country))
                    seller.Country = dto.Country;

                if (!string.IsNullOrEmpty(dto.Gstin))
                    seller.Gstin = dto.Gstin;

                if (!string.IsNullOrEmpty(dto.IdProofDocument))
                    seller.IdProofDocument = dto.IdProofDocument;

                if (!string.IsNullOrEmpty(dto.IdProofNumber))
                    seller.IdProofNumber = dto.IdProofNumber;

                if (!string.IsNullOrEmpty(dto.BusinessLicense))
                    seller.BusinessLicense = dto.BusinessLicense;

                if (!string.IsNullOrEmpty(dto.BankAccountNumber))
                    seller.BankAccountNumber = dto.BankAccountNumber;

                if (!string.IsNullOrEmpty(dto.BankIfsc))
                    seller.BankIfsc = dto.BankIfsc;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<object> GetSalesReport(int sellerId)
        {
            var orders = await _context.OrderItems
                .Include(o => o.Product)
                .Where(o => o.Product.SellerId == sellerId)
                .ToListAsync();

            var totalSales = orders.Sum(o => o.Quantity * o.Price);

            var totalOrders = orders.Count;

            return new
            {
                TotalSales = totalSales,
                TotalOrders = totalOrders
            };
        }

        public async Task<List<Address>> GetAddresses(int userId)
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<string> GoogleLogin(string idToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[]
                {
            _config["GoogleAuth:ClientId"]
        }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(
                idToken,
                settings
            );

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == payload.Email);

            if (user == null)
            {
                user = new User
                {
                    Name = payload.Name,
                    Email = payload.Email,
                    Username = payload.Email.Split('@')[0],

                    Role = "Buyer",
                    IsActive = true,

                    PasswordHash = "",

                    Address = "",
                    Phone = "",
                };

                _context.Users.Add(user);

                await _context.SaveChangesAsync();
            }

            return GenerateJwtToken(user);
        }

        public async Task<object> GetSellerProducts(int userId)
        {
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (seller == null)
                throw new Exception("Seller not found");

            var products = await _context.Products
                .Where(p => p.SellerId == seller.Id)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.Stock,
                    p.ImageUrl,
                    p.CategoryId,
                    p.BrandId,
                    p.SellerId,
                    p.IsActive,
                    p.CreatedDate
                })
                .ToListAsync();

            return products;

            return products;
        }

        public async Task<object> GetSellerOrders(int userId)
        {
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (seller == null)
                throw new Exception("Seller not found");

            var orders = await _context.OrderItems
                .Include(o => o.Order)
                .Include(o => o.Product)
                .Where(o => o.Product.SellerId == seller.Id)
                .Select(o => new
                {
                    OrderId = o.OrderId,
                    ProductName = o.Product.Name,
                    Quantity = o.Quantity,
                    Price = o.Price,
                    Status = o.Order.Status,
                    CustomerId = o.Order.UserId,
                    OrderDate = o.Order.CreatedDate
                })
                .ToListAsync();

            return orders;
        }

        public async Task UpdateOrderStatus(int orderId, string status)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Order not found");

            order.Status = status;

            await _context.SaveChangesAsync();
        }
    }
}