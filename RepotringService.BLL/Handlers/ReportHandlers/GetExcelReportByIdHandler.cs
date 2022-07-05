using IronXL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReportingService.BLL;
using ReportingService.BLL.Errors;
using ReportingService.BLL.Queries.Report;
using ReportingService.DAL.EF;
using RepotringService.BLL.Responses.Report;

namespace RepotringService.BLL.Handlers.ReportHandlers
{
    public class GetExcelReportByIdHandler : IRequestHandler<GetExcelReportByIdQuery, Result<FileModel, Error>>
    {
        private readonly ApplicationContext db;
        public GetExcelReportByIdHandler(ApplicationContext db)
        {
            this.db = db;
        }

        public async Task<Result<FileModel, Error>> Handle(GetExcelReportByIdQuery request, CancellationToken cancellationToken)
        {
            var report = await db.Reports.Include(x => x.Services).ThenInclude(x => x.Provider).FirstOrDefaultAsync(x => x.Id == request.ReportId, cancellationToken: cancellationToken);
            if (report == null) return Result<FileModel, Error>.Failed(new NotFoundError("Couldn't find a Report"));

            string filePath = Path.GetTempFileName() + ".xlsx";
            byte[] file = null;
            try
            {
                WorkBook workbook = WorkBook.Create();
                var sheet = workbook.CreateWorkSheet(report.Name);
                sheet["A1"].Value = "Provider Name";
                sheet["B1"].Value = "Address";
                sheet["C1"].Value = "Service";
                sheet["D1"].Value = "Sum";
                for (int i = 0, j = 2; i < report.Services.Count; i++, j++)
                {
                    var service = report.Services.ToList()[i];
                    if (request.ProviderId == 0 || (request.ProviderId != 0 && request.ProviderId == service.ProviderId))
                    {
                        sheet["A" + j].Value = service.Provider.Name;
                        sheet["B" + j].Value = service.Provider.Address;
                        sheet["C" + j].Value = service.Type;
                        sheet["D" + j].Value = service.Sum;
                    }
                }
                sheet[$"C{report.Services.Count + 2}"].Value = "Average Sum = ";
                sheet[$"D{report.Services.Count + 2}"].Value = sheet[$"D2:D{report.Services.Count + 1}"].Avg();
                workbook.SaveAs(filePath);
                file = File.ReadAllBytes(filePath);

                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            catch (Exception)
            {
                return Result<FileModel, Error>.Failed(new Error("Critical error occured"));
            }

            var fileModel = new FileModel()
            {
                Name = report.Name,
                File = file
            };

            return Result<FileModel, Error>.Succeeded(fileModel);
        }
    }
}
