/// <summary>
/// Handler for retrieving an address by id.
/// </summary>
namespace Application.Features.Address.Queries
{
    public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, AddressDto?>
    {
        private readonly IRepository<Domain.Address> _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes the handler with repository and mapper.
        /// </summary>
        public GetAddressByIdQueryHandler(IRepository<Domain.Address> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves the address matching the query id.
        /// </summary>
        public async Task<AddressDto?> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            return entity is null ? null : _mapper.Map<AddressDto>(entity);
        }
    }
}

