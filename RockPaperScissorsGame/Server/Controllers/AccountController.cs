using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/v1")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Account account)
        {
            var success = await _authService.Register(account.Login, account.Password);
            if (success)
            {
                return Ok();
            }

            return Conflict();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Account account)
        {
            var token = await _authService.Login(account.Login, account.Password);
            if (token == null) return NotFound();
            return Ok();
        }
    }
}