namespace ASP.NETCore_WithoutEF.Repositories.Contract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> List();

        Task<bool> Save(T model);

        Task<bool> Edit(T model);

        Task<bool> Delete(int id);

    }
    
}
