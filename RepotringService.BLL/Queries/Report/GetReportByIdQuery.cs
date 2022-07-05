using MediatR;
using ReportingService.BLL.Errors;
using RepotringService.BLL.Responses.Report;

namespace ReportingService.BLL.Queries.Report
{
    public record GetReportByIdQuery(int ReportId) : IRequest<Result<ReportModel, Error>>;
}
