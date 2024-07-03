using FluentValidation;
using UserManager.DTOs;

namespace OTPService.Validations
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(i => i.Type).IsInEnum();

            RuleFor(d => d.Username).NotEmpty().NotNull()

                                                      .Matches(@"^(\+98|0)?9\d{9}$")
                                                      .WithMessage("Phone number is Not Valid!")
                                                      .When(i => i.Type == UserManager.Enums.LoginType.Phone)

                                                      .EmailAddress()
                                                      .When(i => i.Type == UserManager.Enums.LoginType.Email);

            RuleFor(i => i.Platform).IsInEnum();
            RuleFor(i => i.UniqueId).NotEmpty().NotNull();
            RuleFor(i => i.OrganizationId).NotEmpty().NotNull();
            RuleFor(i => i.Password).NotEmpty().NotNull().When(i => i.Type == UserManager.Enums.LoginType.Password);
        }
    }
}