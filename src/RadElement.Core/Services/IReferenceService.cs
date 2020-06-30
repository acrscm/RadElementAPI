using RadElement.Core.Domain;
using RadElement.Core.DTO;
using System.Threading.Tasks;

namespace RadElement.Core.Services
{
    /// <summary>
    /// Business interface for handling the reference related operations
    /// </summary>
    public interface IReferenceService
    {
        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <returns></returns>
        Task<JsonResult> GetReferences();

        /// <summary>
        /// Gets the reference.
        /// </summary>
        /// <param name="referenceId">The reference identifier.</param>
        /// <returns></returns>
        Task<JsonResult> GetReference(int referenceId);

        /// <summary>
        /// Searches the references.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        Task<JsonResult> SearchReferences(string searchKeyword);

        /// <summary>
        /// Creates the reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <returns></returns>
        Task<JsonResult> CreateReference(CreateUpdateReference reference);

        /// <summary>
        /// Updates the reference.
        /// </summary>
        /// <param name="referenceId">The reference identifier.</param>
        /// <param name="reference">The reference.</param>
        /// <returns></returns>
        Task<JsonResult> UpdateReference(int referenceId, CreateUpdateReference reference);

        /// <summary>
        /// Deletes the reference.
        /// </summary>
        /// <param name="referenceId">The reference identifier.</param>
        /// <returns></returns>
        Task<JsonResult> DeleteReference(int referenceId);
    }
}
