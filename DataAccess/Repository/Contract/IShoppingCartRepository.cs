using ASP.NETCore_WithoutEF.App.Models;

namespace ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract
{
    public interface IShoppingCartRepository <T> where T : class
    {
        Task<List<T>> GetCartList(string applicationUserId);

        Task<T> GetCart(int cartId);

        Task<bool> Save(T model);

        Task<bool> Edit(T model);

        Task<bool> Delete(int id);

        Task<bool> DeleteCartItemByAppUserId(string id);

        
    }
}
