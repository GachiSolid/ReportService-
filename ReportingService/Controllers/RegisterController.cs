using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ReportingService.BLL.Server;
using ReportingService.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReportingService.Controllers
{
    [Route("[controller]")]
    public class RegisterController : Controller
    {
        private readonly IWorkerWithUser workerWithUser;
        public RegisterController(IWorkerWithUser workerWithUser)
        {
            this.workerWithUser = workerWithUser;

        }

        [HttpPost]
        public async Task<IActionResult> Regisration([FromForm] UserDTO user)
        {
            var result = await workerWithUser.Register(user);

            if (result == null)
                return BadRequest();

            return Ok(result);
        }
    }
}
