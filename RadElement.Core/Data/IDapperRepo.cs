using RadElement.Core.Domain;
using RadElement.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RadElement.Core.Data
{
    public interface IDapperRepo
    {
        /// <summary>
        /// Gets the elements details.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        Task<List<Element>> SearchElementsDetails(string searchKeyword);

        /// <summary>
        /// Searches the elements.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        Task<List<BasicElementDetails>> SearchBasicElementsDetails(string searchKeyword);

        /// <summary>
        /// Gets the element identifier.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        Task<List<ElementSet>> GetSetsByElementId(int elementId);

        /// <summary>
        /// Gets the element values by element identifier.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        Task<List<ElementValue>> GetElementValuesByElementId(int elementId);

        /// <summary>
        /// Gets the person.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <returns></returns>
        Task<Person> GetPerson(int personId);

        /// <summary>
        /// Gets the organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        Task<Organization> GetOrganization(int organizationId);

        /// <summary>
        /// Gets the person roles by element identifier.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        Task<List<PersonRoleElementRef>> GetPersonRolesByElementId(int elementId);

        /// <summary>
        /// Gets the organization roles by element identifier.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        Task<List<OrganizationRoleElementRef>> GetOrganizationRolesByElementId(int elementId);
    }
}
