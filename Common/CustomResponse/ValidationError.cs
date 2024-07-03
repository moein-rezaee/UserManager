namespace UserManager.Common.CustomResponse
{
    public class ValidationError
    {
        public required string PropertyName { get; set; }
        public required string ErrorMessage { get; set; }
        public required string ErrorCode { get; set; }
    }
}