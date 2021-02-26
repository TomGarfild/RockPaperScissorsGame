using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Server.Models;
using Server.Options;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/v1/round")]
    public class RoundController : Controller
    {
        private readonly ISeriesService _seriesService;
        private readonly IRoundService _roundService;
        private readonly IOptions<TimeOptions> _options;
        private readonly IAuthService _authService;

        public RoundController(ISeriesService seriesService, IRoundService roundService, IOptions<TimeOptions> options,
            IAuthService authService)
        {
            _seriesService = seriesService;
            _roundService = roundService;
            _options = options;
            _authService = authService;
        }

        [HttpGet]
        [Route("Play")]
        public async Task<ActionResult<string>> Throw(
            [FromHeader(Name = "x-token")] [Required]
            string token,
            [FromHeader(Name = "x-series")] [Required]
            string series,
            [FromHeader(Name = "x-choice")] [Required]
            string choice,
            [FromServices] IStatisticService statisticService,
            [FromServices] Stopwatch stopwatch
        )
        {
            if (_authService.IsAuthorized(token))
            {
                var user = _authService.GetLogin(token);
                if (!_seriesService.SeriesIs(series))
                {
                    return StatusCode(404);
                }

                stopwatch.Start();
                var tokenCancellationToken = _roundService.StartRound(user, series, choice);
                try
                {
                    if (token != null)
                        await Task.Delay(_options.Value.RoundTimeOut, (CancellationToken)tokenCancellationToken);
                }
                catch (TaskCanceledException)
                {
                    
                }

                var result = _roundService.GetResult(user, series);
                stopwatch.Stop();
                if(result!=Round.Result.Undefine) 
                    statisticService.Add(user, stopwatch.Elapsed, DateTimeOffset.Now, result, Round.ParseChoice(choice));
                return result.ToString();
            }
            else
            {
                return StatusCode(401);
            }
        }

        [HttpGet]
        [Route("TrainingPlay")]
        public async Task<ActionResult<string>> TrainingPlay(
            [FromHeader(Name = "x-token")] [Required]
            string token,
            [FromHeader(Name = "x-series")] [Required]
            string series,
            [FromHeader(Name = "x-choice")] [Required]
            string choice
        )
        {
            if (_authService.IsAuthorized(token))
            {
                var user = _authService.GetLogin(token);
                if (!_seriesService.SeriesIs(series))
                {
                    return StatusCode(404);
                }

                _roundService.StartRoundTraining(user, series, choice);
                return _roundService.GetResult(user, series).ToString();
            }

            return StatusCode(401);
        }
    }
}
