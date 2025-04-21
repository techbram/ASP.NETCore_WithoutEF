namespace ASP.NETCore_WithoutEF.Models
{
    public class OrderVM
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetail> OrderDetailList { get; set; }      
    }
}
