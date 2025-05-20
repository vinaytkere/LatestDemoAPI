namespace Application.Features.Address.Queries
{
    public class GetAllAddressesQueryHandler : IRequestHandler<GetAllAddressesQuery, PaginatedResult<AddressDto>>
    {
        private readonly IRepository<Domain.Address> _repository;
        private readonly IMapper _mapper;

        public GetAllAddressesQueryHandler(IRepository<Domain.Address> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<AddressDto>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
        {
            var all = await _repository.GetAllAsync();

            // Optional filtering
            if (!string.IsNullOrWhiteSpace(request.City))
                all = all.Where(a => a.City.Contains(request.City, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(request.State))
                all = all.Where(a => a.State.Contains(request.State, StringComparison.OrdinalIgnoreCase));

            var total = all.Count();

            var paged = all
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
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
