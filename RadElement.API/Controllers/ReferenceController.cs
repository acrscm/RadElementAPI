using System.Threading.Tasks;
using RadElement.Core.DTO;
using RadElement.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RadElement.API.Controllers
{
    /// <summary>
    /// Endpoint for reference controller
    /// </summary>
    /// <seealso cref="RadElement.API.Controllers.BaseController" />
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("radelement/api/v1")]
    [Authorize(Policy = "UserIdExists")]
    [ApiController]
    public class ReferenceController : BaseController
    {
        /// <summary>
        /// The reference service
        /// </summary>
        private readonly IReferenceService referenceService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceController" /> class.
        /// </summary>
        /// <param name="referenceService">The reference service.</param>
        /// <param name="logger">The logger.</param>
        public ReferenceController(IReferenceService referenceService, ILogger<ReferenceController> logger)
        {
            this.referenceService = referenceService;
            LoggerInstance = logger;
        }

        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <returns></returns>
        [HttpGet("references")]
        public async Task<IActionResult> GetReferences()
        {
            var result = await referenceService.GetReferences();
            return StatusCode((int)result.Code, result.Value);
        }
        /// <summary>
        /// Gets the reference.
        /// </summary>
        /// <param name="referencesId">The references identifier.</param>
        /// <returns></returns>
        [HttpGet("references/{referencesId}")]
        public async Task<IActionResult> GetReference(int referencesId)
        {
            var result = await referenceService.GetReference(referencesId);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Searches the references.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        [HttpGet("references/search")]
        public async Task<IActionResult> SearchReferences([FromQuery]string searchKeyword)
        {
            var result = await referenceService.SearchReferences(searchKeyword);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates the reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <returns></returns>
        [HttpPost("references")]
        public async Task<IActionResult> CreateReference([FromBody]CreateUpdateReference reference)
        {
            var result = await referenceService.CreateReference(reference);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the reference.
        /// </summary>
        /// <param name="referencesId">The references identifier.</param>
        /// <param name="reference">The reference.</param>
        /// <returns></returns>
        [HttpPut("references/{referencesId}")]
        public async Task<IActionResult> UpdateReference(int referencesId, [FromBody]CreateUpdateReference reference)
        {
            var result = await referenceService.UpdateReference(referencesId, reference);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Deletes the reference.
        /// </summary>
        /// <param name="referencesId">The references identifier.</param>
        /// <returns></returns>
        [HttpDelete("references/{referencesId}")]
        public async Task<IActionResult> DeleteReference(int referencesId)
        {
            var result = await referenceService.DeleteReference(referencesId);
            return StatusCode((int)result.Code, result.Value);
        }
    }
}