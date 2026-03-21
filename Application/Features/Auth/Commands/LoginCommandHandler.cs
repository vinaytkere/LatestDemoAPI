/// <summary>
/// Handles user login and token issuance.
/// </summary>
namespace Application.Features.Auth.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<string>>
    {
        private readonly IRepository<User> _repository;
        private readonly IPasswordHasher<User> _hasher;
        private readonly IJwtTokenGenerator _tokenGenerator;

        /// <summary>
        /// Constructs the handler with required services.
        /// </summary>
        public LoginCommandHandler(
            IRepository<User> repository,
            IPasswordHasher<User> hasher,
            IJwtTokenGenerator tokenGenerator)
        {
            _repository = repository;
            _hasher = hasher;
            _tokenGenerator = tokenGenerator;
        }

        /// <summary>
        /// Validates credentials and returns a JWT on success.
        /// </summary>
        public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return Result<string>.Failure("Username and password are required");
            }

            var user = _repository.FindAsync(
                u => u.UserName.ToLower() == request.Username.ToLower()).Result.FirstOrDefault();

            if (user == null)
                return Result<string>.Failure("Invalid credentials");

            var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (verify != PasswordVerificationResult.Success)
                return Result<string>.Failure("Invalid credentials");

            var token = _tokenGenerator.GenerateToken(user);

            return Result<string>.Success(token);
        }
    }
}

