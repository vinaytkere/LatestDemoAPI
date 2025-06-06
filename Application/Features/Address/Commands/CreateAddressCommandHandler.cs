/// <summary>
/// Handles creation of address records.
/// </summary>
namespace Application.Features.Address.Commands
{
    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, Result<Guid>>
    {
        private readonly IRepository<Domain.Address> _repo;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the handler with its dependencies.
        /// </summary>
        public CreateAddressCommandHandler(IRepository<Domain.Address> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Persists a new address based on the provided command.
        /// </summary>
        public async Task<Result<Guid>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            var address = _mapper.Map<Domain.Address>(request);

            await _repo.AddAsync(address);
            await _repo.SaveChangesAsync();

            return Result<Guid>.Success(address.Id);
        }
    }
}
