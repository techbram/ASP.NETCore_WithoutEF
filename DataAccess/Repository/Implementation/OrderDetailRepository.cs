using ASP.NETCore_WithoutEF.App.Models;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace ASP.NETCore_WithoutEF.App.DataAccess.Repository.Implementation
{
    public class OrderDetailRepository : IOrderDetailRepository<OrderDetail>
    {
        private readonly string? _SQLString = "";
        public OrderDetailRepository(IConfiguration _configuration)
        {
            _SQLString = _configuration.GetConnectionString("SQLString");
        }
        
        public async Task<IEnumerable<OrderDetail>> GetOrderDetail(int orderId)
        {
            List<OrderDetail> orderDetail = new List<OrderDetail>();

            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_getOrderDetailByOrderId", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderId", orderId);

                SqlDataReader dr = await cmd.ExecuteReaderAsync();

                if (dr.HasRows)
                {
                    while (await dr.ReadAsync())
                    {
                        var obj = new OrderDetail
                        {
                            Id = Convert.ToInt32(dr["OrderDetailId"]),
                            OrderHeader = new OrderHeader()
                            {
                                Id = Convert.ToInt32(dr["OrderHeaderId"]),
                                OrderDate = Convert.ToDateTime(dr["OrderDate"]),
                                ShippingDate = Convert.ToDateTime(dr["ShippingDate"]),
                                OrderTotal = Convert.ToDouble(dr["OrderTotal"]),
                                OrderStatus = dr["OrderStatus"].ToString(),
                                PaymentStatus = dr["PaymentStatus"].ToString(),
                                TrackingNumber = dr["TrackingNumber"].ToString(),
                                Carrier = dr["Carrier"].ToString(),
                                PaymentDate = Convert.ToDateTime(dr["PaymentDate"]),
                                PaymentDueDate = Convert.ToDateTime(dr["PaymentDueDate"]),
                                PaymentIntentId = dr["PaymentIntentId"].ToString(),
                                Name = dr["Name"].ToString(),
                                StreetAddress = dr["StreetAddress"].ToString(),
                                City = dr["City"].ToString(),
                                State = dr["State"].ToString(),
                                PostalCode = dr["PostalCode"].ToString(),
                                PhoneNumber = dr["PhoneNumber"].ToString(),

                                ApplicationUser = new ApplicationUser()
                                {
                                    Id = dr["ApplicationUserId"].ToString(),
                                    Email = dr["Email"].ToString()
                                }
                            },
                            Product = new Product()
                            {
                                ProductName = dr["ProductName"].ToString(),
                                Id = Convert.ToInt32(dr["ProductId"])
                            },

                            Count = Convert.ToInt32(dr["Count"]),
                            Price = Convert.ToDouble(dr["Price"])
                        };
                        orderDetail.Add(obj);
                    }
                }

            }
            return orderDetail;
        }
        public async Task<bool> Save(OrderDetail model)
        {
            using (SqlConnection con = new SqlConnection(_SQLString))   
            {
                await con.OpenAsync();                                           

                SqlCommand cmd = new SqlCommand("sp_saveOrderDetail", con);  
               
                cmd.Parameters.AddWithValue("OrderHeaderId", model.OrderHeaderId);
                cmd.Parameters.AddWithValue("ProductId", model.ProductId);
                cmd.Parameters.AddWithValue("Count", model.Count);
                cmd.Parameters.AddWithValue("Price", model.Price);

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
        public Task<bool> Edit(OrderDetail model)
        {
            throw new NotImplementedException();
        }

    }
}
