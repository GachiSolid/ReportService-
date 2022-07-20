namespace ReportingService.BLL.Errors
{
    /// <summary>
    /// Error Base Class
    /// </summary>
    public class Error
    {
        public string Message { get; }
        public Error(string message)
        {
            Message = message;
        }
    }
}
