using MediatR;
using ReportingService.BLL.Errors;
using RepotringService.BLL.Responses.Account;

namespace ReportingService.BLL.Queries.Account
{
    public record GetUserByIdQuery(string UserId) : IRequest<Result<UserModel, Error>>;
}
