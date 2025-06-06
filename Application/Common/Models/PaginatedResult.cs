/// <summary>
/// Helper container for paginated query results.
/// </summary>
namespace Application.Common.Models
{
    public class PaginatedResult<T>
    {
        /// <summary>
        /// The subset of items for the requested page.
        /// </summary>
        public IEnumerable<T> Items { get; set; } = [];
        /// <summary>
        /// Total number of items available.
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// Current page number.
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Size of the page requested.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Computed number of total pages.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}

