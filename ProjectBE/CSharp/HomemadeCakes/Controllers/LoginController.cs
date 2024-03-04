using HomemadeCakes.ModelView;
using HomemadeCakes.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomemadeCakes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly UserService _usersService;
        public LoginController(UserService UserService) =>
            _usersService = UserService;

        [HttpPost]
        public async Task<string> GetLoginAsync([FromBody] LoginRequest login)
        {
            return await _usersService.Authencate(login);
        }

    }
}
