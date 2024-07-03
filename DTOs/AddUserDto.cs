namespace UserManager.DTOs
{
    public class AddUserDto
    {
        public Guid OrganizationId { get; set; }
        public required string Phone { get; set; }
        public required string Username { get; set; }
        public string? Email { get; set; }
        public required string Password { get; set; }
    }
}