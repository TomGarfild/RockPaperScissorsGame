using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Service;
using Server.Services;
using Server.StatisticStorege;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/v1/statistic")]
    public class StatisticController : ControllerBase
    {
        private readonly IAuthService _authService;

        public StatisticController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        [Route("LocalStatistic")]
        public async Task<ActionResult<string>> Local([FromHeader(Name = "x-token")][Required] string token,
        [FromServices] IStatisticService statisticService)
        {
            if (!_authService.IsAuthorized(token))
            {
                var user = _authService.GetLogin(token);
                return StatisticItem.ToStringStatisticLocalPlayer(statisticService.GetStatisticItems(user).ToList());
            }
            else
            {
                return StatusCode(401);
            }
        }
        [HttpGet]
        [Route("GlobalStatistic")]
        public async Task<string> Global([FromServices] IStatisticService statisticService)
        {
            return StatisticItem.ToStringStatisticGlobal( statisticService.GetGlobalStatistic().ToList());
        }

    }
}
