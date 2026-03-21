/// <summary>
/// Command to authenticate a user and retrieve a token.
/// </summary>
namespace Application.Features.Auth.Commands
{
    public class LoginCommand : IRequest<Result<string>>
    {
        /// <summary>
        /// Username of the user attempting to log in.
        /// </summary>
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// Password for the login attempt.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Validates the login command properties.
    /// </summary>
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        /// <summary>
        /// Set up validation rules for login.
        /// </summary>
        public LoginCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required"); ;

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required"); ;
        }
    }
}

