namespace ASP.NETCore_WithoutEF.Repository.Contract
{
    public interface IOrderDetailRepository<T> where T : class
    {
        Task<List<T>> GetListOrderDetail();

        Task<bool> Save(T model);

        Task<bool> Edit(T model);

        Task<bool> Delete(int id);
        Task<T> GetOrderDetail(int id);
    }
}
