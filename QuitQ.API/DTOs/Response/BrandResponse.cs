namespace QuitQ.API.DTOs.Response
{
    public class BrandResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductMiniDto> Products { get; set; }
    }
}
