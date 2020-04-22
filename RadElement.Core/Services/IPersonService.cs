using RadElement.Core.Domain;
using RadElement.Core.DTO;
using System.Threading.Tasks;

namespace RadElement.Core.Services
{
    /// <summary>
    /// Business interface for handling the person related operations
    /// </summary>
    public interface IPersonService
    {
        /// <summary>
        /// Gets the persons.
        /// </summary>
        /// <returns></returns>
        Task<JsonResult> GetPersons();

        /// <summary>
        /// Gets the person.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <returns></returns>
        Task<JsonResult> GetPerson(int personId);

        /// <summary>
        /// Searches the person.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        Task<JsonResult> SearchPersons(SearchKeyword searchKeyword);

        /// <summary>
        /// Creates the person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        Task<JsonResult> CreatePerson(CreateUpdatePerson person);

        /// <summary>
        /// Updates the person.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        Task<JsonResult> UpdatePerson(int personId, CreateUpdatePerson person);

        /// <summary>
        /// Deletes the person.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <returns></returns>
        Task<JsonResult> DeletePerson(int personId);
    }
}
