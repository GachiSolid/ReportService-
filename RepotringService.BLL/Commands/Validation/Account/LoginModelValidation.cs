using FluentValidation;
using RepotringService.BLL.Commands.Account;

namespace ReportingService.BLL.Commands.Validation.Account
{
    /// <summary>
    /// Login Command Validation
    /// </summary>
    public class LoginModelValidation : AbstractValidator<LoginCommand>
    {
        public LoginModelValidation()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
