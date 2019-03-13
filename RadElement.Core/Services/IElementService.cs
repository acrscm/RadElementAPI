using RadElement.Core.Domain;
using RadElement.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RadElement.Core.Services
{
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
        Task<JsonResult> SearchElement(string searchKeyword);

        /// <summary>
        /// Creates the element.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementType">Type of the element.</param>
        /// <param name="dataElement">The data element.</param>
        /// <returns></returns>
        Task<JsonResult> CreateElement(string setId, DataElementType elementType, CreateUpdateElement dataElement);

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="elementType">Type of the element.</param>
        /// <param name="dataElement">The data element.</param>
        /// <returns></returns>
        Task<JsonResult> UpdateElement(string setId, string elementId, DataElementType elementType, CreateUpdateElement dataElement);

        /// <summary>
        /// Deletes the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        Task<JsonResult> DeleteElement(string setId, string elementId);
    }
}
