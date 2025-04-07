using ASP.NETCore_WithoutEF.Models;

namespace ASP.NETCore_WithoutEF.Repository.Contract
{
    public interface ICompanyRepository <T> where T : class
    {
        Task<List<T>> GetCompanyList();

        Task<bool> Save(T model);

        Task<bool> Edit(T model);

        Task<bool> Delete(int id);
       

    }
}
