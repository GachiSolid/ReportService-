namespace RepotringService.BLL.Responses.Report
{
    public class ReportModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public IList<ServiceModel> Services { get; set; }
    }
}
