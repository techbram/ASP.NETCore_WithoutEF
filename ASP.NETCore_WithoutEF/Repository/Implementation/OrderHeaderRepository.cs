using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repository.Contract;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace ASP.NETCore_WithoutEF.Repository.Implementation
{
    public class OrderHeaderRepository : IOrderHeaderRepository<OrderHeader>
    {
        private readonly string? _SQLString = "";
        public OrderHeaderRepository(IConfiguration _configuration)
        {
            _SQLString = _configuration.GetConnectionString("SQLString");     //Getting connection with database

        }
        public async Task<int> InsertOrderHeaderInfo(OrderHeader model)
        {
            int lastInsertedId = 0;
            using (SqlConnection con = new SqlConnection(_SQLString))    
            {
                con.Open(); 
                SqlCommand cmd = new SqlCommand("sp_saveOrderHeader", con);  

                cmd.Parameters.AddWithValue("ApplicationUserId", model.ApplicationUserId);
                cmd.Parameters.AddWithValue("OrderDate", model.OrderDate);
                cmd.Parameters.AddWithValue("ShippingDate", model.ShippingDate);			

                cmd.Parameters.AddWithValue("OrderTotal", model.OrderTotal);
                cmd.Parameters.AddWithValue("OrderStatus", model.OrderStatus);
                cmd.Parameters.AddWithValue("PaymentStatus", model.PaymentStatus);
                cmd.Parameters.AddWithValue("TrackingNumber", string.IsNullOrEmpty(model.TrackingNumber) ? DBNull.Value : model.TrackingNumber);
                
                cmd.Parameters.AddWithValue("Carrier", string.IsNullOrEmpty(model.Carrier) ? DBNull.Value : model.Carrier);

                cmd.Parameters.AddWithValue("PaymentDate", model.PaymentDate);

                cmd.Parameters.AddWithValue("SessionId", string.IsNullOrEmpty(model.SessionId) ? DBNull.Value : (object)model.SessionId);
                cmd.Parameters.AddWithValue("PaymentIntentId", string.IsNullOrEmpty(model.PaymentIntentId) ? DBNull.Value : (object)model.PaymentIntentId);

                cmd.Parameters.AddWithValue("PaymentDueDate", model.PaymentDueDate);
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

                if(cmd.Parameters["@LastInsertedId"].Value != null)
                {
                    lastInsertedId = Convert.ToInt32(cmd.Parameters["@LastInsertedId"].Value);
                }
            }
            return lastInsertedId;
        }
        //public async Task<bool> Save(OrderHeader model)
        //{
        //    using (SqlConnection con = new SqlConnection(_SQLString))    // Create connection
        //    {
        //        con.Open();                                              // Open Connection

        //        SqlCommand cmd = new SqlCommand("sp_saveProduct", con);  // Specify command store procedure

        //        //Add parameter with value
        //        //cmd.Parameters.AddWithValue("p_id", model.Id);
        //        cmd.Parameters.AddWithValue("ApplicationUserId", model.ApplicationUserId);
        //        cmd.Parameters.AddWithValue("OrderDate", model.OrderDate);
        //        cmd.Parameters.AddWithValue("ShippingDate", model.ShippingDate);

        //        cmd.Parameters.AddWithValue("OrderTotal", model.OrderTotal);
        //        cmd.Parameters.AddWithValue("OrderStatus", model.OrderStatus);
        //        cmd.Parameters.AddWithValue("PaymentStatus", model.PaymentStatus);
        //        cmd.Parameters.AddWithValue("TrackingNumber", string.IsNullOrEmpty(model.TrackingNumber) ? DBNull.Value : model.TrackingNumber);

        //        cmd.Parameters.AddWithValue("Carrier", string.IsNullOrEmpty(model.Carrier) ? DBNull.Value : model.Carrier);

        //        cmd.Parameters.AddWithValue("PaymentDate", model.PaymentDate);

        //        cmd.Parameters.AddWithValue("SessionId", string.IsNullOrEmpty(model.SessionId) ? DBNull.Value : (object)model.SessionId);
        //        cmd.Parameters.AddWithValue("PaymentIntentId", string.IsNullOrEmpty(model.PaymentIntentId) ? DBNull.Value : (object)model.PaymentIntentId);

        //        cmd.Parameters.AddWithValue("PaymentDueDate", model.PaymentDueDate);
        //        cmd.Parameters.AddWithValue("Name", model.Name);
        //        cmd.Parameters.AddWithValue("StreetAddress", model.StreetAddress);
        //        cmd.Parameters.AddWithValue("City", model.City);
        //        cmd.Parameters.AddWithValue("State", model.State);
        //        cmd.Parameters.AddWithValue("PostalCode", model.PostalCode);
        //        cmd.Parameters.AddWithValue("PhoneNumber", model.PhoneNumber);

        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //        int affected_rows = await cmd.ExecuteNonQueryAsync();

        //        if (affected_rows > 0) { return true; }
        //        else { return false; }
        //    }
        //}
        //public async Task<bool> Edit(OrderHeader model)                   //Update OrderHerder
        //{
        //    using (SqlConnection con = new SqlConnection(_SQLString))    // Create connection
        //    {
        //        con.Open();                                              // Open Connection

        //        SqlCommand cmd = new SqlCommand("sp_EditOrderHeader", con);  // Specify command store procedure

        //        //Add parameter with value

        //        cmd.Parameters.AddWithValue("Id", model.Id);
        //        cmd.Parameters.AddWithValue("ApplicationUserId", model.ApplicationUserId);
        //        cmd.Parameters.AddWithValue("OrderDate", model.OrderDate);

        //        cmd.Parameters.AddWithValue("ShippingDate", model.ShippingDate);				

        //        cmd.Parameters.AddWithValue("OrderTotal", model.OrderTotal);
        //        cmd.Parameters.AddWithValue("OrderStatus", model.OrderStatus);
        //        cmd.Parameters.AddWithValue("PaymentStatus", model.PaymentStatus);

        //        cmd.Parameters.AddWithValue("TrackingNumber", string.IsNullOrEmpty(model.TrackingNumber) ? DBNull.Value : (object)model.TrackingNumber);

        //        cmd.Parameters.AddWithValue("Carrier", string.IsNullOrEmpty(model.Carrier) ? DBNull.Value : (object)model.Carrier);
        //        cmd.Parameters.AddWithValue("PaymentDate",model.PaymentDate != DateTime.MinValue ? (object)model.PaymentDate : DBNull.Value);
        //        cmd.Parameters.AddWithValue("SessionId", string.IsNullOrEmpty(model.SessionId) ? DBNull.Value : (object)model.SessionId);
        //        cmd.Parameters.AddWithValue("PaymentIntentId", string.IsNullOrEmpty(model.PaymentIntentId) ? DBNull.Value : (object)model.PaymentIntentId);
        //        cmd.Parameters.AddWithValue("PaymentDueDate", model.PaymentDueDate);
        //        cmd.Parameters.AddWithValue("Name", model.Name);
        //        cmd.Parameters.AddWithValue("StreetAddress", model.StreetAddress);
        //        cmd.Parameters.AddWithValue("City", model.City);
        //        cmd.Parameters.AddWithValue("State", model.State);
        //        cmd.Parameters.AddWithValue("PostalCode", model.PostalCode);
        //        cmd.Parameters.AddWithValue("PhoneNumber", model.PhoneNumber);

        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;


        //        int affected_rows = await cmd.ExecuteNonQueryAsync();

        //        if (affected_rows > 0) { return true; }
        //        else { return false; }
        //    }
        //}
        public async Task<List<OrderHeader>> GetListOrderHeader()
        {
            List<OrderHeader> orderHeaderList = new List<OrderHeader>();                // Instantiate object(Category)  

            using (SqlConnection con = new SqlConnection(_SQLString))         // Create connection
            {
                con.Open();                                                   //Open Connection

                SqlCommand cmd = new SqlCommand("sp_getListOrderHeader", con);   // Specify command (Stored Procedure)

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    //read stored data and add to the list

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
                            ApplicationUser = new ApplicationUser() { 
                                Email = dr["Email"].ToString()
                            }

                        });
                    }
                }
            }
            return orderHeaderList;
        }

        public async Task<OrderHeader> GetOrderHeader(int id)
        {
            OrderHeader orderHeader = new OrderHeader();
            //Product product = null;             // return null if no prodduct is found

            using (SqlConnection con = new SqlConnection(_SQLString))
            {
                //con.Open();
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_getDetailOrderHeader", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;   //Specify command type                
                cmd.Parameters.AddWithValue("@OrderHeaderId", id);    //  the stored procedure takes a parameter like @p_id

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
                            PhoneNumber = dr["PhoneNumber"].ToString()
                        };
                    }
                }
            }
            return orderHeader;
        }
        public async Task<bool> UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            using (SqlConnection conn = new SqlConnection(_SQLString))  
            {
                conn.Open();                                            

                SqlCommand cmd = new SqlCommand("sp_updateStatus", conn);  

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add parameter with value
                cmd.Parameters.AddWithValue("@OrderHeaderId", id);
                cmd.Parameters.AddWithValue("@OrderStatus", orderStatus);
                cmd.Parameters.AddWithValue("@PaymentStatus", paymentStatus);

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }        // return type is bool(true OR false) 
                else { return false; }
            }
        }

        public async Task<bool> UpdateStripePaymentId(int orderHeaderId, string sessionId, string paymentIntentId)
        {
            using (SqlConnection conn = new SqlConnection(_SQLString))   
            {
                conn.Open();                                            

                SqlCommand cmd = new SqlCommand("sp_updateStripePaymentId", conn);  

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add parameter with value
                cmd.Parameters.AddWithValue("@OrderHeaderId", orderHeaderId);
                cmd.Parameters.AddWithValue("@SessionId", sessionId);
                cmd.Parameters.AddWithValue("@PaymentIntentId", paymentIntentId);

                int affected_rows = await cmd.ExecuteNonQueryAsync();

                if (affected_rows > 0) { return true; }        // return type is bool(true OR false) 
                else { return false; }
            }
        }

      
    }
}
