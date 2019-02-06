using System.Threading.Tasks;
using Acr.Assist.RadElement.Core.Domain;
using Acr.Assist.RadElement.Core.DTO;
using Acr.Assist.RadElement.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Acr.Assist.RadElement.API.Controllers
{
    /// <summary>
    /// Endpoint for elements controller
    /// </summary>
    /// <seealso cref="Acr.Assist.RadElement.API.Controllers.BaseController" />
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("radelement/api/v1")]
    [Authorize]
    public class ElementController : BaseController
    {
        private readonly IElementService radElementService;
        private readonly ILogger<ElementController> logger;

        /// <summary>
        /// Intializes Rad element service and logger.
        /// </summary>
        /// <param name="radElementService"></param>
        /// <param name="logger"></param>
        public ElementController(IElementService radElementService, ILogger<ElementController> logger)
        {
            this.radElementService = radElementService;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the elements.
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

        /// <summary>
        /// Creates the element.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementType">Type of the element.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("set/{setId:int}/element/elementType")]
        public async Task<IActionResult> CreateElement(int setId, DataElementType elementType, [FromBody]CreateUpdateElement content)
        {
            var result = await radElementService.CreateElement(setId, elementType, content);
            return Ok(result); ;
        }

        /// <summary>
        /// Updates the element.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="elementType">Type of the element.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("set/{setId:int}/element/{elementId:int}/elementType")]
        public async Task<IActionResult> UpdateElement(int setId, int elementId, DataElementType elementType, [FromBody]CreateUpdateElement content)
        {
            var result = await radElementService.UpdateElement(setId, elementId, elementType, content);
            return Ok(result); ;
        }

        /// <summary>
        /// Deletes the element.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("set/{setId:int}/element/{elementId:int}")]
        public async Task<IActionResult> DeleteElement(int setId, int elementId)
        {
            var result = await radElementService.DeleteElement(setId, elementId);
            return Ok(result); ;
        }
    }
}