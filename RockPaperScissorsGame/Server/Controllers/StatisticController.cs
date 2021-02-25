using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Service;
using Server.StatisticStorege;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/v1/statistic")]
    public class StatisticController : ControllerBase
    {
        [HttpGet]
        [Route("LocalStatistic")]
        public  Task<IEnumerable<StatisticItem>> Local([FromHeader(Name = "x-login")][Required] string user,
        [FromServices] StatisticService statisticService)
        {
            // todo check authorization
            return Task.FromResult(statisticService.GetStatisticItems(user));
        }
        [HttpGet]
        [Route("GlobalStatistic")]
        public Task<IEnumerable<StatisticItem>> Global([FromHeader(Name = "x-login")][Required] string user,
            [FromServices] StatisticService statisticService)
        {
            // todo check authorization
            return Task.FromResult(statisticService.GetGlobalStatistic());
        }

    }
}
