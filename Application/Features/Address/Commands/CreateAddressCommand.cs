/// <summary>
/// Command used to create a new address.
/// </summary>
namespace Application.Features.Address.Commands
{
    public class CreateAddressCommand : IRequest<Result<Guid>>
    {
        /// <summary>
        /// Country portion of the new address.
        /// </summary>
        public string Country { get; set; } = default!;
        /// <summary>
        /// State or province.
        /// </summary>
        public string State { get; set; } = default!;
        /// <summary>
        /// Name of the city.
        /// </summary>
        public string City { get; set; } = default!;
        /// <summary>
        /// Postal code.
        /// </summary>
        public string PinCode { get; set; } = default!;
        /// <summary>
        /// Optional landmark information.
        /// </summary>
        public string? LandMark { get; set; }
    }

    /// <summary>
    /// Validator ensuring the create command has valid data.
    /// </summary>
    public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
    {
        /// <summary>
        /// Configure validation rules for the create command.
        /// </summary>
        public CreateAddressCommandValidator()
        {
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required");
            RuleFor(x => x.State).NotEmpty().WithMessage("State is required");
            RuleFor(x => x.City).NotEmpty().WithMessage("City is required");
            RuleFor(x => x.PinCode)
                .NotEmpty().WithMessage("PinCode is required")
                .Matches(@"^\d{5,6}$").WithMessage("PinCode must be 5 or 6 digits");

            RuleFor(x => x.LandMark).MaximumLength(100).WithMessage("Landmark too long");
        }
    }
}
