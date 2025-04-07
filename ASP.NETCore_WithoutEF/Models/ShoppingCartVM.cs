namespace ASP.NETCore_WithoutEF.Models
{
    public class ShoppingCartVM
    {
        public List<ShoppingCart> ShoppingCartList { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
