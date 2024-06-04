namespace UserManager.ViewModels
{
    public class VerifyViewModel
    {
        public required Guid OrganizationId { get; set; }
        public required string Username { get; set; }
    }
}