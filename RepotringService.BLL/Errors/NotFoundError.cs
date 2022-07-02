namespace ReportingService.BLL.Errors
{
    /// <summary>
    /// 404 Error
    /// </summary>
    public class NotFoundError: Error
    {
        public NotFoundError(string message)
        : base(message)
        { }
    }
}
