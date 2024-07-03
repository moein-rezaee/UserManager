namespace UserManager.DTOs
{
    public class EmailCodeDto()
    {
        public Guid OrganizationId { get; set; }
        public required string Email { get; set; }
    }
}