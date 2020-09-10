using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ATech.ContactFormServer.Api.Controllers
{
    /// <summary>
    /// System Utilities Controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="mediator"></param>
        public SystemController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// System heartbeat function
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<string>> Ping()
        {
            await Task.Yield();

            return Ok("Pong");
        }
    }
}
