namespace UserManager.DTOs
{
    public class VerifyDto
    {
        public required Guid OrganizationId { get; set; }
        public required string Username { get; set; }
        public required string Code { get; set; }
    }
}