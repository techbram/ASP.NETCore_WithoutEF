namespace ASP.NETCore_WithoutEF.App.Models.ViewModel
{
    public class ShoppingCartVM
    {
        public List<ShoppingCart> ShoppingCartList { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
