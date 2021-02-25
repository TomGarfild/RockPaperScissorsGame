using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/v1/account")]
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
        [Route("register")]
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
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Account account)
        {
            var token = await _authService.Login(account.Login, account.Password);
            if (token == null) return NotFound();
            return Ok(token);
        }

        [HttpDelete]
        [Route(("logout/{token}"))]
        public async Task<IActionResult> Logout()
        {
            var token = (string)HttpContext.Request.RouteValues["token"];
            var result = await _authService.Logout(token);
            if (result) return Ok();
            return NotFound();
        }
    }
}