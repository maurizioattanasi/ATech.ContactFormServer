using System;
using System.Threading.Tasks;
using ATech.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ATech.ContactFormServer.Api.Controllers
{
    /// <summary>
    /// Account controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        public AccountController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Registers a new account item
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]/{email}")]
        public async Task<ActionResult<Guid>> Register([FromRoute] string email)
        {
            try
            {
                var id = await this.mediator.Send(new Features.Account.Add(email));

                return Ok(id);
            }
            catch (HttpException ex)
            {
                string content = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                var problemDetails = new ProblemDetails()
                {
                    Status = ex.StatusCode,
                    Detail = content,
                    Instance = $"/Register",
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
                    Instance = $"/Register/",
                };

                return BadRequest(problemDetails);
            }
        }
    }
}
