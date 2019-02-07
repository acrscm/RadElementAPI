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
    }
}