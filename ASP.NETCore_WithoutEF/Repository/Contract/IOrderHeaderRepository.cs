namespace ASP.NETCore_WithoutEF.Repository.Contract
{
    public interface IOrderHeaderRepository<T> where T : class
    {
        Task<int> InsertOrderHeaderInfo(T model);

        Task<List<T>> GetListOrderHeader();

        Task<T> GetOrderHeader(int id);

        Task<bool> Edit(T model);

        Task<bool> UpdateStatus(int id, string orderStatus, string? paymentStatus = null);

        Task<bool> UpdateStripePaymentId(int id, string sessionId, string paymentIntentId);
        
       

        //Task<bool> Save(T model);
        //Task<bool> Delete(int id);
    }
}
