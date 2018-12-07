using System;
using System.Threading.Tasks;
using Acr.Assist.RadElement.Core.DTO;
using Acr.Assist.RadElement.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Acr.Assist.RadElement.API.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("radelement/api/v1")]
    public class RadElementController : BaseController
    {
        private readonly IRadElementService radElementService;
        private readonly ILogger<RadElementController> logger;

        /// <summary>
        /// Intializes Rad element service and logger.
        /// </summary>
        /// <param name="radElementService"></param>
        /// <param name="logger"></param>
        public RadElementController(IRadElementService radElementService, ILogger<RadElementController> logger)
        {
            this.radElementService = radElementService;
            this.logger = logger;
        }

        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name="moduleId">The module identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{moduleId}/{userId}")]
        public async Task<IActionResult> InsertData(string moduleId, string userId)
        {
            await radElementService.InsertData(new UserModule() { ModuleId = moduleId, UserId = userId, RoleId = "74919C0F-AE3A-483B-8923-CF814B01070C" });
            return Ok(); ;
        }

        /// <summary>
        /// Inserts the content of the data by XML.
        /// </summary>
        /// <param name="xmlContent">Content of the XML.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("xml")]
        public async Task<IActionResult> InsertData([FromBody]XmlContent xmlContent)
        { 
            await radElementService.InsertData(xmlContent);
            return Ok(); ;
        }
    }
}