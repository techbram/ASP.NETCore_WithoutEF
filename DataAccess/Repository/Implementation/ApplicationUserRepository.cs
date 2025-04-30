using ASP.NETCore_WithoutEF.App.Models;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.Design;


namespace ASP.NETCore_WithoutEF.App.DataAccess.Repository.Implementation
{
    public class ApplicationUserRepository : IApplicationUserRepository<ApplicationUser>
    {

        private readonly string? _SQLString = "";
        public ApplicationUserRepository(IConfiguration _configuration)
        {
            _SQLString = _configuration.GetConnectionString("SQLString"); 
        }
        //public async Task<List<ApplicationUser>> GetUserList()
        //{
        //    List<ApplicationUser> user = new List<ApplicationUser>();

        //    using (SqlConnection con = new SqlConnection(_SQLString))
        //    {
        //        con.Open();

        //        SqlCommand cmd = new SqlCommand("sp_getListApplicationUsers", con);

        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //        using (var dr = await cmd.ExecuteReaderAsync())
        //        {
        //            //read stored data and add to the list

        //            while (await dr.ReadAsync())
        //            {
        //                user.Add(new ApplicationUser
        //                {
        //                    Id = dr["ApplicationUserId"].ToString(),
        //                    UserName = dr["UserName"].ToString(),
        //                    Email = dr["Email"].ToString(),
        //                    PasswordHash = dr["PasswordHash"].ToString(),
        //                    Name = dr["Name"].ToString(),
        //                    StreetAddress = dr["StreetAddress"].ToString(),
        //                    City = dr["City"].ToString(),
        //                    State = dr["State"].ToString(),
        //                    PostalCode = dr["PostalCode"].ToString(),
        //                    PhoneNumber = dr["PhoneNumber"].ToString(),
        //                    CompanyId = dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : Convert.ToInt32(dr["CompanyId"])
        //                });
        //            }
        //        }
        //    }
        //    return user;
        //}

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

            return user; 
        }

        public async Task<bool> SaveUser(ApplicationUser model)
        {
            using (SqlConnection con = new SqlConnection(_SQLString))    
            {
                con.Open();                                              

                SqlCommand cmd = new SqlCommand("sp_saveApplicaitonUser", con);

                cmd.Parameters.AddWithValue("UserId", Guid.NewGuid().ToString());
                cmd.Parameters.AddWithValue("UserName", model.Email);
                cmd.Parameters.AddWithValue("Email", model.Email);
                cmd.Parameters.AddWithValue("@EmailConfirmed", false);

                cmd.Parameters.AddWithValue("PasswordHash", model.PasswordHash);
                cmd.Parameters.AddWithValue("PhoneNumber", model.PhoneNumber);
                cmd.Parameters.AddWithValue("PhoneNumberConfirmed", false);
                cmd.Parameters.AddWithValue("TwoFactorEnabled", false);

                cmd.Parameters.AddWithValue("LockoutEnabled", false);
                cmd.Parameters.AddWithValue("AccessFailedCount", 0);
                cmd.Parameters.AddWithValue("Name", model.Name);

                cmd.Parameters.AddWithValue("StreetAddress", model.StreetAddress);
                cmd.Parameters.AddWithValue("City", model.City);
                cmd.Parameters.AddWithValue("Discriminator", string.Empty);
                cmd.Parameters.AddWithValue("State", model.State);

                cmd.Parameters.AddWithValue("PostalCode", model.PostalCode);
                cmd.Parameters.AddWithValue("CompanyId", model.CompanyId);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }
            }
        }

        public async Task<string> ValidateUser(string email, string password)
        {
            string applicationUserId = string.Empty;

            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                //con.Open();
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_validateUser", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;                   
                cmd.Parameters.AddWithValue("@userName", email);
                cmd.Parameters.AddWithValue("@password", password);

                SqlDataReader dr = await cmd.ExecuteReaderAsync();

                if (dr.HasRows)
                {
                    while (await dr.ReadAsync())
                    {
                        applicationUserId = dr["Id"].ToString();                       
                    }
                }
            }
            return applicationUserId;
        }
    }
}
