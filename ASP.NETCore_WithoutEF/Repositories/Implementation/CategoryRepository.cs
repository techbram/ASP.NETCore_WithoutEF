using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repositories.Contract;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace ASP.NETCore_WithoutEF.Repositories.Implementation
{
    public class CategoryRepository : IGenericRepository<Category>
    {
        private readonly string? _SQLString = "";

        // primary constructor to configure and instantiate database connection string
        public CategoryRepository(IConfiguration _configuration)
        {
            _SQLString = _configuration.GetConnectionString("SQLString");     //Getting connection with database
        }
        public async Task<List<Category>> List()
        {
            List<Category> categoryList = new List<Category>();                // Instantiate object(Category)  

            using (SqlConnection con = new SqlConnection(_SQLString))         // Create connection
            {
                con.Open();                                                   //Open Connection

                SqlCommand cmd = new SqlCommand("sp_getListCategory", con);   // Specify command (Stored Procedure)

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync()) 
                {
                    //read stored data and add to the list

                    while (await dr.ReadAsync()) 
                    {
                        categoryList.Add(new Category {
                            Id =  Convert.ToInt32(dr["c_id"]),
                            Name = dr["c_name"].ToString(),
                            CreatedDate = dr["CreatedDate"].ToString()
                        });
                    }
                }
            }
            return categoryList;

        }
        public Task<bool> Edit(Category model)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> Save(Category model)
        {
            using (SqlConnection con = new SqlConnection(_SQLString))    // Create connection
            {
                con.Open();                                              // Open Connection

                SqlCommand cmd = new SqlCommand("sp_saveCategory", con);  // Specify command store procedure

                //Add parameter with value

                cmd.Parameters.AddWithValue("c_name", model.Name);
                cmd.Parameters.AddWithValue("CreatedDate", model.CreatedDate);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }
            }
        }
        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        
    }
}
