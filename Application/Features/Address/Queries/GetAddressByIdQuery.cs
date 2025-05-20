namespace Application.Features.Address.Queries
{
    public class GetAddressByIdQuery : IRequest<AddressDto?>
    {
        public Guid Id { get; set; }

        public GetAddressByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
