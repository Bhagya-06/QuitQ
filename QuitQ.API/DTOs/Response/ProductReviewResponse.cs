namespace QuitQ.API.DTOs.Response
{
    public class ProductReviewResponse
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string ReviewImages { get; set; }
        public string ReviewVideo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UserName { get; set; }
    }
}
