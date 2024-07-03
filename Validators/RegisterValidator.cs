using FluentValidation;
using Microsoft.AspNetCore.Razor.Language;
using UserManager.DTOs;

namespace OTPService.Validations
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(d => d.Phone).NotEmpty().NotNull()
                                                .Matches(@"^(\+98|0)?9\d{9}$")
                                                .WithMessage("Phone number is Not Valid!");

            RuleFor(d => d.Email).EmailAddress().When(i => !string.IsNullOrEmpty(i.Email));
            RuleFor(i => i.OrganizationId).NotEmpty().NotNull();
            RuleFor(i => i.Password).NotEmpty().NotNull();

            RuleFor(i => i.RepPassword).Must((i, RepPassword) => RepPassword == i.Password)
            .WithMessage("Password Repeat is Not Equal to the Password");
        }
    }
}