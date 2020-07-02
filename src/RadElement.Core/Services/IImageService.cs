using RadElement.Core.Domain;
using RadElement.Core.DTO;
using System.Threading.Tasks;

namespace RadElement.Core.Services
{
    /// <summary>
    /// Business interface for handling the image related operations
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Gets the images.
        /// </summary>
        /// <returns></returns>
        Task<JsonResult> GetImages();

        /// <summary>
        /// Gets the imagee.
        /// </summary>
        /// <param name="imageId">The image identifier.</param>
        /// <returns></returns>
        Task<JsonResult> GetImage(int imageId);

        /// <summary>
        /// Searches the images.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        Task<JsonResult> SearchImages(string searchKeyword);

        /// <summary>
        /// Creates the imagee.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        Task<JsonResult> CreateImage(CreateUpdateImage image);

        /// <summary>
        /// Updates the image.
        /// </summary>
        /// <param name="imageId">The image identifier.</param>
        /// <param name="reference">The reference.</param>
        /// <returns></returns>
        Task<JsonResult> UpdateImage(int imageId, CreateUpdateImage reference);

        /// <summary>
        /// Deletes the image.
        /// </summary>
        /// <param name="imageId">The image identifier.</param>
        /// <returns></returns>
        Task<JsonResult> DeleteImage(int imageId);
    }
}
