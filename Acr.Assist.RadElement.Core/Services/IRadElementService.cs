using Acr.Assist.RadElement.Core.Domain;
using Acr.Assist.RadElement.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace Acr.Assist.RadElement.Core.Services
{
    public interface IRadElementService
    {
        #region Set

        /// <summary>
        /// Gets the set.
        /// </summary>
        /// <returns></returns>
        Task<List<ElementSet>> GetSets();

        /// <summary>
        /// Gets the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        Task<ElementSet> GetSet(int setId);

        /// <summary>
        /// Searches the cde set.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        Task<List<ElementSet>> SearchSet(string searchKeyword);

        /// <summary>
        /// Creates the cde set.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        Task<string> CreateSet(XmlElement content);

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        Task<bool> UpdateSet(int setId, XmlElement content);

        /// <summary>
        /// Deletes the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        Task<bool> DeleteSet(int setId);

        #endregion

        #region Element

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
        /// <param name="content">The content.</param>
        /// <returns></returns>
        Task<List<int>> CreateElement(int setId, XmlElement content);

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        Task<bool> UpdateElement(int setId, int elementId, XmlElement content);

        /// <summary>
        /// Deletes the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        Task<bool> DeleteElement(int setId, int elementId);

        #endregion

        /// <summary>
        /// Inserts the Rad element data by module Id
        /// </summary>
        /// <param name="xmlContent">Content of the XML.</param>
        /// <returns></returns>
        Task<string> CreateElementSet(XmlElement xmlContent);

        /// <summary>
        /// Creates the element set.
        /// </summary>
        /// <param name="userModule">The user module.</param>
        /// <returns></returns>
        Task<string> CreateElementSet(UserModule userModule);
    }
}
