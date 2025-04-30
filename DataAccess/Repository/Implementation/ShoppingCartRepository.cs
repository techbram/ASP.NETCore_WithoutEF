using ASP.NETCore_WithoutEF.App.Models;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;


namespace ASP.NETCore_WithoutEF.App.DataAccess.Repository.Implementation
{
    public class ShoppingCartRepository : IShoppingCartRepository<ShoppingCart>
    {
        private readonly string? _SQLString = "";
        public ShoppingCartRepository(IConfiguration _configuration)
        {
            _SQLString = _configuration.GetConnectionString("SQLString");     //Getting connection with database

        }
        
        public async Task<List<ShoppingCart>> GetCartList(string applicationUserId)
        {
            List<ShoppingCart> cartList = new List<ShoppingCart>();

            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("sp_listShoppingCartById", con);
                cmd.Parameters.AddWithValue("ApplicationUserId", applicationUserId);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    //read stored data and add to the list

                    while (await dr.ReadAsync())
                    {
                        cartList.Add(new ShoppingCart
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Product = new Product
                            {
                                Id = Convert.ToInt32(dr["ProductId"]),
                                ProductName = dr["p_name"].ToString(),
                                Price = Convert.ToDouble(dr["Price"]),
                                Price50 = Convert.ToDouble(dr["Price50"]),
                                Price100 = Convert.ToDouble(dr["Price100"]),
                                CategoryId = Convert.ToInt32(dr["CategoryId"]),
                            },
                            Count = Convert.ToInt32(dr["Count"]),
                            ApplicationUser = new ApplicationUser
                            {
                                Id = dr["ApplicationUserId"].ToString(),
                                Name = dr["Name"].ToString(),
                                StreetAddress = dr["StreetAddress"].ToString(),
                                City = dr["City"].ToString(),
                                State = dr["State"].ToString(),
                                PostalCode = dr["PostalCode"].ToString(),
                                PhoneNumber = dr["PhoneNumber"].ToString(),
                                CompanyId = dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : Convert.ToInt32(dr["CompanyId"])
                            }

                        });
                    }
                }
            }
            return cartList;
        }


        public async Task<bool> Save(ShoppingCart model)
        {
            using (SqlConnection con = new SqlConnection(_SQLString))    
            {
                con.Open();                                              

                SqlCommand cmd = new SqlCommand("sp_saveOrAddCart", con);  

                cmd.Parameters.AddWithValue("ProductId", model.ProductId);
                cmd.Parameters.AddWithValue("Count", model.Count);
                cmd.Parameters.AddWithValue("ApplicationUserId", model.ApplicationUserId);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }
            }
        }

        public async Task<bool> Edit(ShoppingCart model)
        {
            using (SqlConnection conn = new SqlConnection(_SQLString))   
            {
                conn.Open();                                            

                SqlCommand cmd = new SqlCommand("sp_editShoppingCart", conn); 

                cmd.Parameters.AddWithValue("Id", model.Id);
                cmd.Parameters.AddWithValue("ProductId", model.ProductId);
                cmd.Parameters.AddWithValue("Count", model.Count);
                cmd.Parameters.AddWithValue("ApplicationUserId", model.ApplicationUserId);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;  

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }      
                else { return false; }
            }
        }
        public async Task<bool> Delete(int cartId)
        {
            using (SqlConnection conn = new SqlConnection(_SQLString))  
            {
                conn.Open();                                           

                SqlCommand cmd = new SqlCommand("sp_deleteCartItem", conn);  

                cmd.Parameters.AddWithValue("@CartId", cartId);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;   

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }

            }
        }

        public async Task<bool> DeleteCartItemByAppUserId(string applicationUserId)
        {
            using (SqlConnection conn = new SqlConnection(_SQLString))  
            {
                conn.Open();                                            

                SqlCommand cmd = new SqlCommand("sp_deleteCartItemByAppUserId", conn); 

                cmd.Parameters.AddWithValue("@ApplicationUserId", applicationUserId);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;    

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }
            }
        }
        public async Task<ShoppingCart> GetCart(int cartId)
        {
            ShoppingCart cart = new ShoppingCart();

            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("sp_getCartItem", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@ShoppingCartId", cartId);
                 
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (dr.HasRows)
                        {
                            while (await dr.ReadAsync())
                            {
                                
                                cart = new ShoppingCart
                                {
                                    Id = Convert.ToInt32(dr["ShoppingCartId"]), 
                                    ProductId = Convert.ToInt32(dr["ProductId"]),
                                    Count = Convert.ToInt32(dr["Count"]),
                                    ApplicationUserId = dr["ApplicationUserId"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            return cart; 
        }

    }
}
