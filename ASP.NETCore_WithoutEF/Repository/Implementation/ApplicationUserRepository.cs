using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repository.Contract;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;

namespace ASP.NETCore_WithoutEF.Repository.Implementation
{
    public class ApplicationUserRepository : IApplicationUserRepository<ApplicationUser>
    {

        private readonly string? _SQLString = "";
        public ApplicationUserRepository(IConfiguration _configuration)
        {
            _SQLString = _configuration.GetConnectionString("SQLString");     

        }
        public async Task<List<ApplicationUser>> GetUserList()
        {
            List<ApplicationUser> user = new List<ApplicationUser>();              

            using (SqlConnection con = new SqlConnection(_SQLString))       
            {
                con.Open();                                                   

                SqlCommand cmd = new SqlCommand("sp_getListApplicationUsers", con);   

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    //read stored data and add to the list

                    while (await dr.ReadAsync())
                    {
                        user.Add(new ApplicationUser
                        {
                            Id = dr["ApplicationUserId"].ToString(),
                            UserName = dr["UserName"].ToString(),
                            Email = dr["Email"].ToString(),
                            PasswordHash = dr["PasswordHash"].ToString(),
                            Name = dr["Name"].ToString(),
                            StreetAddress = dr["StreetAddress"].ToString(),
                            City = dr["City"].ToString(),
                            State = dr["State"].ToString(),
                            PostalCode = dr["PostalCode"].ToString(),
                            PhoneNumber = dr["PhoneNumber"].ToString(),
                            CompanyId = dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : Convert.ToInt32(dr["CompanyId"])
                        });
                    }
                }
            }
            return user;
        }
        
        public async Task<ApplicationUser> GetUser(string userId)
        {
           
            ApplicationUser user = new ApplicationUser();

            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("sp_getUserDetail", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (dr.HasRows)
                        {
                            while (await dr.ReadAsync())
                            {
                                
                                user = new ApplicationUser
                                {
                                    Id = dr["ApplicationUserId"].ToString(), 
                                    Name = dr["Name"].ToString(),
                                    StreetAddress = dr["StreetAddress"].ToString(),
                                    City = dr["City"].ToString(),
                                    State = dr["State"].ToString(),
                                    PostalCode = dr["PostalCode"].ToString(),
                                    PhoneNumber = dr["PhoneNumber"].ToString(),
                                    CompanyId = dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : Convert.ToInt32(dr["CompanyId"])
                                };
                            }
                        }
                    }
                }
            }

            return user; // Return null if no rows were found.
        }		
    }
}
