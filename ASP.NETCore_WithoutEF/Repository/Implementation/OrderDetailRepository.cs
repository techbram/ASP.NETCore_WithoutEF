using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repository.Contract;
using Microsoft.Data.SqlClient;

namespace ASP.NETCore_WithoutEF.Repository.Implementation
{
    public class OrderDetailRepository : IGenericRepository<OrderDetail>
    {
		private readonly string? _SQLString = "";
		public OrderDetailRepository(IConfiguration _configuration)
		{
			_SQLString = _configuration.GetConnectionString("SQLString");     //Getting connection with database

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

        public Task<OrderDetail> Detail(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Edit(OrderDetail model)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderDetail>> List()
        {
            throw new NotImplementedException();
        }

       
    }
}
