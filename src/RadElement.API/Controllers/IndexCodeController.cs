using System.Threading.Tasks;
using RadElement.Core.DTO;
using RadElement.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RadElement.API.Controllers
{
    /// <summary>
    /// Endpoint for index code controller
    /// </summary>
    /// <seealso cref="RadElement.API.Controllers.BaseController" />
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("radelement/api/v1")]
    [Authorize(Policy = "UserIdExists")]
    [ApiController]
    public class IndexCodeController : BaseController
    {
        /// <summary>
        /// The index code service
        /// </summary>
        private readonly IIndexCodeService indexCodeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexCodeController" /> class.
        /// </summary>
        /// <param name="indexCodeService">The index code service.</param>
        /// <param name="logger">The logger.</param>
        public IndexCodeController(IIndexCodeService indexCodeService, ILogger<IndexCodeController> logger)
        {
            this.indexCodeService = indexCodeService;
            LoggerInstance = logger;
        }

        /// <summary>
        /// Gets the index codes.
        /// </summary>
        /// <returns></returns>
        [HttpGet("indexcodes")]
        public async Task<IActionResult> GetIndexCodes()
        {
            var result = await indexCodeService.GetIndexCodes();
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the index code.
        /// </summary>
        /// <param name="indexCodeId">The index code identifier.</param>
        /// <returns></returns>
        [HttpGet("indexcodes/{indexCodeId}")]
        public async Task<IActionResult> GetIndexCode(int indexCodeId)
        {
            var result = await indexCodeService.GetIndexCode(indexCodeId);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Searches the index codes.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        [HttpGet("indexcodes/search")]
        public async Task<IActionResult> SearchIndexCodes([FromQuery]string searchKeyword)
        {
            var result = await indexCodeService.SearchIndexCodes(searchKeyword);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates the index code.
        /// </summary>
        /// <param name="indexCode">The index code.</param>
        /// <returns></returns>
        [HttpPost("indexcodes")]
        public async Task<IActionResult> CreateIndexCode([FromBody]CreateUpdateIndexCode indexCode)
        {
            var result = await indexCodeService.CreateIndexCode(indexCode);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the index code.
        /// </summary>
        /// <param name="indexCodeId">The index code identifier.</param>
        /// <param name="indexCode">The index code.</param>
        /// <returns></returns>
        [HttpPut("indexcodes/{indexCodeId}")]
        public async Task<IActionResult> UpdateIndexCode(int indexCodeId, [FromBody]CreateUpdateIndexCode indexCode)
        {
            var result = await indexCodeService.UpdateIndexCode(indexCodeId, indexCode);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Deletes the index code.
        /// </summary>
        /// <param name="indexCodeId">The index code identifier.</param>
        /// <returns></returns>
        [HttpDelete("indexcodes/{indexCodeId}")]
        public async Task<IActionResult> DeleteIndexCode(int indexCodeId)
        {
            var result = await indexCodeService.DeleteIndexCode(indexCodeId);
            return StatusCode((int)result.Code, result.Value);
        }
    }
}