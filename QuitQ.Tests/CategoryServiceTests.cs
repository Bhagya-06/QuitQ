using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using QuitQ.API.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

[TestFixture]
public class CategoryServiceTests
{
    private QuitQDbContext _context;
    private CategoryService _service;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<QuitQDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new QuitQDbContext(options);
        _service = new CategoryService(_context);

        SeedData();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    private void SeedData()
    {
        _context.Categories.Add(new Category
        {
            Id = 1,
            Name = "Electronics",
            Description = "Devices"
        });

        _context.SaveChanges();
    }

    [Test]
    public async Task GetAllCategories_ReturnsAll()
    {
        var result = await _service.GetAllCategories();

        Assert.AreEqual(1, result.Count);
    }

    [Test]
    public async Task AddCategory_CreatesCategory()
    {
        await _service.AddCategory("Clothing", "Wearables");

        Assert.AreEqual(2, _context.Categories.Count());
    }

    [Test]
    public async Task UpdateCategory_UpdatesCorrectly()
    {
        await _service.UpdateCategory(1, "Updated", "Updated Description");

        Assert.AreEqual("Updated", _context.Categories.First().Name);
    }

    [Test]
    public void UpdateCategory_InvalidId_ThrowsNotFound()
    {
        Assert.ThrowsAsync<Exception>(() =>
            _service.UpdateCategory(999, "Invalid", "Invalid Description"));
    }

    [Test]
    public async Task DeleteCategory_RemovesCorrectly()
    {
        await _service.DeleteCategory(1);

        Assert.AreEqual(0, _context.Categories.Count());
    }

    [Test]
    public void DeleteCategory_InvalidId_ThrowsNotFound()
    {
        Assert.ThrowsAsync<Exception>(() =>
            _service.DeleteCategory(999));
    }
}