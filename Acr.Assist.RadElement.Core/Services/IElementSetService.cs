using Acr.Assist.RadElement.Core.Domain;
using Acr.Assist.RadElement.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acr.Assist.RadElement.Core.Services
{
    public interface IElementSetService
    {
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
        Task<SetIdDetails> CreateSet(CreateUpdateSet content);

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        Task<bool> UpdateSet(int setId, CreateUpdateSet content);

        /// <summary>
        /// Deletes the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        Task<bool> DeleteSet(int setId);
    }
}
