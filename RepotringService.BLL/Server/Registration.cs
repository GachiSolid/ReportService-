using Microsoft.EntityFrameworkCore;
using ReportingService.BLL.Server;
using ReportingService.DAL.DTOs;
using ReportingService.DAL.Repositiories;
using ReportingService.Models;

namespace ReportingService.Server
{
    public class Registration : IWorkerWithUser
    {
        public readonly IRepository<UserDTO> repository;
        public Registration(IRepository<UserDTO> repository)
        {
            this.repository = repository;
        }

        public async Task<UserDTO> Register(UserDTO user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }
                else
                {
                    return await repository.Create(user);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
