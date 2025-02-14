using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repository.Contract;

namespace ASP.NETCore_WithoutEF.Repository.Implementation
{
    public class OrderDetailRepository : IGenericRepository<OrderDetail>
    {
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

        public Task<bool> Save(OrderDetail model)
        {
            throw new NotImplementedException();
        }
    }
}
