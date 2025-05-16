using App.Models.ViewModel;
using ASP.NETCore_WithoutEF.App.Models;
using System.Linq.Expressions;

namespace ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract
{
    public interface IApplicationUserRepository<T> where T : class
    {
        Task<T> GetUser(string userId);
        Task<UserInfo> ValidateUser(string email, string password );
        Task<string> SaveUserAndReturnId(T model);
        Task<List<T>> GetUserList();


        //Task<bool> AssignRoleToUser(string userId, string roleId);

        //Task<bool> SaveUser(T model);
    }

}
