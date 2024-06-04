using FluentValidation;
using OTPService.DTOs;

namespace OTPService.Validations
{
    public class SendValidator : AbstractValidator<SendDto>
    {
        public SendCodeValidator()
        {
            RuleFor(d => d.Username).NotEmpty().NotNull();
            RuleFor(i => i.OrganizationId).NotEmpty().NotNull();
        }
    }
}