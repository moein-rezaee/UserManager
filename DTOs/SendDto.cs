namespace UserManager.DTOs
{
    public class SendDto()
    {
        public Guid OrganizationId { get; set; }
        public required string Username { get; set; }
    }
}