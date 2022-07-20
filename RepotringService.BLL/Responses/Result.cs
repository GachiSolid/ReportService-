using ReportingService.BLL.Errors;

namespace ReportingService.BLL
{
    /// <summary>
    /// Response class
    /// </summary>
    /// <typeparam name="T">Response Value</typeparam>
    /// <typeparam name="TError">Response Error</typeparam>
    public class Result<T, TError> where TError : Error where T : class
    {
        public bool Successed { get; }
        public T Value { get; }
        public TError Error { get; }

        private Result(bool isSucceded, T value, TError error)
        {
            Successed = isSucceded;
            Value = value;
            Error = error;
        }

        public static Result<T, TError> Succeeded(T value)
        {
            return new Result<T, TError>(true, value, null);
        }

        public static Result<T, TError> Failed(TError error)
        {
            return new Result<T, TError>(false, null, error);
        }
    }
}