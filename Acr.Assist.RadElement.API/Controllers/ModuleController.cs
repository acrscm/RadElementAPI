using System.Threading.Tasks;
using System.Xml;
using Acr.Assist.RadElement.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Acr.Assist.RadElement.API.Controllers
{
    [Consumes("application/XML")]
    [Produces("application/json")]
    [Route("radelement/api/v1")]
    public class ModuleController : Controller
    {
        private readonly IModuleService moduleService;
        private readonly ILogger<ModuleController> logger;

        /// <summary>
        /// Intializes Rad element service and logger.
        /// </summary>
        /// <param name="radElementService"></param>
        /// <param name="logger"></param>
        public ModuleController(IModuleService radElementService, ILogger<ModuleController> logger)
        {
            this.moduleService = radElementService;
            this.logger = logger;
        }

        /// <summary>
        /// Inserts the content of the data by XML.
        /// </summary>
        /// <param name="xmlContent">Content of the XML.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("module")]
        public async Task<IActionResult> CreateModule([FromBody]XmlElement xmlContent)
        {
            var result = await moduleService.CreateModule(xmlContent);
            return Ok(result);
        }

        /// <summary>
        /// Updates the module.
        /// </summary>
        /// <param name="xmlContent">Content of the XML.</param>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("module/{setId}")]
        public async Task<IActionResult> UpdateModule([FromBody]XmlElement xmlContent, int setId)
        {
            var result = await moduleService.UpdateModule(xmlContent, setId);
            return Ok(result); ;
        }
    }
}