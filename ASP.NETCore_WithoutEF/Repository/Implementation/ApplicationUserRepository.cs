using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repository.Contract;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;

namespace ASP.NETCore_WithoutEF.Repository.Implementation
{
	public class ApplicationUserRepository : IGenericRepository<ApplicationUser>
	{

		private readonly string? _SQLString = "";
		public ApplicationUserRepository(IConfiguration _configuration)
		{
			_SQLString = _configuration.GetConnectionString("SQLString");     //Getting connection with database

		}
		public async Task<List<ApplicationUser>> List()
		{
			List<ApplicationUser> user = new List<ApplicationUser>();                // Instantiate object(Category)  

			using (SqlConnection con = new SqlConnection(_SQLString))         // Create connection
			{
				con.Open();                                                   //Open Connection

				SqlCommand cmd = new SqlCommand("sp_getApplicationUser", con);   // Specify command (Stored Procedure)

				cmd.CommandType = System.Data.CommandType.StoredProcedure;

				using (var dr = await cmd.ExecuteReaderAsync())
				{
					//read stored data and add to the list

					while (await dr.ReadAsync())
					{
						user.Add(new ApplicationUser
						{
							Id = dr["ApplicationUserId"].ToString(),
							UserName = dr["UserName"].ToString(),
							Email = dr["Email"].ToString(),
							PasswordHash = dr["PasswordHash"].ToString(),
							Name = dr["Name"].ToString(),
							StreetAddress = dr["StreetAddress"].ToString(),
							City = dr["City"].ToString(),
							State = dr["State"].ToString(),
							PostalCode = dr["PostalCode"].ToString(),
							PhoneNumber = dr["PhoneNumber"].ToString(),
							//CompanyId = Convert.ToInt32(dr["CompanyId"])
						});
					}
				}
			}
			return user;
		}
		

		public async Task<ApplicationUser> Detail(int id)
		{
			ApplicationUser user = new ApplicationUser();

			using (SqlConnection con = new SqlConnection(_SQLString))
			{
				await con.OpenAsync();

				using (SqlCommand cmd = new SqlCommand("sp_detailUser", con))
				{
					cmd.CommandType = System.Data.CommandType.StoredProcedure;

					cmd.Parameters.AddWithValue("@UserId", id);

					// Use a using block for SqlDataReader to ensure it is properly disposed
					using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
					{
						if (dr.HasRows)
						{
							while (await dr.ReadAsync())
							{
								// Populate the cart object with the data
								user = new ApplicationUser
								{
									Id = dr["ApplicationUserId"].ToString(), // Ensure correct column name
									Name = dr["Name"].ToString(),
									StreetAddress = dr["StreetAddress"].ToString(),
									City = dr["City"].ToString(),
									State = dr["State"].ToString(),
									PostalCode = dr["PostalCode"].ToString(),
									PhoneNumber = dr["PhoneNumber"].ToString(),
								};
							}
						}
					}
				}
			}

			return user; // Return null if no rows were found.
		}

		public Task<bool> Edit(ApplicationUser model)
		{
			throw new NotImplementedException();
		}

		public Task<bool> Save(ApplicationUser model)
		{
			throw new NotImplementedException();
		}
		public Task<bool> Delete(int id)
		{
			throw new NotImplementedException();
		}
	}
}
