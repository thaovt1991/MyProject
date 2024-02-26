using HomemadeCakes.Model;
using HomemadeCakes.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomemadeCakes.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [ApiController]
        [Route("api/[controller]")]
        public class BooksController : ControllerBase
        {
            private readonly UserService _usersService;

            public BooksController(UserService UserService) =>
                _usersService = UserService;

            [HttpGet]
            public async Task<List<User>> Get() =>
                await _usersService.GetAsync();

            [HttpGet("{id:length(24)}")]
            public async Task<ActionResult<User>> Get(string id)
            {
                var user = await _usersService.GetAsync(id);

                if (user is null)
                {
                    return NotFound();
                }

                return user;
            }

            [HttpPost]
            public async Task<IActionResult> Post(User newUser)
            {
                await _usersService.CreateAsync(newUser);

                return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
            }

            [HttpPut("{id:length(24)}")]
            public async Task<IActionResult> Update(string id, User updatedBook)
            {
                var user = await _usersService.GetAsync(id);

                if (user is null)
                {
                    return NotFound();
                }

                updatedBook.Id = user.Id;

                await _usersService.UpdateAsync(id, updatedBook);

                return NoContent();
            }

            [HttpDelete("{id:length(24)}")]
            public async Task<IActionResult> Delete(string id)
            {
                var user = await _usersService.GetAsync(id);

                if (user is null)
                {
                    return NotFound();
                }

                await _usersService.RemoveAsync(id);

                return NoContent();
            }
        }
    }
}
