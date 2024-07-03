using UserManager.Enums;

namespace UserManager.DTOs
{
    public class GenerateTokenDto
    {
        public required Guid OrganizationId { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }

        public required string UniqueId { get; set; }
        public required SessionPlatform Platform { get; set; }

    }
}
