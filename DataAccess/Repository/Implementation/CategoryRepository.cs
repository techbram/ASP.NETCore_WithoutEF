using ASP.NETCore_WithoutEF.App.Models;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract;
using Microsoft.Data.SqlClient;

using Microsoft.Extensions.Configuration;

namespace ASP.NETCore_WithoutEF.App.DataAccess.Repository.Implementation
{
    public class CategoryRepository : ICategoryRepository<Category>
    {
        private readonly string? _SQLString = "";

        public CategoryRepository(IConfiguration _configuration)
        {
            _SQLString = _configuration.GetConnectionString("SQLString");
        }
        public async Task<List<Category>> GetCategoryList()
        {
            List<Category> categoryList = new List<Category>();

            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_getListCategory", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    categoryList.Add(new Category
                    {
                        Id = Convert.ToInt32(dr["c_id"]),
                        Name = dr["c_name"].ToString(),
                        CreatedDate = dr["CreatedDate"].ToString(),
                        DisplayOrder = Convert.ToInt32(dr["DisplayOrder"]),
                    });
                }

            }
            return categoryList;
        }

        public async Task<bool> EditCategory(Category model)
        {
            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_editCategory", con);

                cmd.Parameters.AddWithValue("c_id", model.Id);
                cmd.Parameters.AddWithValue("c_name", model.Name);
                cmd.Parameters.AddWithValue("DisplayOrder", model.DisplayOrder);
                cmd.Parameters.AddWithValue("CreatedDate", model.CreatedDate);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }
            }
        }

        public async Task<bool> SaveCategory(Category model)
        {
            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_saveCategory", con);

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
            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_deleteCategory", con);

                cmd.Parameters.AddWithValue("c_id", id);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }

            }

        }

    }
}
