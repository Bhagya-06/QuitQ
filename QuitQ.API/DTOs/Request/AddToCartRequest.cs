using System.ComponentModel.DataAnnotations;

namespace QuitQ.API.DTOs.Request
{
    public class AddToCartRequest
    {
        public required int ProductId { get; set; }
        public required int Quantity { get; set; }
    }
}