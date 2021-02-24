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
        [Route("NewRoom")]
        public async Task<ActionResult<string>> NewRoom([FromHeader(Name = "x-login")][Required] string user)
        {
            // todo check authorization
            var room = _seriesService.AddToSeries(user); // ToDo add user id who send request

            while ((!room.IsDeleted)
                   && (!room.IsFull))
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                _seriesService.Check();
            }

            if (room.IsDeleted)
            {
                return StatusCode(423);
            }
            else
            {
                return room.Id;
            }
        }   
    }
}
