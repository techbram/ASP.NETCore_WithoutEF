using App.DataAccess.Repository.Contract;
using App.Models;
using ASP.NETCore_WithoutEF.App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DataAccess.Repository.Implementation
{
    public class RoleRepository : IRoleRepository<Role>
    {
        private readonly string? _SQLString = "";
        public RoleRepository(IConfiguration _configuration)
        {
            _SQLString = _configuration.GetConnectionString("SQLString");
        }

        public async Task<Role> GetRole(string roleId)
        {
            Role role = new Role();

            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("sp_getRoleById", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RoleId", roleId);

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (dr.HasRows)
                        {
                            while (await dr.ReadAsync())
                            {

                                role = new Role
                                {
                                    Id = dr["RoleId"].ToString(),
                                    Name = dr["Name"].ToString(),

                                };
                            }
                        }
                    }
                }
            }

            return role;

        }

        public async Task<List<Role>> GetRoleList() 
        {
            List<Role> roleList = new List<Role>();
            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_getRoleList", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        roleList.Add(new Role
                        {
                            Id = dr["Id"].ToString(),
                            Name = dr["Name"].ToString(),
                        });
                    }

                }

            }
            return roleList;
        }

    }
}
