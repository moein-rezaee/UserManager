using UserManager.Enums;

namespace UserManager.DTOs
{
    public class LoginDto
    {
        public Guid OrganizationId { get; set; }
        public required string Username { get; set; }

        public LoginType Type { get; set; } = LoginType.Password;

        public required string UniqueId { get; set; }
        public required SessionPlatform Platform { get; set; } = SessionPlatform.Web;
        public string? Password { get; set; }
    }
}