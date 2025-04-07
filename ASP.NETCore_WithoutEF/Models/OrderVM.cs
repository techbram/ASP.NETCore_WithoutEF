namespace ASP.NETCore_WithoutEF.Models
{
    public class OrderVM
    {
        public OrderHeader Orderheader { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }

    }
}
