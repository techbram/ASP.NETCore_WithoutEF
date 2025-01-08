namespace ASP.NETCore_WithoutEF.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public int CategoryId { get; set; }
        public Category? CategoryRef { get; set; }
        public int Price { get; set; }
    }
}
