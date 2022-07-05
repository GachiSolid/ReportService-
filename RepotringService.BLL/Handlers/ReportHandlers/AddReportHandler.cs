using MediatR;
using ReportingService.BLL;
using ReportingService.BLL.Errors;
using RepotringService.BLL.Commands.Report;
using RepotringService.BLL.Responses.Report;
using ReportingService.DAL.Models;
using ReportingService.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace RepotringService.BLL.Handlers.ReportHandlers
{
    public class AddReportHandler : IRequestHandler<AddFileCommand, Result<ReportModel, Error>>
    {
        private readonly ApplicationContext db;
        public AddReportHandler(ApplicationContext db)
        {
            this.db = db;
        }

        public async Task<Result<ReportModel, Error>> Handle(AddFileCommand request, CancellationToken cancellationToken)
        {
            var report = new Report()
            {
                Name = request.Name,
                Date = DateTime.UtcNow,
                UserId = request.UserId
            };

            await db.Reports.AddAsync(report, cancellationToken);

            using (var reader = new StreamReader(request.File.OpenReadStream()))
            {
                await reader.ReadLineAsync();
                while (reader.Peek() >= 0)
                {
                    string? line = await reader.ReadLineAsync();
                    if (line != null)
                    {
                        string[] words = line.Split(',');
                        var provider = await db.Providers.FirstOrDefaultAsync(x => x.Name == words[0] && x.Address == words[1], cancellationToken: cancellationToken);
                        if (provider == null)
                        {
                            provider = new Provider { Name = words[0], Address = words[1] };
                            await db.Providers.AddAsync(provider, cancellationToken);
                        }
                        var service = new Service { Type = words[2], Provider = provider, Sum = Convert.ToInt32(words[3]), Report = report };
                        await db.Services.AddAsync(service, cancellationToken);
                    }
                }
            }

            await db.SaveChangesAsync(cancellationToken);

            var services = new List<ServiceModel>();
            foreach (var service in await db.Services.Include(x => x.Provider).Where(x => x.ReportId == report.Id).ToListAsync(cancellationToken: cancellationToken))
            {
                var serviceModel = new ServiceModel()
                {
                    Id = service.Id,
                    Type = service.Type,
                    ProviderName = service.Provider.Name,
                    ProviderAddress = service.Provider.Address,
                    Sum = service.Sum
                };
            }

            var reportModel = new ReportModel()
            {
                Id = report.Id,
                Date = report.Date,
                Name = report.Name,
                Services = services
            };

            return Result<ReportModel, Error>.Succeeded(reportModel);
        }
    }
}
