using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Server
{
    [ApiController]
    [Route("api/v1/registration")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class RegistrationController : ControllerBase
    {
        
    }
}