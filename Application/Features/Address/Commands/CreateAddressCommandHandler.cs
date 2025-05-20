namespace Application.Features.Address.Commands
{
    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, Result<Guid>>
    {
        private readonly IRepository<Domain.Address> _repo;
        private readonly IMapper _mapper;

        public CreateAddressCommandHandler(IRepository<Domain.Address> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            var address = _mapper.Map<Domain.Address>(request);

            await _repo.AddAsync(address);
            await _repo.SaveChangesAsync();

            return Result<Guid>.Success(address.Id);
        }
    }
}