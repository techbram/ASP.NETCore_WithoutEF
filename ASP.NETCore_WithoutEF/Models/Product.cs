namespace ASP.NETCore_WithoutEF.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }        
        public int CategoryId { get; set; }
        public Category CategoryRef { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? ProductDetail { get; set; }
        public decimal DiscountPrice { get; set; }
        public string? DateAdded { get; set; }
    }
}
