/// <summary>
/// Application user with authentication details.
/// </summary>
namespace Domain
{
    public class User : BaseEntity
    {
        /// <summary>
        /// Unique username for the user.
        /// </summary>
        public string UserName { get; set; } = default!;
        /// <summary>
        /// Hashed password for secure storage.
        /// </summary>
        public string PasswordHash { get; set; } = default!;
        /// <summary>
        /// Role assigned to the user.
        /// </summary>
        public string Role { get; set; } = "User";
    }
}

