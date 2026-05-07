using System.ComponentModel.DataAnnotations;

namespace QuitQ.API.DTOs.Request
{
    public class RegisterRequest
    {
        public required string Username { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; } 
        public required string Phone { get; set; }
        public required string Address { get; set; }
        public string AddressName { get; set; } = "Home";
        public required string City { get; set; }
        public required string PinCode { get; set; }
        public required string Role { get; set; }
        public string? StoreName { get; set; }
    }
}