using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DataAccess.Repository.Contract
{
    public interface IRoleRepository<T> where T : class
    {
        Task<T> GetRole(string roleId);
        Task<List<T>> GetRoleList();
    }
}
