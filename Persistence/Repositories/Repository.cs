/// <summary>
/// Generic repository implementation using Entity Framework Core.
/// </summary>
namespace Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        /// <summary>
        /// Creates a new repository for a given context.
        /// </summary>
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        /// <summary>
        /// searches for entities by name. Assumes the entity has a 'Name' property.
        /// </summary>
        public async Task<IEnumerable<T>> SearchByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Enumerable.Empty<T>();

            var term = name.Trim().ToLower();

            if (typeof(T) == typeof(Address))
            {
                var query = _context.Addresses
                    .Where(a =>
                        a.City.ToLower().Contains(term) ||
                        a.State.ToLower().Contains(term) ||
                        a.Country.ToLower().Contains(term) ||
                        (a.LandMark != null && a.LandMark.ToLower().Contains(term)));

                var results = await query.ToListAsync();
                return results.Cast<T>();
            }

            return await _dbSet
                .Where(e => EF.Property<string>(e, "Name") != null &&
                            EF.Property<string>(e, "Name").ToLower().Contains(term))
                .ToListAsync();
        }
        /// <summary>
        /// Fetches an entity by its id.
        /// </summary>
        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Returns all entities.
        /// </summary>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Returns entities matching a predicate.
        /// </summary>
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Adds a new entity to the context.
        /// </summary>
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        /// <summary>
        /// Updates an entity in the context.
        /// </summary>
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        /// <summary>
        /// Removes an entity from the context.
        /// </summary>
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Persists changes to the database.
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
