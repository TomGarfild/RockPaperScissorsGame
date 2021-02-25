using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Server.Model;
using Server.Service;
using Server.Services;


namespace Server.Controllers
{
    [ApiController]
    [Route("api/v1/series")]
    public class SeriesController : ControllerBase
    {
        private readonly ISeriesService _seriesService;
        private readonly IAuthService _authService;

        public SeriesController(ISeriesService seriesService, IAuthService authService)
        {
            _seriesService = seriesService;
            _authService = authService;
        }

        [HttpGet]
        [Route("NewPublicSeries")]
        public async Task<ActionResult<Series>> NewPublicSeries([FromHeader(Name = "x-token")] [Required]
            string token)
        {
            if (_authService.IsAuthorized(token))
            {
                var user = _authService.GetLogin(token);
                var series = _seriesService.AddToPublicSeries(user); // ToDo add user id who send request

                while ((!series.IsDeleted)
                       && (!series.IsFull))
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    _seriesService.Check();
                }

                if (series.IsDeleted)
                {
                    return StatusCode(423);
                }
                else
                {
                    return series;
                }
            }
            else
            {
                return StatusCode(401);
            }
        }

        [HttpGet]
        [Route("NewPrivateSeries")]
        public async Task<ActionResult<PrivateSeries>> NewPrivateSeries([FromHeader(Name = "x-token")] [Required]
            string token)
        {
            if (_authService.IsAuthorized(token))
            {
                var user = _authService.GetLogin(token);
                var series = _seriesService.AddToPrivateSeries(user); // ToDo add user id who send request
                return series;
            }

            return StatusCode(401);
        }

        [HttpGet]
        [Route("SearchPrivateSeries")]
        public async Task<ActionResult<Series>> SearchPrivateSeries([FromHeader(Name = "x-token")] [Required]
            string token,
            [FromHeader(Name = "x-code")] [Required]
            string code)
        {

            if (_authService.IsAuthorized(token))
            {
                var user = _authService.GetLogin(token);
                var series =
                    _seriesService.SearchAndAddToPrivateSeries(user, code);

                while ((!series.IsDeleted)
                       && (!series.IsFull))
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    _seriesService.Check();
                }

                if (series.IsDeleted)
                {
                    return StatusCode(423);
                }

                return series;
            }
            return StatusCode(401);
        }

        [HttpGet]
        [Route("NewTrainingSeries")]
        public async Task<ActionResult<TrainingSeries>> NewTrainingSeries([FromHeader(Name = "x-token")] [Required]
            string token)
        {
            if (_authService.IsAuthorized(token))
            {
                var user = _authService.GetLogin(token);
                var series = _seriesService.AddToTrainingSeries(user); // ToDo add user id who send request
                return series;
            }

            return StatusCode(401);
        }
        [HttpDelete]
        [Route("CancelSeries")]
        public async Task CancelSeries([FromHeader(Name = "x-token")] [Required]
            string token, [FromHeader(Name = "x-series")] [Required]
            string series)
        {
            if (_authService.IsAuthorized(token))
            {
                _seriesService.CancelSeries(series);
            }
        }
    }
}
