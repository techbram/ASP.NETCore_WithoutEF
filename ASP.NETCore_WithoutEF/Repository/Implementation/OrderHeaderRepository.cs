using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repository.Contract;

namespace ASP.NETCore_WithoutEF.Repository.Implementation
{
    public class OrderHeaderRepository : IGenericRepository<OrderHeader>
    {
        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderHeader> Detail(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Edit(OrderHeader model)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderHeader>> List()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Save(OrderHeader model)
        {
            throw new NotImplementedException();
        }
    }
}
