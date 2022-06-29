using ReportingService.DAL.DTOs;

namespace ReportingService.BLL.Server
{
    public interface IWorkerWithUser
    {
        public Task<UserDTO> Register(UserDTO model);
    }
}
