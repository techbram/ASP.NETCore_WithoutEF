using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repository.Contract;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.CodeDom;
using System.Linq.Expressions;

namespace ASP.NETCore_WithoutEF.Repository.Implementation
{
    public class CategoryRepository : ICategoryRepository<Category>
    {
        private readonly string? _SQLString = "";

       

        // primary constructor to configure and instantiate database connection string
        public CategoryRepository(IConfiguration _configuration)
        {
            _SQLString = _configuration.GetConnectionString("SQLString");     //Getting connection with database
        }
        public async Task<List<Category>> GetCategoryList()
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
                            CreatedDate = dr["CreatedDate"].ToString(),
                            DisplayOrder = Convert.ToInt32(dr["DisplayOrder"]),
                        });
                    }
                }
            }
            return categoryList;

        }
        public async Task<bool> EditCategory(Category model)
        {
            using (SqlConnection con = new SqlConnection(_SQLString)) { 

                con.Open();
                SqlCommand cmd = new SqlCommand("sp_editCategory", con);

                // Update Category
                cmd.Parameters.AddWithValue("c_id", model.Id);
                cmd.Parameters.AddWithValue("c_name", model.Name);
                cmd.Parameters.AddWithValue("DisplayOrder", model.DisplayOrder);
                cmd.Parameters.AddWithValue("CreatedDate", model.CreatedDate);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;     //Specify command type

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }
            }
        }
        public async Task<bool> SaveCategory(Category model)
        {
            using (SqlConnection con = new SqlConnection(_SQLString))    // Create connection
            {
                con.Open();                                              // Open Connection

                SqlCommand cmd = new SqlCommand("sp_saveCategory", con);  // Specify command store procedure

                //Add parameter with value
                
                cmd.Parameters.AddWithValue("c_name", model.Name);
                cmd.Parameters.AddWithValue("DisplayOrder", model.DisplayOrder);
                cmd.Parameters.AddWithValue("CreatedDate", model.CreatedDate);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }
            }
        }
        public async Task<bool> DeleteCategory(int id)
        {
            using (SqlConnection conn = new SqlConnection(_SQLString))   // Create connection
            {
                conn.Open();                                             // Open Connection

                SqlCommand cmd = new SqlCommand("sp_deleteCategory", conn);  // Specify command store procedure

                cmd.Parameters.AddWithValue("c_id", id);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;    //Specify command type

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }

            }

        }       
       
    }
}
