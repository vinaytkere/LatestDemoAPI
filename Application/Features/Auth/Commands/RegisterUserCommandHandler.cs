/// <summary>
/// Handles user registration logic.
/// </summary>
namespace Application.Features.Auth.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
    {
        private readonly IRepository<User> _repository;
        private readonly IPasswordHasher<User> _hasher;

        /// <summary>
        /// Creates a new instance using the repository and password hasher.
        /// </summary>
        public RegisterUserCommandHandler(IRepository<User> repository, IPasswordHasher<User> hasher)
        {
            _repository = repository;
            _hasher = hasher;
        }

        /// <summary>
        /// Registers the new user if the username is unique.
        /// </summary>
        public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var exists = (await _repository.FindAsync(u => u.UserName == request.Username)).Any();
            if (exists)
                return Result<Guid>.Failure("User already exists");

            var user = new User { UserName = request.Username };
            user.PasswordHash = _hasher.HashPassword(user, request.Password);
            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();
            return Result<Guid>.Success(user.Id);
        }
    }
}

