using EvaluateItEasily.Core.Contracts;
namespace EvaluateItEasily.Infrastructure
{
    using Microsoft.EntityFrameworkCore;

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default!)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default!)
        {
            return await _dbSet.FindAsync(id,cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default!)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}