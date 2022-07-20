using MediatR;
using Microsoft.EntityFrameworkCore;
using ReportingService.BLL;
using ReportingService.BLL.Errors;
using ReportingService.BLL.Queries.Report;
using ReportingService.DAL.EF;
using RepotringService.BLL.Responses.Report;

namespace RepotringService.BLL.Handlers.ReportHandlers
{
    public class GetReportByIdHandler : IRequestHandler<GetReportByIdQuery, Result<ReportModel, Error>>
    {
        private readonly ApplicationContext db;
        public GetReportByIdHandler(ApplicationContext db)
        {
            this.db = db;
        }

        public async Task<Result<ReportModel, Error>> Handle(GetReportByIdQuery request, CancellationToken cancellationToken)
        {
            var report = await db.Reports.Include(x => x.Services).ThenInclude(x => x.Provider).FirstOrDefaultAsync(x => x.Id == request.ReportId, cancellationToken: cancellationToken);

            if (report == null) return Result<ReportModel, Error>.Failed(new NotFoundError("Couldn't find a Report"));

            var services = new List<ServiceModel>();
            foreach (var service in report.Services)
            {
                var serviceModel = new ServiceModel()
                {
                    Id = service.Id,
                    Type = service.Type,
                    ProviderName = service.Provider.Name,
                    ProviderAddress = service.Provider.Address,
                    Sum = service.Sum
                };

                services.Add(serviceModel);
            }

            var reportModel = new ReportModel()
            {
                Id = report.Id,
                Name = report.Name,
                Date = report.Date,
                Services = services
            };

            return Result<ReportModel, Error>.Succeeded(reportModel);
        }
    }
}
