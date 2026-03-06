namespace EvaluateItEasily.Core.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> AddAsync(T entity,CancellationToken cancellationToken=default!);
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default!);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default!);
        void Update(T entity);
        void Delete(T entity);
    }
}
