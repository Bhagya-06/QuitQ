using System.ComponentModel.DataAnnotations;

namespace QuitQ.API.DTOs.Request
{
    public class AddAddressRequest
    {
        [Required]
        public required string AddressName { get; set; }  // e.g. "Home", "Office"

        [Required]
        public required string Address1 { get; set; }

        public required string City { get; set; }

        public required string Pincode { get; set; }

        public bool IsDefault { get; set; }
    }
}