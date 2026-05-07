namespace QuitQ.API.DTOs.Response
{
    public class SalesReportResponse
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalProductsSold { get; set; }
    }
}
