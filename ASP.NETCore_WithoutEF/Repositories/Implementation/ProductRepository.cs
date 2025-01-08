using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repositories.Contract;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ASP.NETCore_WithoutEF.Repositories.Implementation
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

                SqlCommand cmd = new SqlCommand("sp_listProducts",con);  // Specify command store procedure

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Read stored data
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        //Employee objEmployee = new Employee();
                        //objEmployee.id = Convert.ToInt32(dr["e_id"]);
                        //objEmployee.name = dr["e_name"].ToString();

                        //    Department objDepartment = new Department();
                        //    objDepartment.id = Convert.ToInt32(dr["d_id"]);
                        //    objDepartment.name = dr["d_name"].ToString();
                        //    objEmployee.departmentRef = objDepartment;

                        //objEmployee.salary = Convert.ToInt32(dr["salary"]);
                        //objEmployee.contractDate = dr["contract_date"].ToString();

                        //productList.Add(objEmployee);


                        productList.Add(                        //Add product in the product list
                            new Product
                            {
                                Id = Convert.ToInt32(dr["p_id"]),
                                ProductName = dr["p_name"].ToString(),
                                CategoryRef = new Category()
                                {
                                    Id = Convert.ToInt32(dr["c_id"]),
                                    Name = dr["c_name"].ToString()
                                },
                                Price = Convert.ToInt32(dr["Price"])
                                
                            });
                    }

                }

            }
                return productList;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Edit(Product model)
        {
            throw new NotImplementedException();
        }

       
        public Task<bool> Save(Product model)
        {
            throw new NotImplementedException();
        }
    }
}
