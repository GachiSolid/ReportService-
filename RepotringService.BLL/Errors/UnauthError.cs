namespace ReportingService.BLL.Errors
{
    /// <summary>
    /// 401 Error
    /// </summary>
    public class UnauthError: Error
    {
        public UnauthError(string message)
        : base(message)
        { }
    }
}
