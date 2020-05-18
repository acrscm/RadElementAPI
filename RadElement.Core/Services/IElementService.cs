using RadElement.Core.Domain;
using RadElement.Core.DTO;
using System.Threading.Tasks;

namespace RadElement.Core.Services
{
    /// <summary>
    /// Business interface for handling the element related operations
    /// </summary>
    public interface IElementService
    {
        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <returns></returns>
        Task<JsonResult> GetElements();

        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        Task<JsonResult> GetElement(string elementId);

        /// <summary>
        /// Gets the elements by set identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        Task<JsonResult> GetElementsBySetId(string setId);

        /// <summary>
        /// Searches the element.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        Task<JsonResult> SearchElements(string searchKeyword);

        /// <summary>
        /// Deeps the search elements.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        Task<JsonResult> DeepSearchElements(string searchKeyword, string operation);

        /// <summary>
        /// Simples the search elements.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        Task<JsonResult> SimpleSearchElements(string searchKeyword, string operation);

        /// <summary>
        /// Creates the element.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="dataElement">The data element.</param>
        /// <returns></returns>
        Task<JsonResult> CreateElement(string setId, CreateElement dataElement);

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="dataElement">The data element.</param>
        /// <returns></returns>
        Task<JsonResult> UpdateElement(string setId, string elementId, UpdateElement dataElement);

        /// <summary>
        /// Deletes the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        Task<JsonResult> DeleteElement(string setId, string elementId);
    }
}
