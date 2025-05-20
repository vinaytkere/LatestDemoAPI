namespace Application.Features.Address.Commands
{
    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, Result>
    {
        private readonly IRepository<Domain.Address> _repository;
        private readonly IMapper _mapper;
        public UpdateAddressCommandHandler(IRepository<Domain.Address> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing is null)
                return Result.Failure("Address not found");

            _mapper.Map(request, existing); // updates the entity in-place

            _repository.Update(existing);
            await _repository.SaveChangesAsync();

            return Result.Success();
        }
    }
}