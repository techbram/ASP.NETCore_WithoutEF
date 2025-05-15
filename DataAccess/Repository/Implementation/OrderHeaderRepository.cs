using ASP.NETCore_WithoutEF.App.Models;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace ASP.NETCore_WithoutEF.App.DataAccess.Repository.Implementation
{
    public class OrderHeaderRepository : IOrderHeaderRepository<OrderHeader>
    {
        private readonly string? _SQLString = "";
        public OrderHeaderRepository(IConfiguration _configuration)
        {
            _SQLString = _configuration.GetConnectionString("SQLString");

        }
        public async Task<int> InsertOrderHeaderInfo(OrderHeader model)
        {
            int lastInsertedId = 0;
            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_saveOrderHeader", con);

                cmd.Parameters.AddWithValue("ApplicationUserId", model.ApplicationUserId);
                cmd.Parameters.AddWithValue("OrderDate", model.OrderDate);
                cmd.Parameters.AddWithValue("@ShippingDate", model.ShippingDate ?? (object)DBNull.Value);

                cmd.Parameters.AddWithValue("OrderTotal", model.OrderTotal);
                cmd.Parameters.AddWithValue("OrderStatus", model.OrderStatus);
                cmd.Parameters.AddWithValue("PaymentStatus", model.PaymentStatus);
                cmd.Parameters.AddWithValue("TrackingNumber", string.IsNullOrEmpty(model.TrackingNumber) ? DBNull.Value : model.TrackingNumber);
                cmd.Parameters.AddWithValue("Carrier", string.IsNullOrEmpty(model.Carrier) ? DBNull.Value : model.Carrier);

                cmd.Parameters.AddWithValue("@PaymentDate", model.PaymentDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("SessionId", string.IsNullOrEmpty(model.SessionId) ? DBNull.Value : (object)model.SessionId);
                cmd.Parameters.AddWithValue("PaymentIntentId", string.IsNullOrEmpty(model.PaymentIntentId) ? DBNull.Value : (object)model.PaymentIntentId);
                cmd.Parameters.AddWithValue("@PaymentDueDate", model.PaymentDueDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("Name", model.Name);
                cmd.Parameters.AddWithValue("StreetAddress", model.StreetAddress);
                cmd.Parameters.AddWithValue("City", model.City);
                cmd.Parameters.AddWithValue("State", model.State);
                cmd.Parameters.AddWithValue("PostalCode", model.PostalCode);
                cmd.Parameters.AddWithValue("PhoneNumber", model.PhoneNumber);
                cmd.Parameters.Add(new SqlParameter()
                {
                    Direction = ParameterDirection.Output,
                    ParameterName = "@LastInsertedId",
                    SqlDbType = SqlDbType.Int
                });

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (cmd.Parameters["@LastInsertedId"].Value != null)
                {
                    lastInsertedId = Convert.ToInt32(cmd.Parameters["@LastInsertedId"].Value);
                }
            }
            return lastInsertedId;
        }

        public async Task<bool> Edit(OrderHeader model)
        {
            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_EditOrderHeader", con);

                cmd.Parameters.AddWithValue("Id", model.Id);
                cmd.Parameters.AddWithValue("ApplicationUserId", model.ApplicationUserId);
                cmd.Parameters.AddWithValue("OrderDate", model.OrderDate);
                cmd.Parameters.AddWithValue("ShippingDate", model.ShippingDate != DateTime.MinValue ? (object)model.ShippingDate : DBNull.Value);
                cmd.Parameters.AddWithValue("OrderTotal", model.OrderTotal);
                cmd.Parameters.AddWithValue("OrderStatus", model.OrderStatus);
                cmd.Parameters.AddWithValue("PaymentStatus", model.PaymentStatus);
                cmd.Parameters.AddWithValue("TrackingNumber", string.IsNullOrEmpty(model.TrackingNumber) ? DBNull.Value : (object)model.TrackingNumber);
                cmd.Parameters.AddWithValue("Carrier", string.IsNullOrEmpty(model.Carrier) ? DBNull.Value : (object)model.Carrier);
                cmd.Parameters.AddWithValue("PaymentDate", model.PaymentDate != DateTime.MinValue ? (object)model.PaymentDate : DBNull.Value);
                cmd.Parameters.AddWithValue("SessionId", string.IsNullOrEmpty(model.SessionId) ? DBNull.Value : (object)model.SessionId);
                cmd.Parameters.AddWithValue("PaymentIntentId", string.IsNullOrEmpty(model.PaymentIntentId) ? DBNull.Value : (object)model.PaymentIntentId);
                cmd.Parameters.AddWithValue("PaymentDueDate", model.PaymentDueDate != DateTime.MinValue ? (object)model.PaymentDueDate : DBNull.Value);
                cmd.Parameters.AddWithValue("Name", model.Name);
                cmd.Parameters.AddWithValue("StreetAddress", model.StreetAddress);
                cmd.Parameters.AddWithValue("City", model.City);
                cmd.Parameters.AddWithValue("State", model.State);
                cmd.Parameters.AddWithValue("PostalCode", model.PostalCode);
                cmd.Parameters.AddWithValue("PhoneNumber", model.PhoneNumber);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }
                else { return false; }
            }
        }
        public async Task<List<OrderHeader>> GetListOrderHeader()
        {
            List<OrderHeader> orderHeaderList = new List<OrderHeader>();

            using (SqlConnection con = new SqlConnection(_SQLString))
            {
               await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_getListOrderHeader", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    orderHeaderList.Add(new OrderHeader
                    {
                        Id = Convert.ToInt32(dr["OrderHeaderId"]),
                        ApplicationUserId = dr["ApplicationUserId"].ToString(),
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
                            Email = dr["Email"].ToString()
                        }

                    });
                }

            }
            return orderHeaderList;
        }

        public async Task<OrderHeader> GetOrderHeader(int id)
        {
            OrderHeader orderHeader = new OrderHeader();
           
            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_getOrderHeaderById", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;   
                cmd.Parameters.AddWithValue("@OrderHeaderId", id);    
                SqlDataReader dr = await cmd.ExecuteReaderAsync();

                if (dr.HasRows)
                {
                    while (await dr.ReadAsync())
                    {
                        orderHeader = new OrderHeader
                        {
                            Id = Convert.ToInt32(dr["OrderHeaderId"]),
                            ApplicationUserId = dr["ApplicationUserId"].ToString(),
                            OrderDate = Convert.ToDateTime(dr["OrderDate"]),
                            ShippingDate = Convert.ToDateTime(dr["ShippingDate"]),
                            OrderTotal = Convert.ToDouble(dr["OrderTotal"]),
                            OrderStatus = dr["OrderStatus"].ToString(),
                            PaymentStatus = dr["PaymentStatus"].ToString(),
                            TrackingNumber = dr["TrackingNumber"].ToString(),
                            Carrier = dr["Carrier"].ToString(),
                            PaymentDate = Convert.ToDateTime(dr["PaymentDate"]),
                            PaymentDueDate = Convert.ToDateTime(dr["PaymentDueDate"]),
                            SessionId = dr["SessionId"].ToString(),
                            PaymentIntentId = dr["PaymentIntentId"].ToString(),
                            Name = dr["Name"].ToString(),
                            StreetAddress = dr["StreetAddress"].ToString(),
                            City = dr["City"].ToString(),
                            State = dr["State"].ToString(),
                            PostalCode = dr["PostalCode"].ToString(),
                            PhoneNumber = dr["PhoneNumber"].ToString(),
                            ApplicationUser = new ApplicationUser()
                            {
                                Email = dr["Email"].ToString()
                            }
                        };
                    }
                }
            }
            return orderHeader;
        }
        public async Task<bool> UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            using (SqlConnection con = new SqlConnection(_SQLString))
            {
               await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_updateStatus", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderHeaderId", id);
                cmd.Parameters.AddWithValue("@OrderStatus", orderStatus);
                cmd.Parameters.AddWithValue("@PaymentStatus", paymentStatus);

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }        
                else { return false; }
            }
        }

        public async Task<bool> UpdateStripePaymentId(int orderHeaderId, string sessionId, string paymentIntentId)
        {
            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_updateStripePaymentId", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrderHeaderId", orderHeaderId);
                cmd.Parameters.AddWithValue("@SessionId", sessionId);
                cmd.Parameters.AddWithValue("@PaymentIntentId", paymentIntentId);

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }       
                else { return false; }
            }
        }


    }
}
