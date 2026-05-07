namespace QuitQ.API.DTOs.Request
{
    public class UpdateReviewRequest
    {
        public string Review { get; set; }
        public int Rating { get; set; }
        public string? MediaUrl { get; set; }
    }
}
