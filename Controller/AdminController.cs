using Homework1.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Homework1.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        [HttpGet("dashboard")]
        [Authorize(Roles = RoleConstants.Admin)]
        public IActionResult Dashboard()
        {
            return Ok("Chào Admin!");
        }
    }
}
