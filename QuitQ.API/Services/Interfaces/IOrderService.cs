using QuitQ.API.DTOs.Request;
using QuitQ.API.Models;

namespace QuitQ.API.Services.Interfaces
{
    public interface IOrderService
    {
        Task PlaceOrder(PlaceOrderRequest request, int userId);

        Task PlaceDirectProductOrder(int productId, int quantity, int addressId, int userId);

        Task<List<Order>> GetOrderHistory(int userId);

        Task<Order> GetOrderById(int orderId, int userId);

    }
}