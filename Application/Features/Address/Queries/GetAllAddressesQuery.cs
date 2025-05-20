namespace Application.Features.Address.Queries
{
    public class GetAllAddressesQuery : IRequest<PaginatedResult<AddressDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? City { get; set; }
        public string? State { get; set; }
    }
}