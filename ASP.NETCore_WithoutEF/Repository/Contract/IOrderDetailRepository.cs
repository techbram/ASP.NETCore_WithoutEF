using ASP.NETCore_WithoutEF.Models;

namespace ASP.NETCore_WithoutEF.Repository.Contract
{
    public interface IOrderDetailRepository<T> where T : class
    {
        Task<bool> Save(T model);

        Task<bool> Edit(T model);

        Task<bool> Delete(int id);
      
        Task<IEnumerable<OrderDetail>> GetOrderDetail(int orderHeaderId);
    }
}
