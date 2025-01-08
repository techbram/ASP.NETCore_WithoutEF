using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repositories.Contract;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace ASP.NETCore_WithoutEF.Repositories.Implementation
{
    public class CategoryRepository : IGenericRepository<Category>
    {
        private readonly string? _SQLString = "";
        public CategoryRepository(IConfiguration _configuration)
        {
            _SQLString = _configuration.GetConnectionString("SQLString");
        }
        public async Task<List<Category>> List()
        {
            List<Category> categoryList = new List<Category>();
            using (SqlConnection con = new SqlConnection(_SQLString)) 
            {
                con.Open();  //Open Connection
                SqlCommand cmd = new SqlCommand("sp_listCategories", con);   // Specify command (Stored Procedure)
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync()) 
                {
                    //read stored data and add to the list
                    while (await dr.ReadAsync()) 
                    {
                        categoryList.Add(new Category {
                            Id =  Convert.ToInt32(dr["c_id"]),
                            Name = dr["c_name"].ToString()
                        });
                    }
                }
            }
            return categoryList;

        }
        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Edit(Category model)
        {
            throw new NotImplementedException();
        }

        

        public Task<bool> Save(Category model)
        {
            throw new NotImplementedException();
        }
    }
}
