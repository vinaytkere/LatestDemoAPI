using Domain;

/// <summary>
/// Service for creating JWT tokens for authenticated users.
/// </summary>
namespace Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// Generate a token for the specified user.
        /// </summary>
        string GenerateToken(User user);
    }
}

