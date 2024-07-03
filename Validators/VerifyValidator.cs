using FluentValidation;
using UserManager.DTOs;
using UserManager.Enums;

namespace UserManager.Validations
{
    public class VerifyValidator : AbstractValidator<VerifyDto>
    {
        public VerifyValidator()
        {
            RuleFor(i => i.Type).IsInEnum();

            RuleFor(i => i.Code)
                .NotEmpty()
                .NotNull()
                .Length(4, 4);

            RuleFor(i => i.OrganizationId)
                .NotEmpty()
                .NotNull();

            RuleFor(i => i.Username)
                .NotEmpty()
                .NotNull()

                .Matches(@"^(\+98|0)?9\d{9}$")
                .WithMessage("Phone number is Not Valid!")
                .When(i => i.Type == VerifyType.Phone)

                .EmailAddress()
                .When(i => i.Type == VerifyType.Email);
        }

    }
}