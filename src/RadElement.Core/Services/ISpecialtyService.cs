using RadElement.Core.Domain;
using System.Threading.Tasks;

namespace RadElement.Core.Services
{
    /// <summary>
    /// Business interface for handling the specialty related operations
    /// </summary>
    public interface ISpecialtyService
    {
        /// <summary>
        /// Gets the specialties.
        /// </summary>
        /// <returns></returns>
        Task<JsonResult> GetSpecialties();

        /// <summary>
        /// Gets the specialty.
        /// </summary>
        /// <param name="specialtyId">The specialty identifier.</param>
        /// <returns></returns>
        Task<JsonResult> GetSpecialty(int specialtyId);

        /// <summary>
        /// Searches the specialties.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        Task<JsonResult> SearchSpecialties(string searchKeyword);
    }
}
