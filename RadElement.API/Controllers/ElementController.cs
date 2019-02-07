using System.Threading.Tasks;
using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace RadElement.API.Controllers
{
    /// <summary>
    /// Endpoint for elements controller
    /// </summary>
    /// <seealso cref="RadElement.API.Controllers.BaseController" />
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("radelement/api/v1")]
    [Authorize]
    public class ElementController : BaseController
    {
        private readonly IElementService radElementService;

        /// <summary>
        /// Intializes Rad element service and logger.
        /// </summary>
        /// <param name="radElementService"></param>
        /// <param name="logger"></param>
        public ElementController(IElementService radElementService, ILogger<ElementController> logger)
        {
            this.radElementService = radElementService;
            LoggerInstance = logger;
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
            return SendResult(result);
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
            return SendResult(result);
        }

        /// <summary>
        /// Gets the element by set identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("set/{setId:int}/element")]
        public async Task<IActionResult> GetElementsBySetId(int setId)
        {
            var result = await radElementService.GetElementsBySetId(setId);
            return SendResult(result);
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
            return SendResult(result);
        }

        /// <summary>
        /// Creates the element.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementType">Type of the element.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("set/{setId:int}/element/{elementType}")]
        public async Task<IActionResult> CreateElement(int setId, DataElementType elementType, [FromBody]CreateUpdateElement content)
        {
            var result = await radElementService.CreateElement(setId, elementType, content);
            return SendResult(result);
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
        [Route("set/{setId:int}/element/{elementId:int}/{elementType}")]
        public async Task<IActionResult> UpdateElement(int setId, int elementId, DataElementType elementType, [FromBody]CreateUpdateElement content)
        {
            var result = await radElementService.UpdateElement(setId, elementId, elementType, content);
            return SendResult(result);
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
            return SendResult(result);
        }
    }
}