/// <summary>
/// Abstraction for a basic repository handling CRUD operations.
/// </summary>
namespace Application.Interfaces
{
    public interface IRepository
    {
        public interface IRepository<T> where T : class
        {
            /// <summary>
            /// Retrieve an entity by its identifier.
            /// </summary>
            Task<T?> GetByIdAsync(Guid id);

            Task<IEnumerable<T>> SearchByNameAsync(string name);
            /// <summary>
            /// Retrieve all entities.
            /// </summary>
            Task<IEnumerable<T>> GetAllAsync();
            /// <summary>
            /// Find entities matching the provided predicate.
            /// </summary>
            Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
            /// <summary>
            /// Add a new entity to the data store.
            /// </summary>
            Task AddAsync(T entity);
            /// <summary>
            /// Update an existing entity.
            /// </summary>
            void Update(T entity);
            /// <summary>
            /// Delete an entity from the data store.
            /// </summary>
            void Remove(T entity);
            /// <summary>
            /// Persist changes to the data store.
            /// </summary>
            Task SaveChangesAsync();
        }
    }
}

