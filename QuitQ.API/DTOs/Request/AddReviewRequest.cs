namespace QuitQ.API.DTOs.Request
{
    public class AddReviewRequest
    {
        public int ProductId { get; set; }
        public required string Comment { get; set; }
        public int Rating { get; set; }
        public string? ReviewImages { get; set; }
        public string? ReviewVideo { get; set; }
    }
}
