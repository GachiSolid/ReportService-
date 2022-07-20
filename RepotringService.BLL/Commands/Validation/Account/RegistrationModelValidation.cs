using FluentValidation;
using RepotringService.BLL.Commands.Account;

namespace ReportingService.BLL.Commands.Validation.Account
{
    public class RegistrationModelValidation: AbstractValidator<RegistrationCommand>
    {
        public RegistrationModelValidation()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.First_Name).NotEmpty();
            RuleFor(x => x.Last_Name).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
        }
    }
}
