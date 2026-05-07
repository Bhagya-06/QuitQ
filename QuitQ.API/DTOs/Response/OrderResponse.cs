namespace QuitQ.API.DTOs.Response
{
    public class OrderResponse
    {
        public required int Id { get; set; }
        public required DateTime CreatedDate { get; set; }
        public required decimal Total { get; set; }
        public required string Status { get; set; }
        public required string TrackingNumber { get; set; }
        public required List<OrderItemResponse> Items { get; set; }
    }

    public class OrderItemResponse
    {
        public required string ProductName { get; set; }
        public required int Quantity { get; set; }
        public required decimal Price { get; set; }
    }
}