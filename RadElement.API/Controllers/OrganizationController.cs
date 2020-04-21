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
    //[Authorize(Policy = "UserIdExists")]
    [ApiController]
    public class OrganizationController : BaseController
    {
        /// <summary>
        /// The organization service
        /// </summary>
        private readonly IOrganizationService organizationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationController" /> class.
        /// </summary>
        /// <param name="organizationService">The organization service.</param>
        /// <param name="logger">The logger.</param>
        public OrganizationController(IOrganizationService organizationService, ILogger<OrganizationController> logger)
        {
            this.organizationService = organizationService;
            LoggerInstance = logger;
        }

        /// <summary>
        /// Gets the organizations.
        /// </summary>
        /// <returns></returns>
        [HttpGet("organizations")]
        public async Task<IActionResult> GetOrganizations()
        {
            var result = await organizationService.GetOrganizations();
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        [HttpGet("organizations/{organizationId}")]
        public async Task<IActionResult> GetOrganization(int organizationId)
        {
            var result = await organizationService.GetOrganization(organizationId);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the organizations by set identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        [HttpGet("organizations/{setId}")]
        public async Task<IActionResult> GetOrganizationBySetId(string setId)
        {
            var result = await organizationService.GetOrganizationBySetId(setId);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the organizations by element identifier.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        [HttpGet("organizations/{elementId}")]
        public async Task<IActionResult> GetOrganizationByElementId(string elementId)
        {
            var result = await organizationService.GetOrganizationByElementId(elementId);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Searches the organizations.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        [HttpGet("organizations/search")]
        public async Task<IActionResult> SearchOrganization([FromQuery]SearchKeyword searchKeyword)
        {
            var result = await organizationService.SearchOrganization(searchKeyword);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates the organization.
        /// </summary>
        /// <param name="organization">The organization.</param>
        /// <returns></returns>
        [HttpPost("organizations")]
        public async Task<IActionResult> CreateOrganization([FromBody]CreateUpdateOrganization organization)
        {
            var result = await organizationService.CreateOrganization(organization);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="organization">The organization.</param>
        /// <returns></returns>
        [HttpPut("organizations/{organizationId}")]
        public async Task<IActionResult> UpdateOrganization(int organizationId, [FromBody]CreateUpdateOrganization organization)
        {
            var result = await organizationService.UpdateOrganization(organizationId, organization);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Deletes the organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        [HttpDelete("organizations/{organizationId}")]
        public async Task<IActionResult> DeleteOrganization(int organizationId)
        {
            var result = await organizationService.DeleteOrganization(organizationId);
            return StatusCode((int)result.Code, result.Value);
        }
    }
}