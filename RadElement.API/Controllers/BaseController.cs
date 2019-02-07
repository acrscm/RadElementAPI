using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace RadElement.API.Controllers
{
    /// <summary>
    /// Base  class for all controllers
    /// </summary>
    [Produces("application/json")]
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Instance of the Logger
        /// </summary>
        protected ILogger<BaseController> LoggerInstance { get; set; }

        /// <summary>
        /// Sends the result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public IActionResult SendResult(Core.Domain.JsonResult result)
        {
            if (result.Code == HttpStatusCode.Created)
            {
                return Created("", result.Value);
            }
            else if (result.Code == HttpStatusCode.NotFound)
            {
                return NotFound(result.Value);
            }
            else if (result.Code == HttpStatusCode.BadRequest)
            {
                return BadRequest(result.Value);
            }
            else if (result.Code == HttpStatusCode.InternalServerError)
            {
                return StatusCode(500);
            }

            return Ok(result.Value);
        }
    }
}