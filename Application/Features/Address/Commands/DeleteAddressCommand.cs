/// <summary>
/// Command to delete an existing address.
/// </summary>
namespace Application.Features.Address.Commands
{
    public class DeleteAddressCommand : IRequest<Result>
    {
        /// <summary>
        /// Identifier of the address to delete.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Creates a new instance for deletion.
        /// </summary>
        public DeleteAddressCommand(Guid id)
        {
            Id = id;
        }
    }
}

