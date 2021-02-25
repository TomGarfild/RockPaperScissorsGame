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


namespace Server.Controllers
{
    [ApiController]
    [Route("api/v1/series")]
    public class SeriesController : ControllerBase
    {
        private readonly ISeriesService _seriesService;

        public SeriesController(ISeriesService seriesService)
        {
            _seriesService = seriesService;
        }

        [HttpGet]
        [Route("NewPublicSeries")]
        public async Task<ActionResult<Series>> NewPublicSeries([FromHeader(Name = "x-login")][Required] string user)
        {
            // todo check authorization
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

        [HttpGet]
        [Route("NewPrivateSeries")]
        public async Task<ActionResult<PrivateSeries>> NewPrivateSeries([FromHeader(Name = "x-login")][Required] string user)
        {
            // todo check authorization
            var series = _seriesService.AddToPrivateSeries(user); // ToDo add user id who send request
            return series;
        }
        [HttpGet]
        [Route("SearchPrivateSeries")]
        public async Task<ActionResult<Series>> SearchPrivateSeries([FromHeader(Name = "x-login")][Required] string user, [FromHeader(Name = "x-code")][Required] string code)
        {
            // todo check authorization
            var series = _seriesService.SearchAndAddToPrivateSeries(user,code); // ToDo add user id who send request

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
        [HttpGet]
        [Route("NewTrainingSeries")]
        public async Task<ActionResult<TrainingSeries>> NewTrainingSeries([FromHeader(Name = "x-login")][Required] string user)
        {
            // todo check authorization
            var series = _seriesService.AddToTrainingSeries(user); // ToDo add user id who send request
            return series;
           
        }
    }
}
