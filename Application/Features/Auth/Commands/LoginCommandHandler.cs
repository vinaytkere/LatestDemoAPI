namespace Application.Features.Auth.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<string>>
    {
        private readonly IRepository<User> _repository;
        private readonly IPasswordHasher<User> _hasher;
        private readonly IJwtTokenGenerator _tokenGenerator;

        public LoginCommandHandler(
            IRepository<User> repository,
            IPasswordHasher<User> hasher,
            IJwtTokenGenerator tokenGenerator)
        {
            _repository = repository;
            _hasher = hasher;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = (await _repository.FindAsync(u => u.UserName == request.Username)).FirstOrDefault();
            if (user == null)
                return Result<string>.Failure("Invalid credentials");

            var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (verify == PasswordVerificationResult.Failed)
                return Result<string>.Failure("Invalid credentials");

            var token = _tokenGenerator.GenerateToken(user);
            return Result<string>.Success(token);
        }
    }
}
