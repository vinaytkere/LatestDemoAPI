/// <summary>
/// Query parameters for retrieving a paged list of addresses.
/// </summary>
namespace Application.Features.Address.Queries
{
    public class GetAllAddressesQuery : IRequest<PaginatedResult<AddressDto>>
    {
        /// <summary>
        /// Page number being requested.
        /// </summary>
        public int PageNumber { get; set; } = 1;
        /// <summary>
        /// Number of items per page.
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Optional city filter.
        /// </summary>
        public string? City { get; set; }
        /// <summary>
        /// Optional state filter.
        /// </summary>
        public string? State { get; set; }
    }
}
