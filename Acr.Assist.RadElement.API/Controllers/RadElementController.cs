using System;
using System.Threading.Tasks;
using System.Xml;
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

        #region SET

        /// <summary>
        /// Gets the cde sets.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("set")]
        public async Task<IActionResult> GetSets()
        {
            var result = await radElementService.GetSets();
            return Ok(result); ;
        }

        /// <summary>
        /// Gets the cde set by set identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("set/{setId:int}")]
        public async Task<IActionResult> GetSetBySetId(int setId)
        {
            var result = await radElementService.GetSet(setId);
            return Ok(result);
        }

        /// <summary>
        /// Searches the cde set.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("set/search")]
        public async Task<IActionResult> SearchSet(string searchKeyword)
        {
            var result = await radElementService.SearchSet(searchKeyword);
            return Ok(result); ;
        }

        /// <summary>
        /// Creates the set.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/XML")]
        [Route("set")]
        public async Task<IActionResult> CreateSet([FromBody]XmlElement content)
        {
            var result = await radElementService.CreateSet(content);
            return Ok(result); ;
        }

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        [HttpPut]
        [Consumes("application/XML")]
        [Route("set/{setId:int}")]
        public async Task<IActionResult> UpdateSet([FromBody]XmlElement content, int setId)
        {
            var result = await radElementService.UpdateSet(setId, content);
            return Ok(result); ;
        }

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("set/{setId:int}")]
        public async Task<IActionResult> DeleteSet(int setId)
        {
            var result = await radElementService.DeleteSet(setId);
            return Ok(result); ;
        }

        #endregion

        #region Element

        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("element")]
        public async Task<IActionResult> GetElements()
        {
            var result = await radElementService.GetElements();
            return Ok(result); ;
        }

        /// <summary>
        /// Gets the element by element identifier.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("element/{elementId:int}")]
        public async Task<IActionResult> GetElementByElementId(int elementId)
        {
            var result = await radElementService.GetElement(elementId);
            return Ok(result); ;
        }

        /// <summary>
        /// Gets the cde element by set identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("set/{setId:int}/element")]
        public async Task<IActionResult> GetElementBySetId(int setId)
        {
            var result = await radElementService.GetElementsBySetId(setId);
            return Ok(result); ;
        }

        /// <summary>
        /// Searches the cde element.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("element/search")]
        public async Task<IActionResult> SearchElement(string searchKeyword)
        {
            var result = await radElementService.SearchElement(searchKeyword);
            return Ok(result); ;
        }

        [HttpPost]
        [Consumes("application/XML")]
        [Route("set/{setId:int}/element")]
        public async Task<IActionResult> CreateElement(int setId, [FromBody]XmlElement content)
        {
            var result = await radElementService.CreateElement(setId, content);
            return Ok(result); ;
        }

        /// <summary>
        /// Updates the element.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        [HttpPut]
        [Consumes("application/XML")]
        [Route("set/{setId:int}/element/{elementId:int}")]
        public async Task<IActionResult> UpdateElement(int setId, int elementId, [FromBody]XmlElement content)
        {
            var result = await radElementService.UpdateElement(setId, elementId, content);
            return Ok(result); ;
        }

        /// <summary>
        /// Deletes the element.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("set/{setId:int}/element/{elementId:int}")]
        public async Task<IActionResult> DeleteElement(int setId, int elementId)
        {
            var result = await radElementService.DeleteElement(setId, elementId);
            return Ok(result); ;
        }

        #endregion

        #region Element & SET

        /// <summary>
        /// Inserts the content of the data by XML.
        /// </summary>
        /// <param name="xmlContent">Content of the XML.</param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/XML")]
        [Route("set/element")]
        public async Task<IActionResult> CreateElementSet([FromBody]XmlElement xmlContent)
        {
            var result = await radElementService.CreateElementSet(xmlContent);
            return Ok(result);
        }

        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name="moduleId">The module identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/XML")]
        [Route("set/element/{moduleId}/{userId}")]
        public async Task<IActionResult> InsertData(string moduleId, string userId)
        {
            await radElementService.CreateElementSet(new UserModule() { ModuleId = moduleId, UserId = userId, RoleId = "74919C0F-AE3A-483B-8923-CF814B01070C" });
            return Ok(); ;
        }

        #endregion
    }
}