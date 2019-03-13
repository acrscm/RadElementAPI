using System.Threading.Tasks;
using System.Xml;
using RadElement.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RadElement.API.Controllers
{
    /// <summary>
    /// Endpoint for modules controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Consumes("application/XML")]
    [Produces("application/json")]
    [Route("radelement/api/v1")]
    [Authorize(Policy = "UserIdExists")]
    public class ModuleController : BaseController
    {
        private readonly IModuleService moduleService;

        /// <summary>
        /// Intializes Rad element service and logger.
        /// </summary>
        /// <param name="radElementService"></param>
        /// <param name="logger"></param>
        public ModuleController(IModuleService radElementService, ILogger<ModuleController> logger)
        {
            this.moduleService = radElementService;
            LoggerInstance = logger;
        }

        /// <summary>
        /// Inserts the elements and sets with the provided XML.
        /// </summary>
        /// <param name="xmlContent">Content of the XML.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("module")]
        public async Task<IActionResult> CreateModule([FromBody]XmlElement xmlContent)
        {
            var result = await moduleService.CreateModule(xmlContent);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updated the elements and sets with the provided Xml
        /// </summary>
        /// <param name="xmlContent">Content of the XML.</param>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("module/{setId}")]
        public async Task<IActionResult> UpdateModule([FromBody]XmlElement xmlContent, string setId)
        {
            var result = await moduleService.UpdateModule(xmlContent, setId);
            return StatusCode((int)result.Code, result.Value);
        }
    }
}