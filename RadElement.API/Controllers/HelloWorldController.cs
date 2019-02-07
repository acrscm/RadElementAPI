using Microsoft.AspNetCore.Mvc;

namespace RadElement.API.Controllers
{
    /// <summary>
    /// Endpoint for hello worls controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Produces("application/json")]
    [Route("api/HelloWorld")]
    public class HelloWorldController : Controller
    {
        /// <summary>
        /// Method used to check if service is running
        /// </summary>
        /// <returns>Returns "Hello World" </returns>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet]
        public IActionResult HelloWorld()
        {
            return Ok("Hello  World");
        }
    }
}