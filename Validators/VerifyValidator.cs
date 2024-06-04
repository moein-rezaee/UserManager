using FluentValidation;
using UserManager.DTOs;

namespace UserManager.Validations
{
    public class VerifyValidator : AbstractValidator<VerifyDto>
    {
        public VerifyValidator()
        {
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
                .Length(3, 50);
        }

    }
}