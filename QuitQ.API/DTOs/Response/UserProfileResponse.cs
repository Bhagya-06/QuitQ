namespace QuitQ.API.DTOs.Response
{
    public class UserProfileResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string? Gender { get; set; }
        public SellerDto? Seller { get; set; }
        public DateTime? CreatedDate { get; set; }

        public List<AddressDto> Addresses { get; set; }
    }

    public class AddressDto
    {
        public int Id { get; set; }
        public string AddressName { get; set; } = "Home";
        public required string Address1 { get; set; }
        public required string City { get; set; }
        public required string Pincode { get; set; }
    }

    public class SellerDto
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? VerificationStatus { get; set; }
        public string? Gstin { get; set; }
        public string? IdProofDocument { get; set; }
        public string? IdProofNumber { get; set; }
        public string? BusinessLicense { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankIfsc { get; set; }
    }
}
