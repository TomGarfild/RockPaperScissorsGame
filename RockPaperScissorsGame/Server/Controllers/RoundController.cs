using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Server.Options;
using Server.Service;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/v1/round")]
    public class RoundController : Controller
    {
        private readonly ISeriesService _seriesService;
        private readonly IRoundService _roundService;
        private readonly IOptions<TimeOptions> _options;

        public RoundController(ISeriesService seriesService, IRoundService roundService, IOptions<TimeOptions> options)
        {
            _seriesService = seriesService;
            _roundService = roundService;
            _options = options;
        }

        [HttpGet]
        [Route("Throw")]
        public async Task<ActionResult<string>> Throw(
            [FromHeader(Name = "x-login")] [Required]
            string user,
            [FromHeader(Name = "x-series")] [Required]
            string series,
            [FromHeader(Name = "x-choice")] [Required]
            string choice
        )
        {
            // todo check authorization

            if (!_seriesService.SeriesIs(series))
            {
                return StatusCode(404);
            }

            var token = _roundService.StartRound(user, series, choice);
            try
            {
                if (token != null)
                    await Task.Delay(_options.Value.RoundTimeOut, (CancellationToken)token);
            }
            catch (TaskCanceledException)
            {

            }

            return _roundService.GetResult(user, series).ToString();
        }
    }
}
