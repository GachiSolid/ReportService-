using Microsoft.EntityFrameworkCore;
using ReportingService.DAL.EF;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using RepotringService.BLL.Handlers.ReportHandlers;
using RepotringService.BLL.Commands.Report;
using ReportingService.BLL.Queries.Report;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using IronXL;

namespace ReportingService.Tests
{
    [TestClass]
    public class ReportTest : InitTest
    {
        public ReportTest() : base(
                new DbContextOptionsBuilder<ApplicationContext>()
                    .UseInMemoryDatabase("ReportDatabase")
                    .Options)
        {
        }

        [TestMethod]
        public async Task AddFile()
        {
            var file = File.ReadAllBytes("../../../file.csv");
            var stream = new MemoryStream(file);
            var formFile = new FormFile(stream, 0, file.Length, "name", "fileName");

            var addFileCommand = new AddFileCommand()
            {
                UserId = "12345678",
                File = formFile,
                Name = "ReportTest"
            };

            using var context = new ApplicationContext(ContextOptions);
            var handler = new AddReportHandler(context);

            var result = await handler.Handle(addFileCommand, new CancellationToken());

            Assert.IsTrue(result.Successed);
            var report = context.Reports.Include(x => x.Services).FirstOrDefault(x => x.Name == "ReportTest");
            Assert.IsNotNull(report);
            Assert.IsTrue(context.Providers.Any(x => x.Name == "InternetCompany" && x.Address == "kazan"));
            Assert.IsTrue(context.Providers.Any(x => x.Name == "ZKH" && x.Address == "amet"));
            Assert.AreEqual(2, report.Services.Count);
        }

        [TestMethod]
        public async Task CreateExcel_AllProviders()
        {
            using var context = new ApplicationContext(ContextOptions);
            var handler = new GetExcelReportByIdHandler(context);

            var result = await handler.Handle(new GetExcelReportByIdQuery(1, 0), new CancellationToken());

            Assert.IsTrue(result.Successed);
            Assert.AreEqual("Report", result.Value.Name);
            Assert.IsNotNull(result.Value.File);

            var workbook = WorkBook.Load(result.Value.File);
            var worksheet = workbook.GetWorkSheet("Report");
            Assert.AreEqual("DomRu", worksheet["A2"].Value);
            Assert.AreEqual("Building", worksheet["C2"].Value);
            Assert.AreEqual("DomRu", worksheet["A3"].Value);
            Assert.AreEqual("Working", worksheet["C3"].Value);
            Assert.AreEqual("DomRu", worksheet["A4"].Value);
            Assert.AreEqual("Security", worksheet["C4"].Value);
            Assert.AreEqual("Rostelecom", worksheet["A5"].Value);
            Assert.AreEqual("Working", worksheet["C5"].Value);
        }

        [TestMethod]
        public async Task CreateExcel_OneProvider()
        {
            using var context = new ApplicationContext(ContextOptions);
            var handler = new GetExcelReportByIdHandler(context);

            var result = await handler.Handle(new GetExcelReportByIdQuery(1, 1), new CancellationToken());

            Assert.IsTrue(result.Successed);
            Assert.AreEqual("Report", result.Value.Name);
            Assert.IsNotNull(result.Value.File);

            var workbook = WorkBook.Load(result.Value.File);
            var worksheet = workbook.GetWorkSheet("Report");
            Assert.AreEqual("DomRu", worksheet["A2"].Value);
            Assert.AreEqual("Building", worksheet["C2"].Value);
            Assert.AreEqual("DomRu", worksheet["A3"].Value);
            Assert.AreEqual("Working", worksheet["C3"].Value);
            Assert.AreEqual("DomRu", worksheet["A4"].Value);
            Assert.AreEqual("Security", worksheet["C4"].Value);
            Assert.AreNotEqual("Rostelecom", worksheet["A5"].Value);
        }

        [TestMethod]
        public async Task CreatePDF_AllProviders()
        {
            using var context = new ApplicationContext(ContextOptions);
            var handler = new GetPDFReportByIdHandler(context);

            var result = await handler.Handle(new GetPDFReportByIdQuery(1, 0), new CancellationToken());

            Assert.IsTrue(result.Successed);
            Assert.AreEqual("Report", result.Value.Name);
            Assert.IsNotNull(result.Value.File);

            var stream = new MemoryStream(result.Value.File);
            using var reader = new PdfReader(stream);
            var doc = new PdfDocument(reader);
            var text = PdfTextExtractor.GetTextFromPage(doc.GetFirstPage()).Split("\n");

            Assert.IsTrue(text[1].Contains("DomRu"));
            Assert.IsTrue(text[1].Contains("Building"));
            Assert.IsTrue(text[2].Contains("DomRu"));
            Assert.IsTrue(text[2].Contains("Working"));
            Assert.IsTrue(text[3].Contains("DomRu"));
            Assert.IsTrue(text[3].Contains("Security"));
            Assert.IsTrue(text[4].Contains("Rostelecom"));
            Assert.IsTrue(text[4].Contains("Working"));
        }

        [TestMethod]
        public async Task CreatePDF_OneProvider()
        {
            using var context = new ApplicationContext(ContextOptions);
            var handler = new GetPDFReportByIdHandler(context);

            var result = await handler.Handle(new GetPDFReportByIdQuery(1, 1), new CancellationToken());

            Assert.IsTrue(result.Successed);
            Assert.AreEqual("Report", result.Value.Name);
            Assert.IsNotNull(result.Value.File);

            var stream = new MemoryStream(result.Value.File);
            using var reader = new PdfReader(stream);
            var doc = new PdfDocument(reader);
            Console.WriteLine(PdfTextExtractor.GetTextFromPage(doc.GetFirstPage()));
            var text = PdfTextExtractor.GetTextFromPage(doc.GetFirstPage()).Split("\n");

            Assert.AreEqual(4, text.Length);
            Assert.IsTrue(text[1].Contains("DomRu"));
            Assert.IsTrue(text[1].Contains("Building"));
            Assert.IsTrue(text[2].Contains("DomRu"));
            Assert.IsTrue(text[2].Contains("Working"));
            Assert.IsTrue(text[3].Contains("DomRu"));
            Assert.IsTrue(text[3].Contains("Security"));
        }
    }
}
