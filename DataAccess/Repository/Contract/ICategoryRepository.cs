using ASP.NETCore_WithoutEF.App.Models;

namespace ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract
{
    public interface ICategoryRepository <T> where T : class
    {
        Task<List<T>> GetCategoryList();

        Task<bool> SaveCategory(T model);

        Task<bool> EditCategory(T model);

        Task<bool> DeleteCategory(int id);        

    }
}
