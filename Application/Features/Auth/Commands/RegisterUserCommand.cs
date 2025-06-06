/// <summary>
/// Command to register a new user.
/// </summary>
namespace Application.Features.Auth.Commands
{
    public class RegisterUserCommand : IRequest<Result<Guid>>
    {
        /// <summary>
        /// Desired username for the new user.
        /// </summary>
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// Password for the new account.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Validates the new user registration request.
    /// </summary>
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        /// <summary>
        /// Configure validation rules for registration.
        /// </summary>
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");
        }
    }
}

