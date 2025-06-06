/// <summary>
/// Command to update an existing address.
/// </summary>
namespace Application.Features.Address.Commands
{
    public class UpdateAddressCommand : IRequest<Result>
    {
        /// <summary>
        /// Identifier of the address to update.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Country of the address.
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
    /// Validator ensuring the update command has valid data.
    /// </summary>
    public class UpdateAddressCommandValidator : AbstractValidator<UpdateAddressCommand>
    {
        /// <summary>
        /// Configure validation rules for the update command.
        /// </summary>
        public UpdateAddressCommandValidator()
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

