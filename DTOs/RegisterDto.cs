namespace UserManager.DTOs
{
    public class RegisterDto
    {
        public Guid OrganizationId { get; set; }
        public required string Phone { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public required string Password { get; set; }
        public required string RepPassword { get; set; }
    }
}