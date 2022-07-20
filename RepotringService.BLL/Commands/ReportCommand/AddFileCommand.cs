using MediatR;
using Microsoft.AspNetCore.Http;
using ReportingService.BLL;
using ReportingService.BLL.Errors;
using RepotringService.BLL.Responses.Report;

namespace RepotringService.BLL.Commands.Report
{
    public class AddFileCommand : IRequest<Result<ReportModel, Error>>
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}
