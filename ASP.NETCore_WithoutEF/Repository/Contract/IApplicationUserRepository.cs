using ASP.NETCore_WithoutEF.Models;
using System.Linq.Expressions;

namespace ASP.NETCore_WithoutEF.Repository.Contract
{
    public interface IApplicationUserRepository<T> where T : class
    {
        Task<T> GetUser(string userId);
        Task<List<T>> GetUserList();
        
    }
    
}
