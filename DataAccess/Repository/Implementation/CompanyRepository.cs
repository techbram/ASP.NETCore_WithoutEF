using ASP.NETCore_WithoutEF.App.Models;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace ASP.NETCore_WithoutEF.App.DataAccess.Repository.Implementation
{
    public class CompanyRepository : ICompanyRepository<Company>
    {
        private readonly string? _SQLString = "";
        public CompanyRepository(IConfiguration _configuration)
        {
            _SQLString = _configuration.GetConnectionString("SQLString");
        }

        public async Task<List<Company>> GetCompanyList()
        {
            List<Company> companyList = new List<Company>();

            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_getCompanyList", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    companyList.Add(new Company
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = dr["Name"].ToString(),
                        StreetAddress = dr["StreetAddress"].ToString(),
                        City = dr["City"].ToString(),
                        State = dr["State"].ToString(),
                        PostalCode = dr["PostalCode"].ToString(),
                        PhoneNumber = dr["PhoneNumber"].ToString()

                    });
                }

            }
            return companyList;
        }

        public async Task<bool> Edit(Company model)
        {

            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_editCompany", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("Id", model.Id);
                cmd.Parameters.AddWithValue("Name", model.Name);
                cmd.Parameters.AddWithValue("StreetAddress", model.StreetAddress);
                cmd.Parameters.AddWithValue("City", model.City);
                cmd.Parameters.AddWithValue("State", model.State);
                cmd.Parameters.AddWithValue("PostalCode", model.PostalCode);
                cmd.Parameters.AddWithValue("PhoneNumber", model.PhoneNumber);

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }

            }
        }

        public async Task<bool> Save(Company model)
        {
            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_saveCompany", con);

                cmd.Parameters.AddWithValue("Name", model.Name);
                cmd.Parameters.AddWithValue("StreetAddress", model.StreetAddress);
                cmd.Parameters.AddWithValue("City", model.City);
                cmd.Parameters.AddWithValue("State", model.State);
                cmd.Parameters.AddWithValue("PostalCode", model.PostalCode);
                cmd.Parameters.AddWithValue("PhoneNumber", model.PhoneNumber);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }

            }
        }

        public async Task<bool> Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_deleteCompany", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("Id", id);

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }
            }
        }

    }
}

