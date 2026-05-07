using System.ComponentModel.DataAnnotations;

namespace QuitQ.API.DTOs.Request
{
    public class PlaceOrderRequest
    {
        public required int AddressId { get; set; }
        public required PaymentMethod PaymentMethod { get; set; } 
    }

    public enum PaymentMethod { UPI, Card, Netbanking, COD }
}