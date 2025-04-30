using ASP.NETCore_WithoutEF.App.Models;

namespace ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract
{
    public interface IProductRepository <T> where T : class
    {
        Task<List<T>> GetProductList();

        Task<T> GetProduct(int id);

        Task<bool> Save(T model);

        Task<bool> Edit(T model);

        Task<bool> Delete(int id);
        

    }
}
