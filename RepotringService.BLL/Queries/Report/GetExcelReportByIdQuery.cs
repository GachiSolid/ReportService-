using MediatR;
using ReportingService.BLL.Errors;
using RepotringService.BLL.Responses.Report;

namespace ReportingService.BLL.Queries.Report
{
    public record GetExcelReportByIdQuery(int ReportId, int ProviderId) : IRequest<Result<FileModel, Error>>;
}
