using RadElement.Core.Domain;
using RadElement.Core.DTO;
using System.Threading.Tasks;

namespace RadElement.Core.Services
{
    /// <summary>
    /// Business interface for handling the index code related operations
    /// </summary>
    public interface IIndexCodeService
    {
        /// <summary>
        /// Gets the index codes.
        /// </summary>
        /// <returns></returns>
        Task<JsonResult> GetIndexCodes();

        /// <summary>
        /// Gets the index code.
        /// </summary>
        /// <param name="indexCodeId">The index identifier.</param>
        /// <returns></returns>
        Task<JsonResult> GetIndexCode(int indexCodeId);

        /// <summary>
        /// Searches the index codes.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        Task<JsonResult> SearchIndexCodes(string searchKeyword);

        /// <summary>
        /// Creates the index code.
        /// </summary>
        /// <param name="indexCode">The index code.</param>
        /// <returns></returns>
        Task<JsonResult> CreateIndexCode(CreateUpdateIndexCode indexCode);

        /// <summary>
        /// Updates the index code.
        /// </summary>
        /// <param name="indexCodeId">The index identifier.</param>
        /// <param name="indexCode">The index code.</param>
        /// <returns></returns>
        Task<JsonResult> UpdateIndexCode(int indexCodeId, CreateUpdateIndexCode indexCode);

        /// <summary>
        /// Deletes the index code.
        /// </summary>
        /// <param name="indexCodeId">The index identifier.</param>
        /// <returns></returns>
        Task<JsonResult> DeleteIndexCode(int indexCodeId);
    }
}
