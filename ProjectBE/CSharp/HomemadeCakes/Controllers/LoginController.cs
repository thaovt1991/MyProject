using HomemadeCakes.ModelView;
using HomemadeCakes.Service;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("login")]
        [AllowAnonymous]//bo qua xac thực
        public async Task<IActionResult> GetLoginAsync([FromBody] LoginRequest login)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var token = await _usersService.Authencate(login);
            return Ok(token);
        }

       

    }
}
