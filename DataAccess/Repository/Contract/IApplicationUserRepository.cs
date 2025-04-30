using ASP.NETCore_WithoutEF.App.Models;
using System.Linq.Expressions;

namespace ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract
{
    public interface IApplicationUserRepository<T> where T : class
    {
        Task<T> GetUser(string userId);
        Task<string> ValidateUser(string email, string password );       
        Task<bool> SaveUser(T model);

       
        //Task<List<T>> GetUserList();
    }

}
