/// <summary>
/// Query to retrieve a single address by identifier.
/// </summary>
namespace Application.Features.Address.Queries
{
    public class GetAddressByIdQuery : IRequest<AddressDto?>
    {
        /// <summary>
        /// Identifier of the address to retrieve.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Initializes the query with the specified id.
        /// </summary>
        public GetAddressByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}

