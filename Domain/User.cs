namespace Domain
{
    public class User : BaseEntity
    {
        public string UserName { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
    }
}
