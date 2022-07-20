namespace ReportingService.BLL.Errors
{
    /// <summary>
    /// 404 Error
    /// </summary>
    public class BadRequestError: Error
    {
        public BadRequestError(string message)
        : base(message)
        {

        }
    }
}
