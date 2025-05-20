namespace Application.Features.Address.Commands
{
    public class CreateAddressCommand : IRequest<Result<Guid>>
    {
        public string Country { get; set; } = default!;
        public string State { get; set; } = default!;
        public string City { get; set; } = default!;
        public string PinCode { get; set; } = default!;
        public string? LandMark { get; set; }
    }

    public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
    {
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