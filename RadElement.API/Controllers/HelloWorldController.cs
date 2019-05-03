using Microsoft.AspNetCore.Mvc;

namespace RadElement.API.Controllers
{
    /// <summary>
    /// Endpoint for hello worls controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Produces("application/json")]
    [Route("api/HelloWorld")]
    [ApiController]
    public class HelloWorldController : Controller
    {
        /// <summary>
        /// Method used to check if service is running
        /// </summary>
        /// <returns>Returns "Hello World" </returns>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet]
        public IActionResult GetHelloWorld()
        {
            return Ok("Get Hello World Success");
        }

        /// <summary>
        /// Method used to check if service is running
        /// </summary>
        /// <returns>Returns "Hello World" </returns>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost]
        public IActionResult PostHelloWorld()
        {
            return Ok("POST Hello World without Route Success");
        }

        /// <summary>
        /// Method used to check if service is running
        /// </summary>
        /// <returns>Returns "Hello World" </returns>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("test")]
        public IActionResult PostHelloWorldWithRoute()
        {
            return Ok("POST Hello World with Route Success");
        }
    }
}