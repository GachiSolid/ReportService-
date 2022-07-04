using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepotringService.BLL.Commands.Report;
using System.Threading.Tasks;

namespace ReportingService.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("[controller]")]
    public class ReportController: Controller
    {
        private readonly IMediator mediator;
        private readonly IValidator<AddFileCommand> fileValidator;

        public ReportController(IMediator mediator, IValidator<AddFileCommand> fileValidator)
        {
            this.mediator = mediator;
            this.fileValidator = fileValidator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Report(AddFileCommand command)
        {
            var check = fileValidator.Validate(command);

            if (!check.IsValid)
                return check.ToWebError();

            var result = await mediator.Send(command);
            return result.ToWebResult();
        }
    }
}
