namespace ASP.NETCore_WithoutEF.Models
{
    public class ShoppingCartVM
    {
        //public int ShoppingCartId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

        public List<ShoppingCart> ShoppingCartList { get; set; }    
        
        public List<ApplicationUser> ApplicationUserList { get; set; }

        public OrderHeader OrderHeader { get; set; }
        


    }
}
