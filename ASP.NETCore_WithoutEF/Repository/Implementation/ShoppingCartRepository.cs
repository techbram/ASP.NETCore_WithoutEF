using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repository.Contract;
using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace ASP.NETCore_WithoutEF.Repository.Implementation
{
    public class ShoppingCartRepository : IShoppingCartRepository<ShoppingCart>
    {
        private readonly string? _SQLString = "";
        public ShoppingCartRepository(IConfiguration _configuration)
        {
            _SQLString = _configuration.GetConnectionString("SQLString");     //Getting connection with database

        }
        //public async Task<List<ShoppingCart>> GetList()
        //{
        //    List<ShoppingCart> cartList = new List<ShoppingCart>();

        //    using (SqlConnection con = new SqlConnection(_SQLString))
        //    {
        //        con.Open();

        //        SqlCommand cmd = new SqlCommand("sp_listShoppingCart", con);
        //        //cmd.Parameters.AddWithValue("ApplicationUserId", applicationUserId);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //        using (var dr = await cmd.ExecuteReaderAsync())
        //        {
        //            //read stored data and add to the list

        //            while (await dr.ReadAsync())
        //            {
        //                cartList.Add(new ShoppingCart
        //                {
        //                    Id = Convert.ToInt32(dr["Id"]),
        //                    Product = new Product
        //                    {
        //                        Id = Convert.ToInt32(dr["ProductId"]),
        //                        ProductName = dr["p_name"].ToString(),
        //                        Price = Convert.ToDouble(dr["Price"]),
        //                        Price50 = Convert.ToDouble(dr["Price50"]),
        //                        Price100 = Convert.ToDouble(dr["Price100"]),
        //                        CategoryId = Convert.ToInt32(dr["CategoryId"]),
        //                    },
        //                    Count = Convert.ToInt32(dr["Count"]),
        //                    ApplicationUser = new ApplicationUser
        //                    {
        //                        Id = dr["ApplicationUserId"].ToString(),
        //                        Name = dr["Name"].ToString(),
        //                        StreetAddress = dr["StreetAddress"].ToString(),
        //                        City = dr["City"].ToString(),
        //                        State = dr["State"].ToString(),
        //                        PostalCode = dr["PostalCode"].ToString(),
        //                        PhoneNumber = dr["PhoneNumber"].ToString(),
        //                        CompanyId = Convert.ToInt32(dr["CompanyId"])
        //                    }

        //                });
        //            }
        //        }
        //    }
        //    return cartList;
        //}
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
            using (SqlConnection con = new SqlConnection(_SQLString))    // Create connection
            {
                con.Open();                                              // Open Connection

                SqlCommand cmd = new SqlCommand("sp_saveOrAddCart", con);  // Specify command store procedure

                //Add parameter with value

                //cmd.Parameters.AddWithValue("Id", model.Id);
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
            //throw new NotImplementedException();
            using (SqlConnection conn = new SqlConnection(_SQLString))   // Create connection
            {
                conn.Open();                                             // Open Connection

                SqlCommand cmd = new SqlCommand("sp_editShoppingCart", conn);  // Specify command store procedure

                // Update product

                //Add parameter with value
                cmd.Parameters.AddWithValue("Id", model.Id);
                cmd.Parameters.AddWithValue("ProductId", model.ProductId);
                cmd.Parameters.AddWithValue("Count", model.Count);
                cmd.Parameters.AddWithValue("ApplicationUserId", model.ApplicationUserId);


                cmd.CommandType = System.Data.CommandType.StoredProcedure;    //Specify command type

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }        // return type is bool(true OR false) 
                else { return false; }
            }
        }
        public async Task<bool> Delete(int cartId)
        {
            using (SqlConnection conn = new SqlConnection(_SQLString))   // Create connection
            {
                conn.Open();                                             // Open Connection

                SqlCommand cmd = new SqlCommand("sp_deleteCartItem", conn);  // Specify command store procedure

                cmd.Parameters.AddWithValue("@CartId", cartId);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;    //Specify command type

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }

            }
        }

        public async Task<bool> DeleteCartItemByAppUserId(string applicationUserId)
        {
            using (SqlConnection conn = new SqlConnection(_SQLString))   // Create connection
            {
                conn.Open();                                             // Open Connection

                SqlCommand cmd = new SqlCommand("sp_deleteCartItemByAppUserId", conn);  // Specify command store procedure

                cmd.Parameters.AddWithValue("@ApplicationUserId", applicationUserId);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;    //Specify command type

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
                  
                    // Use a using block for SqlDataReader to ensure it is properly disposed
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (dr.HasRows)
                        {
                            while (await dr.ReadAsync())
                            {
                                // Populate the cart object with the data
                                cart = new ShoppingCart
                                {
                                    Id = Convert.ToInt32(dr["ShoppingCartId"]), // Ensure correct column name
                                    ProductId = Convert.ToInt32(dr["ProductId"]),
                                    Count = Convert.ToInt32(dr["Count"]),
                                    ApplicationUserId = dr["ApplicationUserId"].ToString()
                                };
                            }
                        }
                    }
                }
            }

            return cart; // Return null if no rows were found.
        }

    }
}
