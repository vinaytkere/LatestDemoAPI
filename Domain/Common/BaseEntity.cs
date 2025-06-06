/// <summary>
/// Base type for all entities providing simple audit fields.
/// </summary>
namespace Domain.Common
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// Primary key for the entity.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// UTC timestamp for when the entity was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// UTC timestamp for the last update to the entity.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
