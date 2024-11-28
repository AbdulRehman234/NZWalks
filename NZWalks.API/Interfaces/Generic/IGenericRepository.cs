namespace NZWalks.API.Interfaces.Generic
{
    public interface IGenericRepository<T> where T :  class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(Guid id ,T entity);
        Task<T> DeleteAsync(Guid id);
        IQueryable<T> Query();
    }
}
