using MediatR;
using RepotringService.BLL.Responses.Account;
using ReportingService.BLL;
using ReportingService.BLL.Errors;

namespace RepotringService.BLL.Commands.Account
{
    /// <summary>
    /// Registration Command
    /// </summary>
    public class RegistrationCommand : IRequest<Result<UserModel, Error>>
    {
        public string Email { get; init; }
        public string UserName { get; init; }
        public string First_Name { get; init; }
        public string Last_Name { get; init; }
        public string Password { get; init; }
        public string ConfirmPassword { get; init; }
    }
}
