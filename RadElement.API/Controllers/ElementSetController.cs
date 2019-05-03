using System.Threading.Tasks;
using RadElement.Core.DTO;
using RadElement.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RadElement.API.Controllers
{
    /// <summary>
    /// Endpoint for elements set controller
    /// </summary>
    /// <seealso cref="RadElement.API.Controllers.BaseController" />
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("radelement/api/v1")]
    [Authorize(Policy = "UserIdExists")]
    [ApiController]
    public class ElementSetController : BaseController
    {
        private readonly IElementSetService elementSetService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementSetController"/> class.
        /// </summary>
        /// <param name="elementSetService">The element set service.</param>
        /// <param name="logger">The logger.</param>
        public ElementSetController(IElementSetService elementSetService, ILogger<ElementSetController> logger)
        {
            this.elementSetService = elementSetService;
            LoggerInstance = logger;
        }
        
        /// <summary>
        /// Fetches all the sets.
        /// </summary>
        /// <returns></returns>
        [HttpGet("sets")]
        public async Task<IActionResult> GetSets()
        {
            var result = await elementSetService.GetSets();
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Fetches the set by set identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        [HttpGet("sets/{setId}")]
        public async Task<IActionResult> GetSetBySetId(string setId)
        {
            var result = await elementSetService.GetSet(setId);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Searches the set with provided keyword.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        [HttpGet("sets/search")]
         public async Task<IActionResult> SearchSet(string searchKeyword)
        {
            var result = await elementSetService.SearchSet(searchKeyword);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates a set.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        [HttpPost("sets")]
        public async Task<IActionResult> CreateSet([FromBody]CreateUpdateSet content)
        {
            var result = await elementSetService.CreateSet(content);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates a set by set identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        [HttpPut("sets/{setId}")]
        public async Task<IActionResult> UpdateSet(string setId, [FromBody]CreateUpdateSet content)
        {
            var result = await elementSetService.UpdateSet(setId, content);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Removes a set by set identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        [HttpDelete("sets/{setId}")]
        public async Task<IActionResult> DeleteSet(string setId)
        {
            var result = await elementSetService.DeleteSet(setId);
            return StatusCode((int)result.Code, result.Value);
        }
    }
}