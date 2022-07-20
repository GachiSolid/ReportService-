using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportingService.BLL.Queries.Report;
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

        [HttpPost]
        public async Task<IActionResult> Report(AddFileCommand command)
        {
            var check = fileValidator.Validate(command);

            if (!check.IsValid)
                return check.ToWebError();

            var result = await mediator.Send(command);
            return result.ToWebResult();
        }

        [HttpGet]
        public async Task<IActionResult> Report(int reportId)
        {
            var result = await mediator.Send(new GetReportByIdQuery(reportId));
            return result.ToWebResult();
        }

        [HttpGet("excel")]
        public async Task<IActionResult> ExcelReport(int reportId, int providerId)
        {
            var result = await mediator.Send(new GetExcelReportByIdQuery(reportId, providerId));
            if (result.Value != null)
            {
                string file_type = "application/vnd.ms-excel";
                string file_name = $"{result.Value.Name}.xlsx";
                return File(result.Value.File, file_type, file_name);
            }
            return result.ToWebResult();
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> PDFReport(int reportId, int providerId)
        {
            var result = await mediator.Send(new GetPDFReportByIdQuery(reportId, providerId));
            if (result.Value != null)
            {
                string file_type = "application/pdf";
                string file_name = $"{result.Value.Name}.pdf";
                return File(result.Value.File, file_type, file_name);
            }
            return result.ToWebResult();
        }
    }
}
