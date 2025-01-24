using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repository.Contract;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace ASP.NETCore_WithoutEF.Repository.Implementation
{
    public class ProductRepository : IGenericRepository<Product>
    {
        private readonly string? _SQLString = "";
        public ProductRepository(IConfiguration _configuration)
        {
            _SQLString = _configuration.GetConnectionString("SQLString");  //Getting connection with database
        }

        //Method to get list of Product
        public async Task<List<Product>> List()
        {
            List<Product> productList = new List<Product>();             // Instantiate object(Product) 
            using (SqlConnection con = new SqlConnection(_SQLString))    // Create connection
            {
                con.Open();                                              // Open Connection

                SqlCommand cmd = new SqlCommand("sp_listProducts", con);  // Specify command store procedure

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Read stored data
                using (var dr = await cmd.ExecuteReaderAsync())
                {
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
                                    //CreatedDate = dr["CreatedDate"].ToString()
                                },
                                Price = Convert.ToDecimal(dr["Price"]),
                                ImageUrl = dr["ImageUrl"].ToString(),
                                ProductDetail = dr["ProductDetail"].ToString(),
                                DiscountPrice = Convert.ToDecimal(dr["DiscountPrice"]),
                                DateAdded = dr["DateAdded"].ToString()

                            }); 
                    }

                }

            }
            return productList;
        }
       
        public async Task<bool> Save(Product model)
        {
            using (SqlConnection con = new SqlConnection(_SQLString))    // Create connection
            {
                con.Open();                                              // Open Connection
                
                SqlCommand cmd = new SqlCommand("sp_saveProduct", con);  // Specify command store procedure

                //Add parameter with value

                cmd.Parameters.AddWithValue("p_name", model.ProductName);
                cmd.Parameters.AddWithValue("c_id", model.CategoryRef.Id);
                cmd.Parameters.AddWithValue("Price", model.Price);
                cmd.Parameters.AddWithValue("ImageUrl", model.ImageUrl);
                cmd.Parameters.AddWithValue("ProductDetail", model.ProductDetail);
                cmd.Parameters.AddWithValue("DiscountPrice", model.DiscountPrice);
                cmd.Parameters.AddWithValue("DateAdded", model.DateAdded);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }
            }
        }

        public async Task<bool> Edit(Product model)
        {
            using (SqlConnection conn = new SqlConnection(_SQLString))   // Create connection
            {
                conn.Open();                                             // Open Connection

                SqlCommand cmd = new SqlCommand("sp_editProduct", conn);  // Specify command store procedure

                // Update product

                //Add parameter with value
                cmd.Parameters.AddWithValue("p_id", model.Id);
                cmd.Parameters.AddWithValue("p_name", model.ProductName);
                cmd.Parameters.AddWithValue("c_id", model.CategoryRef.Id);
                cmd.Parameters.AddWithValue("Price", model.Price);
                cmd.Parameters.AddWithValue("ImageUrl", model.ImageUrl);
                cmd.Parameters.AddWithValue("ProductDetail", model.ProductDetail);
                cmd.Parameters.AddWithValue("DiscountPrice", model.DiscountPrice);
                cmd.Parameters.AddWithValue("DateAdded", model.DateAdded);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;    //Specify command type

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }        // return type is bool(true OR false) 
                else { return false; }
            }
        }
        public async Task<Product> Detail(int id)
        {
            Product product = new Product();
           //Product product = null;             // return null if no prodduct is found

            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                //con.Open();
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_detailProduct", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;   //Specify command type                
                cmd.Parameters.AddWithValue("@p_id", id);    //  the stored procedure takes a parameter like @p_id

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
                            Price = Convert.ToDecimal(dr["Price"]), 
                            ImageUrl = dr["ImageUrl"].ToString(),   
                            DiscountPrice = Convert.ToDecimal(dr["DiscountPrice"]), 
                            ProductDetail = dr["ProductDetail"].ToString()  
                            
                        };
                    }
                }
            }
            return product;
        }
        public async Task<bool> Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_SQLString))   // Create connection
            {
                conn.Open();                                             // Open Connection

                SqlCommand cmd = new SqlCommand("sp_deleteProdduct", conn);  // Specify command store procedure

                cmd.Parameters.AddWithValue("p_id", id);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;    //Specify command type

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }

            }

        }

    }

}
