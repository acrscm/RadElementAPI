using RadElement.Core.Domain;
using RadElement.Core.DTO;
using System.Threading.Tasks;

namespace RadElement.Core.Services
{
    /// <summary>
    /// Business interface for handling the organisation related operations
    /// </summary>
    public interface IOrganizationService
    {
        /// <summary>
        /// Gets the organization.
        /// </summary>
        /// <returns></returns>
        Task<JsonResult> GetOrganizations();

        /// <summary>
        /// Gets the organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        Task<JsonResult> GetOrganization(int organizationId);

        /// <summary>
        /// Searches the organization.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        Task<JsonResult> SearchOrganization(SearchKeyword searchKeyword);

        /// <summary>
        /// Creates the organization.
        /// </summary>
        /// <param name="organization">The organization.</param>
        /// <returns></returns>
        Task<JsonResult> CreateOrganization(CreateUpdateOrganization organization);

        /// <summary>
        /// Updates the organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="organization">The organization.</param>
        /// <returns></returns>
        Task<JsonResult> UpdateOrganization(int organizationId, CreateUpdateOrganization organization);

        /// <summary>
        /// Deletes the organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        Task<JsonResult> DeleteOrganization(int organizationId);
    }
}
