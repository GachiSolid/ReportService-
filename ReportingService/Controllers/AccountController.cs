using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportingService.Token;
using RepotringService.BLL.Commands.Account;
using System.Threading.Tasks;

namespace ReportingService.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IMediator mediator;
        private readonly IValidator<LoginCommand> loginValidator;
        private readonly IValidator<RegistrationCommand> registrationValidator;
        private readonly IJwtGenerator jwtGenerator;

        public AccountController(IJwtGenerator jwtGenerator, IMediator mediator, IValidator<LoginCommand> loginValidator, IValidator<RegistrationCommand> registrationValidator)
        {
            this.jwtGenerator = jwtGenerator;
            this.mediator = mediator;
            this.loginValidator = loginValidator;
            this.registrationValidator = registrationValidator;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Regisration(RegistrationCommand command)
        {
            var check = registrationValidator.Validate(command);

            if (!check.IsValid)
                return check.ToWebError();

            var result = await mediator.Send(command);
            if (result.Successed) result.Value.Token = jwtGenerator.BuildToken(result.Value.UserName);
            return result.ToWebResult();
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var check = loginValidator.Validate(command);

            if (!check.IsValid)
                return check.ToWebError();

            var result = await mediator.Send(command);
            if (result.Successed) result.Value.Token = jwtGenerator.BuildToken(result.Value.UserName);
            return result.ToWebResult();
        }
    }
}
