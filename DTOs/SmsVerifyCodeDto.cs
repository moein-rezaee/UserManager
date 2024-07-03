namespace UserManager.DTOs
{
    public class SmsVerifyCodeDto
    {
        public required Guid OrganizationId { get; set; }
        public required string Mobile { get; set; }
        public required string Code { get; set; }
    }
}