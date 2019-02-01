using System.Threading.Tasks;
using Acr.Assist.RadElement.Core.Domain;
using Acr.Assist.RadElement.Core.DTO;
using Acr.Assist.RadElement.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Acr.Assist.RadElement.API.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("radelement/api/v1")]
    public class ElementSetController : BaseController
    {
        private readonly IElementSetService elementSetService;
        private readonly ILogger<ElementSetController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementSetController"/> class.
        /// </summary>
        /// <param name="elementSetService">The element set service.</param>
        /// <param name="logger">The logger.</param>
        public ElementSetController(IElementSetService elementSetService, ILogger<ElementSetController> logger)
        {
            this.elementSetService = elementSetService;
            this.logger = logger;
        }
        
        /// <summary>
        /// Gets the cde sets.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("set")]
        public async Task<IActionResult> GetSets()
        {
            var result = await elementSetService.GetSets();
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
            var result = await elementSetService.GetSet(setId);
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
            var result = await elementSetService.SearchSet(searchKeyword);
            return Ok(result); ;
        }

        /// <summary>
        /// Creates the set.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("set")]
        public async Task<IActionResult> CreateSet([FromBody]CreateUpdateSet content)
        {
            var result = await elementSetService.CreateSet(content);
            return Ok(result); ;
        }

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("set/{setId:int}")]
        public async Task<IActionResult> UpdateSet(int setId, [FromBody]CreateUpdateSet content)
        {
            var result = await elementSetService.UpdateSet(setId, content);
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
            var result = await elementSetService.DeleteSet(setId);
            return Ok(result); ;
        }
    }
}