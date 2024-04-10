namespace HMS.Services.Repository_Service
{
    public interface IRepositoryService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(long id);
        Task InsertAsync(T dbObject);
        void Update(T dbObject);
        Task DeleteAsync(T dbObject);
        Task SaveAsync();
        Task<bool> ItemExistsAsync(long id);
    }
}
