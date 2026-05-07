namespace QuitQ.API.DTOs.Response
{
    public class ProductResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public required int Stock { get; set; }
        public required string ImageUrl { get; set; }
        public required string CategoryName { get; set; }
        public required string BrandName { get; set; }
        public required string StoreName { get; set; }  // seller's store name
    }
}