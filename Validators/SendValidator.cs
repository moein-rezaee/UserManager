using FluentValidation;
using UserManager.DTOs;

namespace OTPService.Validations
{
    public class SendValidator : AbstractValidator<SendDto>
    {
        public SendValidator()
        {
            RuleFor(d => d.Username).NotEmpty().NotNull();
            RuleFor(i => i.OrganizationId).NotEmpty().NotNull();
        }
    }
}