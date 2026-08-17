using Homework1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Homework1.Controller
{
    [ApiController]
    [Route("api/security")]
    public class SecurityController : ControllerBase
    {
        private readonly KeyService _keyService;

        public SecurityController(KeyService keyService)
        {
            _keyService = keyService;
        }

        [HttpGet("public-key")]
        public IActionResult GetPublicKey()
        {
            var publicKey =
                _keyService.GetPublicKey();

            return Ok(new
            {
                publicKey
            });
        }
    }
}
