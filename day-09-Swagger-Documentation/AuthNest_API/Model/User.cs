namespace AuthNest_API.Model
{
    public class User
    {

        public int Id { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public bool IsEmailConfirmed { get; set; } = false;

        public string? EmailConfirmationToken { get; set; }

    }
}
