namespace UserManager.DTOs
{
    public class GenerateTokenDto
    {
        public required Guid OrganizationId { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
