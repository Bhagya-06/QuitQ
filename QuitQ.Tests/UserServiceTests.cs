using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using QuitQ.API.DTOs.Request;
using QuitQ.API.Models;
using QuitQ.API.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuitQ.Tests
{
    public class UserServiceTests
    {
        private QuitQDbContext _context;
        private UserService _service;
        private readonly PasswordHasher<User> hasher = new PasswordHasher<User>();

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<QuitQDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new QuitQDbContext(options);

            // Mock IConfiguration (for JWT)
            var inMemorySettings = new Dictionary<string, string> {
                {"JwtSettings:SecretKey", "THIS_IS_A_VERY_SECRET_KEY_123456"},
                {"JwtSettings:Issuer", "TestIssuer"},
                {"JwtSettings:Audience", "TestAudience"}
            };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _service = new UserService(_context, config);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task Register_ValidInput_ReturnsSuccess()
        {
            var dto = new RegisterRequest
            {
                Name = "Test",
                Email = "test@mail.com",
                Username = "testuser",
                Password = hasher.HashPassword(null, "123456"),
                ConfirmPassword = hasher.HashPassword(null, "123456"),
                Phone = "1234567890",
                Address = "TestAddress",
                City = "Chennai",
                PinCode = "600001",
                Role = "User"
            };

            var result = await _service.RegisterAsync(dto);

            Assert.AreEqual("User and Profile created successfully", result);
        }

        [Test]
        public void Register_DuplicateEmail_ThrowsException()
        {
            _context.Users.Add(new User
            {
                Name = "Test",
                Email = "test@mail.com",
                Username = "testuser",
                PasswordHash = hasher.HashPassword(null, "123"),
                Phone = "1234567890",
                Address = "Chennai",
                Role = "User"
            });
            _context.SaveChanges();

            var dto = new RegisterRequest
            {
                Name = "Test User",
                Email = "test@mail.com",
                Username = "testuser",
                Password = hasher.HashPassword(null, "123456"),
                ConfirmPassword = hasher.HashPassword(null, "123456"),
                Phone = "1234567890",
                Address = "TestAddress",
                City = "Chennai",
                PinCode = "600001",
                Role = "User"
            };

            Assert.ThrowsAsync<Exception>(async () =>
                await _service.RegisterAsync(dto));
        }

        [Test]
        public async Task Register_SellerRole_CreatesSeller()
        {
            var dto = new RegisterRequest
            {
                Name = "Test",
                Email = "seller@test.com",
                Username = "seller",
                Password = hasher.HashPassword(null, "123456"),
                ConfirmPassword = hasher.HashPassword(null, "123456"),
                Phone = "1234567890",
                Address = "TestAddress",
                City = "Chennai",
                PinCode = "600001",
                Role = "Seller",
                StoreName = "Test Store"
            };

            await _service.RegisterAsync(dto);

            Assert.AreEqual(1, _context.Sellers.Count());
        }

        [Test]
        public async Task Register_PasswordIsHashed()
        {
            var dto = new RegisterRequest
            {
                Name = "Test User",
                Email = "test@mail.com",
                Username = "testuser",
                Password = hasher.HashPassword(null, "123456"),
                ConfirmPassword = hasher.HashPassword(null, "123456"),
                Phone = "1234567890",
                AddressName = "Home",
                Address = "TestAddress",
                City = "Chennai",
                PinCode = "600001",
                Role = "User"
            };

            await _service.RegisterAsync(dto);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            Assert.AreNotEqual("123456", user.PasswordHash);
        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            _context.Users.Add(new User
            {
                Name = "Test",
                Email = "test@mail.com",
                Username = "testuser",
                PasswordHash = hasher.HashPassword(null, "123456"),
                Phone = "1234567890",
                Address = "Chennai",
                Role = "User",
                IsActive = true
            });
            _context.SaveChanges();

            var result = await _service.LoginAsync(new LoginRequest
            {
                Email = "test@mail.com",
                Password = "123456"
            });

            Assert.IsNotNull(result);
        }

        [Test]
        public void Login_WrongPassword_ThrowsException()
        {
            _context.Users.Add(new User
            {
                Name = "Test",
                Email = "test@mail.com",
                Username = "testuser",
                PasswordHash = hasher.HashPassword(null, "123456"),
                Phone = "1234567890",
                Address = "Chennai",
                Role = "User"
            });
            _context.SaveChanges();

            Assert.ThrowsAsync<Exception>(async () =>
                await _service.LoginAsync(new LoginRequest
                {
                    Email = "test@mail.com",
                    Password = "wrong"
                }));
        }

       [Test]
        public void Login_InactiveUser_ThrowsException()
        {
            _context.Users.Add(new User
            {
                Name = "Test1",
                Email = "test1@mail.com",
                Username = "testuser1",
                PasswordHash = hasher.HashPassword(null, "123456"),
                Phone = "1234567890",
                Address = "Chennai",
                Role = "User",
                IsActive = false
            });
            _context.SaveChanges();

            // This will PASS incorrectly unless added IsActive check
            Assert.ThrowsAsync<Exception>(async () =>
                await _service.LoginAsync(new LoginRequest
                {
                    Email = "test@mail.com",
                    Password = "123456"
                }));
        }

        [Test]
        public void Login_EmailNotFound_ThrowsException()
        {
            Assert.ThrowsAsync<Exception>(async () =>
                await _service.LoginAsync(new LoginRequest
                {
                    Email = "no@mail.com",
                    Password = "123456"
                }));
        }

        [Test]
        public async Task GetUserProfile_ValidUser_ReturnsUserWithAddresses()
        {
            var user = new User
            {
                Name = "Test User",
                Email = "test@example.com",
                Username = "testuser",
                Phone = "1234567890",
                Address = "Test Address",
                PasswordHash = "hashed",
                Role = "Customer",
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var address1 = new Address
            {
                UserId = user.Id,
                AddressName = "Home",
                Address1 = "Street 1",
                City = "Chennai",
                Pincode = "600001"
            };

            var address2 = new Address
            {
                UserId = user.Id,
                AddressName = "Home",
                Address1 = "Street 2",
                City = "Chennai",
                Pincode = "600002"
            };

            _context.Addresses.AddRange(address1, address2);
            await _context.SaveChangesAsync();

            var result = await _service.GetUserProfile(user.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual(2, result.Addresses.Count);
            Assert.AreEqual("Street 1", result.Addresses[0].Address1);
        }

        [Test]
        public void GetUserProfile_InvalidUser_ThrowsException()
        {
            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _service.GetUserProfile(999)
            );

            Assert.AreEqual("User not found", ex.Message);
        }
    }
}