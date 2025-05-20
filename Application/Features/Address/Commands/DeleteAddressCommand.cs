namespace Application.Features.Address.Commands
{
    public class DeleteAddressCommand : IRequest<Result>
    {
        public Guid Id { get; set; }

        public DeleteAddressCommand(Guid id)
        {
            Id = id;
        }
    }
}
