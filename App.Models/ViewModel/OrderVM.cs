namespace ASP.NETCore_WithoutEF.App.Models.ViewModel
{
    public class OrderVM
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetail> OrderDetailList { get; set; }      
    }
}
