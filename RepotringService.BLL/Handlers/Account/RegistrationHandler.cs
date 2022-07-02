using ReportingService.BLL.Errors;
using MediatR;
using RepotringService.BLL.Commands.Account;
using RepotringService.BLL.Responses.Account;
using Microsoft.AspNetCore.Identity;
using ReportingService.Models;
using Microsoft.EntityFrameworkCore;

namespace ReportingService.BLL.Handlers.Account
{
    public class RegistrationHandler : IRequestHandler<RegistrationCommand, Result<UserModel, Error>>
    {
        private readonly UserManager<User> userManager;

        public RegistrationHandler(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Result<UserModel, Error>> Handle(RegistrationCommand request, CancellationToken cancellationToken)
        {
            if (await userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken))
                return Result<UserModel, Error>.Failed(new BadRequestError("This account already exist"));

            User user = new()
            {
                UserName = request.UserName,
                Email = request.Email,
                First_Name = request.First_Name,
                Last_Name = request.Last_Name
            };

            await userManager.CreateAsync(user, request.Password);

            var userModel = new UserModel()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName
            };

            return Result<UserModel, Error>.Succeeded(userModel);
        }
    }
}
