using Amazon.Runtime.Internal.Util;
using HomemadeCakes.Common;
using HomemadeCakes.Model;
using HomemadeCakes.ModelView;
using HomemadeCakes.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace HomemadeCakes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserService _usersService;

        public UserController(UserService UserService) =>
            _usersService = UserService;

        [HttpGet]
        public async Task<List<User>> GetAllAsync()
        {
            var test = await _usersService.GetAsync();
            return test;

        }

        [HttpGet("{id}")] //id:length(24)
        public async Task<ActionResult<User>> GetOneAsync(string id)
        {
            var user = await _usersService.GetOneAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        [AllowAnonymous] // Attribute để bỏ qua xác thực
        public async Task<IActionResult> CreatedUserAsync([FromForm] RegisterRequestBase regisUser)
        {
            if (string.IsNullOrEmpty(regisUser.UserName) || string.IsNullOrEmpty(regisUser.Password))
            {
               return BadRequest();
            }
            var newUser = new User();
            newUser.Password = Helper.HashPassword(regisUser.Password); //AESCrypto.Encrypt(regisUser.Password);
            newUser.UserID = regisUser.UserID;
            newUser.UserName = regisUser.UserName;
          

            await _usersService.CreateAsync(newUser);

            return Ok(newUser);
            //return CreatedAtAction(nameof(CreatedUserAsync), new { id = newUser.Id }, newUser);
            // return CreatedAtAction("AddNew", new { id = newUser.Id }, newUser);
        }

        [HttpPut("{id}")]//:length(24)
        public async Task<IActionResult> Update(string id, User updatedBook)
        {
            var user = await _usersService.GetOneAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            updatedBook.Id = user.Id;

            await _usersService.UpdateAsync(id, updatedBook);

            return NoContent();
        }

        [HttpDelete("{id}")] //:length(24)
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _usersService.GetOneAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            await _usersService.RemoveAsync(id);

            return NoContent();
        }

        //[HttpPost]
        //[Route("/api/login")]
        //public async Task<string> GetLoginAsync([FromBody] LoginRequest login)
        //{
        //    return await _usersService.Authencate(login);
        //}
    }

}
