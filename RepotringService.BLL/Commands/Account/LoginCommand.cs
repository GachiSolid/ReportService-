using MediatR;
using RepotringService.BLL.Responses.Account;
using ReportingService.BLL;
using ReportingService.BLL.Errors;

namespace RepotringService.BLL.Commands.Account
{
    /// <summary>
    /// Login Command
    /// </summary>
    public class LoginCommand : IRequest<Result<UserModel, Error>>
    {
        public string UserName { get; init; }
        public string Password { get; init; }
    }
}
