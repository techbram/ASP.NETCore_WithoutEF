using ASP.NETCore_WithoutEF.App.Models;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace ASP.NETCore_WithoutEF.App.DataAccess.Repository.Implementation
{
    public class ProductRepository : IProductRepository<Product>
    {
        private readonly string? _SQLString = "";

        public ProductRepository(IConfiguration _configuration)
        {
            _SQLString = _configuration.GetConnectionString("SQLString");
        }

        public async Task<List<Product>> GetProductList()
        {
            List<Product> productList = new List<Product>();

            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_listProducts", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    productList.Add(new Product
                    {
                        Id = Convert.ToInt32(dr["p_id"]),
                        ProductName = dr["p_name"].ToString(),
                        CategoryRef = new Category()
                        {
                            Id = Convert.ToInt32(dr["c_id"]),
                            Name = dr["c_name"].ToString(),
                        },
                        Price = Convert.ToDouble(dr["Price"]),
                        ImageUrl = dr["ImageUrl"].ToString(),
                        OldImageUrl = dr["ImageUrl"].ToString(),
                        ProductDetail = dr["ProductDetail"].ToString(),
                        DiscountPrice = Convert.ToDouble(dr["DiscountPrice"]),
                        DateAdded = dr["DateAdded"].ToString(),
                        ProductNumber = dr["ProductNumber"].ToString(),
                        Originated = dr["Originated"].ToString(),
                        Price50 = Convert.ToDouble(dr["Price50"]),
                        Price100 = Convert.ToDouble(dr["Price100"])
                    });
                }

            }
            return productList;
        }

        public async Task<bool> Save(Product model)
        {
            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_saveProduct", con);

                cmd.Parameters.AddWithValue("p_name", model.ProductName);
                cmd.Parameters.AddWithValue("c_id", model.CategoryId);
                cmd.Parameters.AddWithValue("Price", model.Price);
                //cmd.Parameters.AddWithValue("ImageUrl", model.ImageUrl);
                cmd.Parameters.AddWithValue("ImageUrl", (object?)model.ImageUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("ProductDetail", model.ProductDetail);
                cmd.Parameters.AddWithValue("DiscountPrice", model.DiscountPrice);
                cmd.Parameters.AddWithValue("DateAdded", model.DateAdded);
                cmd.Parameters.AddWithValue("ProductNumber", model.ProductNumber);
                cmd.Parameters.AddWithValue("Originated", model.Originated);
                cmd.Parameters.AddWithValue("Price50", model.Price50);
                cmd.Parameters.AddWithValue("Price100", model.Price100);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }
            }
        }

        public async Task<bool> Edit(Product model)
        {
            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_editProduct", con);

                cmd.Parameters.AddWithValue("p_id", model.Id);
                cmd.Parameters.AddWithValue("p_name", model.ProductName);
                cmd.Parameters.AddWithValue("c_id", model.CategoryId);
                cmd.Parameters.AddWithValue("Price", model.Price);
                cmd.Parameters.AddWithValue("ImageUrl", (object?)model.ImageUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("ProductDetail", model.ProductDetail);
                cmd.Parameters.AddWithValue("DiscountPrice", model.DiscountPrice);
                cmd.Parameters.AddWithValue("DateAdded", model.DateAdded);
                cmd.Parameters.AddWithValue("ProductNumber", model.ProductNumber);
                cmd.Parameters.AddWithValue("Originated", model.Originated);
                cmd.Parameters.AddWithValue("Price50", model.Price50);
                cmd.Parameters.AddWithValue("Price100", model.Price100);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }
            }
        }
        public async Task<Product> GetProduct(int id)
        {
            Product product = new Product();

            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_detailProduct", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductId", id);

                SqlDataReader dr = await cmd.ExecuteReaderAsync();

                if (dr.HasRows)
                {
                    while (await dr.ReadAsync())
                    {
                        product = new Product
                        {
                            Id = Convert.ToInt32(dr["p_id"]),
                            ProductName = dr["p_name"].ToString(),
                            CategoryRef = new Category
                            {
                                Id = Convert.ToInt32(dr["c_id"]),
                                Name = dr["c_name"].ToString()
                            },
                            Price = Convert.ToDouble(dr["Price"]),
                            ImageUrl = dr["ImageUrl"].ToString(),
                            DiscountPrice = Convert.ToDouble(dr["DiscountPrice"]),
                            ProductDetail = dr["ProductDetail"].ToString(),
                            ProductNumber = dr["ProductNumber"].ToString(),
                            Originated = dr["Originated"].ToString(),
                            Price50 = Convert.ToDouble(dr["Price50"]),
                            Price100 = Convert.ToDouble(dr["Price100"])

                        };
                    }
                }
            }
            return product;
        }
        public async Task<bool> Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(_SQLString))   
            {
                await con.OpenAsync();                                             

                SqlCommand cmd = new SqlCommand("sp_deleteProdduct", con);  

                cmd.Parameters.AddWithValue("p_id", id);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;   

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }

            }

        }

    }

}
