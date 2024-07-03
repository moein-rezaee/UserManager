namespace UserManager.DTOs
{
    public class EmailVerifyCodeDto
    {
        public required Guid OrganizationId { get; set; }
        public required string Email { get; set; }
        public required string Code { get; set; }
    }
}