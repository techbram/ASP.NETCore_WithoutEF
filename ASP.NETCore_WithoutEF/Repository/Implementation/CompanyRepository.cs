using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repository.Contract;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection;
using System.Linq.Expressions;

namespace ASP.NETCore_WithoutEF.Repository.Implementation
{
    public class CompanyRepository : ICompanyRepository<Company>
    {
        private readonly string? _SQLString = "";
        public CompanyRepository(IConfiguration _configuration)
        {
            _SQLString = _configuration.GetConnectionString("SQLString");  //Getting connection with database
        }

        //Method to get list of Company
        public async Task<List<Company>> GetCompanyList()
        {
            List<Company> companyList = new List<Company>();             // Instantiate object(Product) 
            using (SqlConnection con = new SqlConnection(_SQLString))    // Create connection
            {
                con.Open();                                              // Open Connection

                SqlCommand cmd = new SqlCommand("sp_getCompanyList", con);  // Specify command store procedure

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Read stored data
                using (var dr = await cmd.ExecuteReaderAsync())
                {
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

            }
            return companyList;
        }

        //Method to update company
        public async Task<bool> Edit(Company model)
        {
            
            using (SqlConnection con = new SqlConnection(_SQLString))    
            {
                con.Open();                                             

                SqlCommand cmd = new SqlCommand("sp_editCompany", con);  

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Update Company

                //Add parameter with value
                cmd.Parameters.AddWithValue("Id", model.Id);
                cmd.Parameters.AddWithValue("Name", model.Name);
                cmd.Parameters.AddWithValue("StreetAddress", model.StreetAddress);
                cmd.Parameters.AddWithValue("City", model.City);
                cmd.Parameters.AddWithValue("State", model.State);
                cmd.Parameters.AddWithValue("PostalCode", model.PostalCode);
                cmd.Parameters.AddWithValue("PhoneNumber", model.PhoneNumber);

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }        // return type is bool(true OR false) 
                else { return false; }

            }
        }

        //Method to Save Company
        public async Task<bool> Save(Company model)
        {
            using (SqlConnection con = new SqlConnection(_SQLString))    
            {
                con.Open();                                             

                SqlCommand cmd = new SqlCommand("sp_saveCompany", con);     
                
                cmd.Parameters.AddWithValue("Name", model.Name);
                cmd.Parameters.AddWithValue("StreetAddress", model.StreetAddress);
                cmd.Parameters.AddWithValue("City", model.City);
                cmd.Parameters.AddWithValue("State", model.State);
                cmd.Parameters.AddWithValue("PostalCode", model.PostalCode);
                cmd.Parameters.AddWithValue("PhoneNumber", model.PhoneNumber);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }        // return type is bool(true OR false) 
                else { return false; }

            }
        }

        //Method to Delete Company
        public async Task<bool> Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(_SQLString))    // Create connection
            {
                con.Open();                                              // Open Connection

                SqlCommand cmd = new SqlCommand("sp_deleteCompany", con);  // Specify command store procedure

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("Id", id);

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }        // return type is bool(true OR false) 
                else { return false; }
            }
        }

    }
}

