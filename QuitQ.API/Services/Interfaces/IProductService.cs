using QuitQ.API.DTOs.Request;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;

namespace QuitQ.API.Services.Interfaces
{
    public interface IProductService
    {
        Task AddProduct(AddProductRequest dto, int sellerId);
        Task<List<ProductResponse>> GetAll(int? categoryId = null);
        Task<ProductResponse> GetById(int id);

        Task<bool> UpdateProduct(int productId, AddProductRequest dto, int userId);

        Task<bool> DeleteProduct(int productId, int userId);

        Task<object> SearchProducts(string? name, string? storeName, string? categoryName, string? brandName, decimal? minPrice,
            decimal? maxPrice, int? minStock, string? sortBy, bool isDescending, int page = 1, int pageSize = 10);
    }
}
