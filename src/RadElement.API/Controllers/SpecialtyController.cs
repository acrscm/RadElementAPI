using System.Threading.Tasks;
using RadElement.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RadElement.API.Controllers
{
    /// <summary>
    /// Endpoint for specialty controller
    /// </summary>
    /// <seealso cref="RadElement.API.Controllers.BaseController" />
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("radelement/api/v1")]
    [Authorize(Policy = "UserIdExists")]
    [ApiController]
    public class SpecialtyController : BaseController
    {
        /// <summary>
        /// The person service
        /// </summary>
        private readonly ISpecialtyService specialtyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialtyController"/> class.
        /// </summary>
        /// <param name="specialtyService">The specialty service.</param>
        /// <param name="logger">The logger.</param>
        public SpecialtyController(ISpecialtyService specialtyService, ILogger<PersonController> logger)
        {
            this.specialtyService = specialtyService;
            LoggerInstance = logger;
        }

        /// <summary>
        /// Gets the specialties.
        /// </summary>
        /// <returns></returns>
        [HttpGet("specialties")]
        public async Task<IActionResult> GetSpecialties()
        {
            var result = await specialtyService.GetSpecialties();
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the specialty.
        /// </summary>
        /// <param name="specialtyId">The specialty identifier.</param>
        /// <returns></returns>
        [HttpGet("specialties/{specialtyId}")]
        public async Task<IActionResult> GetSpecialty(int specialtyId)
        {
            var result = await specialtyService.GetSpecialty(specialtyId);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Searches the specialties.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        [HttpGet("specialties/search")]
        public async Task<IActionResult> SearchSpecialties([FromQuery]string searchKeyword)
        {
            var result = await specialtyService.SearchSpecialties(searchKeyword);
            return StatusCode((int)result.Code, result.Value);
        }
    }
}