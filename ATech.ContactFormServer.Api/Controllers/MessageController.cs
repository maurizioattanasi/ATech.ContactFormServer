using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ATech.Infrastructure.Exceptions;
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
        /// <param name="accountId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]/{accountId}")]
        public async Task<ActionResult<IEnumerable<Domain.Entities.Message>>> GetAll([FromRoute] Guid accountId, int? page = null, int? pageSize = null)
        {
            try
            {
                var messages = await this.mediator.Send(new Features.Message.GetAll(accountId, page, pageSize));

                return Ok(messages);
            }
            catch (HttpException ex)
            {
                string content = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                var problemDetails = new ProblemDetails()
                {
                    Status = ex.StatusCode,
                    Detail = content,
                    Instance = $"/AddMessage",
                };

                return BadRequest(problemDetails);
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
        /// <param name="accountId"></param>
        /// <param name="messageDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]/{accountId}")]
        public async Task<ActionResult<Guid>> Submit([FromRoute] Guid accountId, [FromBody] DTO.MessageDto messageDto)
        {
            try
            {
                var id = await this.mediator.Send(new Features.Message.Add(accountId, messageDto));

                return Ok(id);
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
