using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repository.Contract;
using Microsoft.Data.SqlClient;

namespace ASP.NETCore_WithoutEF.Repository.Implementation
{
	public class OrderDetailRepository : IOrderDetailRepository<OrderDetail>
	{
		private readonly string? _SQLString = "";
		public OrderDetailRepository(IConfiguration _configuration)
		{
			_SQLString = _configuration.GetConnectionString("SQLString");     //Getting connection with database

		}
        public async Task<List<OrderDetail>> GetListOrderDetail()
        {
            List<OrderDetail> listOrderDetail = new List<OrderDetail>();                // Instantiate object(Category)  

            using (SqlConnection con = new SqlConnection(_SQLString))         // Create connection
            {
                con.Open();                                                   //Open Connection

                SqlCommand cmd = new SqlCommand("sp_getListOrderDetail", con);   // Specify command (Stored Procedure)

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    //read stored data and add to the list

                    while (await dr.ReadAsync())
                    {
                        listOrderDetail.Add(new OrderDetail
                        {
                            Id = Convert.ToInt32(dr["OrderDetailId"]),
                            OrderHeader = new OrderHeader()
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
                                PhoneNumber = dr["PhoneNumber"].ToString()
                            },
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            Count = Convert.ToInt32(dr["Count"]),
                            Price = Convert.ToDouble(dr["Price"])
                        });
                    }
                }
            }
            return listOrderDetail;
        }
    
        public async Task<bool> Save(OrderDetail model)
		{
			using (SqlConnection con = new SqlConnection(_SQLString))    // Create connection
			{
				con.Open();                                              // Open Connection

				SqlCommand cmd = new SqlCommand("sp_saveOrderDetail", con);  // Specify command store procedure

				//Add parameter with value

				//cmd.Parameters.AddWithValue("Id", model.Id);
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

		public Task<OrderDetail> GetOrderDetail(int id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> Edit(OrderDetail model)
		{
			throw new NotImplementedException();
		}

		
		
	}
}
