using ASP.NETCore_WithoutEF.Models;
using System.Linq.Expressions;

namespace ASP.NETCore_WithoutEF.Repository.Contract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> List();
        
        Task<bool> Save(T model);

        Task<bool> Edit(T model);

        Task<bool> Delete(int id);
        Task<T> Detail(int id);
       
       
    }
    
}
