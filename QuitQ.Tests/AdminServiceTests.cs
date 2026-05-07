using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using QuitQ.API.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

[TestFixture]
public class AdminServiceTests
{
    private QuitQDbContext _context;
    private AdminService _service;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<QuitQDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new QuitQDbContext(options);
        _service = new AdminService(_context);

        SeedData();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    private void SeedData()
    {
        _context.Users.AddRange(
            new User
            {
                Id = 1,
                Name = "User1",
                Username = "user1",
                Email = "user1@test.com",
                PasswordHash = "hashedpassword",
                Phone = "1234567890",
                Address = "Chennai",
                Role = "User",
                IsActive = true
            },
            new User
            {
                Id = 2,
                Name = "Seller1",
                Username = "seller1",
                Email = "seller1@test.com",
                PasswordHash = "hashedpassword",
                Phone = "9876543210",
                Address = "Bangalore",
                Role = "Seller",
                IsActive = true
            }
        );

        _context.SaveChanges();
    }

    [Test]
    public async Task GetAllUsers_ReturnsAll()
    {
        var result = await _service.GetAllUsers();

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("User", result.First().Role);
    }

    [Test]
    public async Task GetAllSellers_ReturnsAllSellers()
    {
        var result = await _service.GetAllSellers();

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("Seller", result.First().Role);
    }

    [Test]
    public async Task DeleteUser_RemovesUser()
    {
        await _service.DeleteUser(1);

        Assert.AreEqual(1, _context.Users.Count());
        Assert.IsFalse(_context.Users.Any(u => u.Id == 1));
    }

    [Test]
    public async Task DeleteSeller_DeactivatesSeller()
    {
        await _service.DeleteSeller(2);

        var seller = _context.Users.First(u => u.Id == 2);

        Assert.IsFalse(seller.IsActive);
    }
}