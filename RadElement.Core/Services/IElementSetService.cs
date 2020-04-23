using RadElement.Core.Domain;
using RadElement.Core.DTO;
using System.Threading.Tasks;

namespace RadElement.Core.Services
{
    /// <summary>
    /// Business interface for handling the element set related operations
    /// </summary>
    public interface IElementSetService
    {
        /// <summary>
        /// Gets the set.
        /// </summary>
        /// <returns></returns>
        Task<JsonResult> GetSets();

        /// <summary>
        /// Gets the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        Task<JsonResult> GetSet(string setId);

        /// <summary>
        /// Searches the cde set.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        Task<JsonResult> SearchSets(string searchKeyword);

        /// <summary>
        /// Creates the cde set.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        Task<JsonResult> CreateSet(CreateUpdateSet content);

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        Task<JsonResult> UpdateSet(string setId, CreateUpdateSet content);

        /// <summary>
        /// Deletes the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        Task<JsonResult> DeleteSet(string setId);
    }
}
