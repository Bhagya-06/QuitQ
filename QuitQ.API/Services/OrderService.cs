using QuitQ.API.DTOs.Request;
using QuitQ.API.Models;
using QuitQ.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class OrderService:IOrderService
{
    private readonly QuitQDbContext _context;

    public OrderService(QuitQDbContext context)
    {
        _context = context;
    }

    public async Task PlaceOrder(PlaceOrderRequest request, int userId)
    {
        var cartItems = await _context.ShoppingCarts
            .Where(c => c.UserId == userId)
            .ToListAsync();

        if (!cartItems.Any())
            throw new Exception("Cart empty");

        var address = await _context.Addresses
            .FirstOrDefaultAsync(a => a.Id == request.AddressId && a.UserId == userId);

        if (address == null)
            throw new Exception("Invalid address");

        var groupedItems = cartItems
            .GroupBy(c => c.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                Quantity = g.Sum(x => x.Quantity)
            })
            .ToList();

        decimal total = 0;

        foreach (var item in groupedItems)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == item.ProductId);

            if (product == null)
                throw new Exception($"Product {item.ProductId} not found");

            if (product.Stock < item.Quantity)
                throw new Exception($"Insufficient stock for {product.Name}");

            product.Stock -= item.Quantity;
            total += product.Price * item.Quantity;
        }

        var order = new Order
        {
            UserId = userId,
            AddressId = request.AddressId,
            Total = total,
            Status = "Pending",
            CreatedDate = DateTime.Now,
            TrackingNumber = Guid.NewGuid().ToString()
        };

        _context.Orders.Add(order);

        foreach (var item in cartItems)
        {
            var product = await _context.Products.FindAsync(item.ProductId);

            _context.OrderItems.Add(new OrderItem
            {
                Order = order, 
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = product.Price
            });
        }

        _context.ShoppingCarts.RemoveRange(cartItems);

        await _context.SaveChangesAsync();
    }

    public async Task PlaceDirectProductOrder(int productId, int quantity, int addressId, int userId)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null)
            throw new Exception("Product not found");

        if (product.Stock < quantity)
            throw new Exception("Out of stock");

        var address = await _context.Addresses
            .FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);

        if (address == null)
            throw new Exception("Invalid address");

        product.Stock -= quantity;

        var order = new Order
        {
            UserId = userId,
            AddressId = addressId,
            Total = product.Price * quantity,
            Status = "Pending",
            CreatedDate = DateTime.Now,
            TrackingNumber = Guid.NewGuid().ToString(),

            OrderItems = new List<OrderItem>
        {
            new OrderItem
            {
                ProductId = productId,
                Quantity = quantity,
                Price = product.Price
            }
        }
        };

        _context.Orders.Add(order);

        await _context.SaveChangesAsync();
    }

    public async Task<List<Order>> GetOrderHistory(int userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
            .ToListAsync();
    }

    public async Task<Order> GetOrderById(int orderId, int userId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null || order.UserId != userId)
            throw new Exception("Unauthorized");

        return order;
    }


    private decimal CalculateTotal(List<ShoppingCart> cartItems)
    {
        return cartItems.Sum(c => c.Product.Price * c.Quantity);
    }
}