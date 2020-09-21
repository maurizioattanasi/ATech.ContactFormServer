using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ATech.ContactFormServer.Api.Controllers
{
    /// <summary>
    /// Message Controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        public MessageController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Returns all the contact messages
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<Domain.Entities.ContactFormMessage>>> GetAll(int? page = null, int? pageSize = null)
        {
            try
            {
                var messages = await this.mediator.Send(new Features.Message.GetAll(page, pageSize));

                return Ok(messages);
            }
            catch (Exception ex)
            {
                string content = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                var problemDetails = new ProblemDetails()
                {
                    Title = "Internal Server Error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = content,
                    Instance = $"/AddMessage/",
                };

                return BadRequest(problemDetails);
            }
        }

        /// <summary>
        /// Creates a new message item
        /// </summary>
        /// <param name="messageDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Submit([FromForm] Domain.DTO.MessageDto messageDto)
        {
            try
            {
                var id = await this.mediator.Send(new Features.Message.Add(messageDto));

                return Ok();
            }
            catch (Exception ex)
            {
                string content = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                var problemDetails = new ProblemDetails()
                {
                    Title = "Internal Server Error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = content,
                    Instance = $"/AddMessage/",
                };

                return BadRequest(problemDetails);
            }
        }
    }
}
