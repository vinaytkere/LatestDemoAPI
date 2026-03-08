/// <summary>
/// Handles retrieval of paged address lists.
/// </summary>
namespace Application.Features.Address.Queries
{
    public class GetAllAddressesQueryHandler : IRequestHandler<GetAllAddressesQuery, PaginatedResult<AddressDto>>
    {
        private readonly IRepository<Domain.Address> _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Sets up the handler with required services.
        /// </summary>
        public GetAllAddressesQueryHandler(IRepository<Domain.Address> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a page of addresses optionally filtered by city/state.
        /// </summary>
        public async Task<PaginatedResult<AddressDto>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
        {
            var all = await _repository.GetAllAsync();

            // Optional filtering
            if (!string.IsNullOrWhiteSpace(request.City))
                all = all.Where(a => a.City.Contains(request.City, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(request.State))
                all = all.Where(a => a.State.Contains(request.State, StringComparison.OrdinalIgnoreCase));

            var total = all.Count();

            var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
            var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            var paged = all
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dto = _mapper.Map<IEnumerable<AddressDto>>(paged);

            return new PaginatedResult<AddressDto>
            {
                Items = dto,
                TotalCount = total,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}

