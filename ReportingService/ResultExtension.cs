using Microsoft.AspNetCore.Mvc;
using ReportingService.BLL;
using ReportingService.BLL.Errors;

namespace ReportingService
{
    public static class ResultExtension
    {
        /// <summary>
        /// Convert Handler response to the Web response
        /// </summary>
        public static ObjectResult ToWebResult<T, TError>(this Result<T, TError> result) where TError : Error where T : class => result.Successed switch
        {
            true => new OkObjectResult(result.Value),
            _ => ErrorHandler(result.Error)
        };

        private static ObjectResult ErrorHandler(Error Error) => Error switch
        {
            NotFoundError => new NotFoundObjectResult(Error.Message),
            UnauthError => new UnauthorizedObjectResult(Error.Message),
            BadRequestError => new BadRequestObjectResult(Error.Message),
            _ => new ObjectResult(Error.Message) { StatusCode = 500 }
        };
    }
}
