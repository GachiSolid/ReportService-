using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReportingService.BLL;
using ReportingService.BLL.Errors;
using ReportingService.BLL.Queries.Report;
using ReportingService.DAL.EF;
using RepotringService.BLL.Responses.Report;

namespace RepotringService.BLL.Handlers.ReportHandlers
{
    public class GetPDFReportByIdHandler : IRequestHandler<GetPDFReportByIdQuery, Result<FileModel, Error>>
    {
        private readonly ApplicationContext db;
        public GetPDFReportByIdHandler(ApplicationContext db)
        {
            this.db = db;
        }

        public async Task<Result<FileModel, Error>> Handle(GetPDFReportByIdQuery request, CancellationToken cancellationToken)
        {
            var report = await db.Reports.Include(x => x.Services).ThenInclude(x => x.Provider).FirstOrDefaultAsync(x => x.Id == request.ReportId, cancellationToken: cancellationToken);
            if (report == null) return Result<FileModel, Error>.Failed(new NotFoundError("Couldn't find a Report"));

            byte[] file = null;
            try
            {
                var stream = new MemoryStream();
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);
                Table table = new Table(4, false)
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE);
                Cell cell11 = new Cell(1, 1)
                   .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new Paragraph("Provider Name"));
                table.AddCell(cell11);
                Cell cell12 = new Cell(1, 1)
                   .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new Paragraph("Provider address"));
                table.AddCell(cell12);
                Cell cell13 = new Cell(1, 1)
                   .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new Paragraph("Service"));
                table.AddCell(cell13);
                Cell cell14 = new Cell(1, 1)
                   .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new Paragraph("Sum"));
                table.AddCell(cell14);

                for (int i = 0; i < report.Services.Count; i++)
                {
                    var service = report.Services.ToList()[i];
                    if (request.ProviderId == 0 || (request.ProviderId != 0 && request.ProviderId == service.ProviderId))
                    {
                        Cell cell21 = new Cell(1, 1)
                      .SetTextAlignment(TextAlignment.CENTER)
                      .Add(new Paragraph(service.Provider.Name));
                        table.AddCell(cell21);
                        Cell cell22 = new Cell(1, 1)
                      .SetTextAlignment(TextAlignment.CENTER)
                      .Add(new Paragraph(service.Provider.Address));
                        table.AddCell(cell22);
                        Cell cell23 = new Cell(1, 1)
                      .SetTextAlignment(TextAlignment.CENTER)
                      .Add(new Paragraph(service.Type));
                        table.AddCell(cell23);
                        Cell cell24 = new Cell(1, 1)
                      .SetTextAlignment(TextAlignment.CENTER)
                      .Add(new Paragraph(service.Sum.ToString()));
                        table.AddCell(cell24);
                    }
                }
                document.Add(table);
                document.Close();
                file = stream.ToArray();
            }
            catch (Exception e)
            {
                return Result<FileModel, Error>.Failed(new Error(e.Message));
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
