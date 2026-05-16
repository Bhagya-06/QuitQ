namespace QuitQ.API.DTOs.Request
{
    public class UpdateProfileRequest
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Username { get; set; }
        public string? StoreName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Gstin { get; set; }
        public string? IdProofDocument { get; set; }
        public string? IdProofNumber { get; set; }
        public string? BusinessLicense { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankIfsc { get; set; }
    }
}
