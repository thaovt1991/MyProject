using HomemadeCakes.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;
using HomemadeCakes.Model.Common;

namespace HomemadeCakes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        [HttpPost]
        [Route("invoke")]
        public async Task<IActionResult> Exec([FromBody] RequestBase request)
        {
            using (var scope = HttpContext.RequestServices.CreateScope())
            {
                try
                {
                    var result = await Helper.InvokeMethodAsync(
                        request
                    );

                    return Ok(result);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(new ResponseBase<object> { ErrorCode = ex.HResult.ToString(), Message = ex.Message });
                }
                catch (Exception ex)
                {
                    return BadRequest(new ResponseBase<object> { ErrorCode = ex.HResult.ToString(), Message = ex.Message });
                }
            }

        }
    }
}
