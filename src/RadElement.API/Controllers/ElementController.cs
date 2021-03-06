﻿using System.Threading.Tasks;
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
    [Authorize(Policy = "UserIdExists")]
    [ApiController]
    public class ElementController : BaseController
    {
        /// <summary>
        /// The RAD element service
        /// </summary>
        private readonly IElementService radElementService;

        /// <summary>
        /// Intializes Rad element service and logger.
        /// </summary>
        /// <param name="radElementService"></param>
        /// <param name="logger"></param>
        public ElementController(
            IElementService radElementService, 
            ILogger<ElementController> logger)
        {
            this.radElementService = radElementService;
            LoggerInstance = logger;
        }

        /// <summary>
        /// Fetches all the elements.
        /// </summary>
        /// <returns></returns>
        [HttpGet("elements")]
        public async Task<IActionResult> GetElements()
        {
            var result = await radElementService.GetElements();
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Fetch a element by element identifier.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        [HttpGet("elements/{elementId}")]
        public async Task<IActionResult> GetElementByElementId(string elementId)
        {
            var result = await radElementService.GetElement(elementId);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Fetch the elements by set identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        [HttpGet("sets/{setId}/elements")]
        public async Task<IActionResult> GetElementsBySetId(string setId)
        {
            var result = await radElementService.GetElementsBySetId(setId);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Searches the element with provided keyword.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        [HttpGet("elements/search")]
        public async Task<IActionResult> SearchElements([FromQuery]string searchKeyword)
        {
            var result = await radElementService.SearchElements(searchKeyword);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates a element under specific set identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        [HttpPost("sets/{setId}/elements")]
        public async Task<IActionResult> CreateElement(string setId, [FromBody]CreateElement content)
        {
            var result = await radElementService.CreateElement(setId, content);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the element based on set identifier and element identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        [HttpPut("sets/{setId}/elements/{elementId}")]
        public async Task<IActionResult> UpdateElement(string setId, string elementId, [FromBody]UpdateElement content)
        {
            var result = await radElementService.UpdateElement(setId, elementId, content);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Deletes the element based on set identifier and element identifier..
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        [HttpDelete("sets/{setId}/elements/{elementId}")]
        public async Task<IActionResult> DeleteElement(string setId, string elementId)
        {
            var result = await radElementService.DeleteElement(setId, elementId);
            return StatusCode((int)result.Code, result.Value);
        }
    }
}