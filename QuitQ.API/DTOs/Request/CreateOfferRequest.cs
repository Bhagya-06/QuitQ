namespace QuitQ.API.DTOs.Request
{
    public class CreateOfferRequest
    {
        public string Title { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
