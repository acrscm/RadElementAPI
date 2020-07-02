using System.Threading.Tasks;
using RadElement.Core.DTO;
using RadElement.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RadElement.API.Controllers
{
    /// <summary>
    /// Endpoint for reference controller
    /// </summary>
    /// <seealso cref="RadElement.API.Controllers.BaseController" />
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("radelement/api/v1")]
    [Authorize(Policy = "UserIdExists")]
    [ApiController]
    public class ImageController : BaseController
    {
        /// <summary>
        /// The image service
        /// </summary>
        private readonly IImageService imageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageController"/> class.
        /// </summary>
        /// <param name="imageService">The image service.</param>
        /// <param name="logger">The logger.</param>
        public ImageController(IImageService imageService, ILogger<ImageController> logger)
        {
            this.imageService = imageService;
            LoggerInstance = logger;
        }

        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <returns></returns>
        [HttpGet("images")]
        public async Task<IActionResult> GetImages()
        {
            var result = await imageService.GetImages();
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="imageId">The image identifier.</param>
        /// <returns></returns>
        [HttpGet("images/{imageId}")]
        public async Task<IActionResult> GetImage(int imageId)
        {
            var result = await imageService.GetImage(imageId);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Searches the images.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        [HttpGet("images/search")]
        public async Task<IActionResult> SearchImages([FromQuery]string searchKeyword)
        {
            var result = await imageService.SearchImages(searchKeyword);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        [HttpPost("images")]
        public async Task<IActionResult> CreateImage([FromBody]CreateUpdateImage image)
        {
            var result = await imageService.CreateImage(image);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the image.
        /// </summary>
        /// <param name="imageId">The image identifier.</param>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        [HttpPut("images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromBody]CreateUpdateImage image)
        {
            var result = await imageService.UpdateImage(imageId, image);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Deletes the image.
        /// </summary>
        /// <param name="imageId">The image identifier.</param>
        /// <returns></returns>
        [HttpDelete("images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var result = await imageService.DeleteImage(imageId);
            return StatusCode((int)result.Code, result.Value);
        }
    }
}