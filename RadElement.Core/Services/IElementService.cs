using Acr.Assist.RadElement.Core.Domain;
using Acr.Assist.RadElement.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acr.Assist.RadElement.Core.Services
{
    public interface IElementService
    {
        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <returns></returns>
        Task<List<Element>> GetElements();

        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        Task<Element> GetElement(int elementId);

        /// <summary>
        /// Gets the elements by set identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        Task<List<Element>> GetElementsBySetId(int setId);

        /// <summary>
        /// Searches the element.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        Task<List<Element>> SearchElement(string searchKeyword);

        /// <summary>
        /// Creates the element.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementType">Type of the element.</param>
        /// <param name="dataElement">The data element.</param>
        /// <returns></returns>
        Task<ElementIdDetails> CreateElement(int setId, DataElementType elementType, CreateUpdateElement dataElement);

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="elementType">Type of the element.</param>
        /// <param name="dataElement">The data element.</param>
        /// <returns></returns>
        Task<bool> UpdateElement(int setId, int elementId, DataElementType elementType, CreateUpdateElement dataElement);

        /// <summary>
        /// Deletes the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        Task<bool> DeleteElement(int setId, int elementId);
    }
}
