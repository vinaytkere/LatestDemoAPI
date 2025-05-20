namespace Application.Features.Address.Commands
{
    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, Result>
    {
        private readonly IRepository<Domain.Address> _repository;

        public DeleteAddressCommandHandler(IRepository<Domain.Address> repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var address = await _repository.GetByIdAsync(request.Id);
            if (address == null)
                return Result.Failure("Address not found");

            _repository.Remove(address);
            await _repository.SaveChangesAsync();
            return Result.Success();
        }
    }
}