using System.ComponentModel.DataAnnotations;

namespace QuitQ.API.DTOs.Request
{
    public class AddProductRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
        public string ImageUrl { get; set; }
        public required int CategoryId { get; set; }
        public int? BrandId { get; set; }
    }
}