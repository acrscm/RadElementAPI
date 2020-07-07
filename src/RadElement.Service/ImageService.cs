using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Core.Services;
using Serilog;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RadElement.Core.Data;

namespace RadElement.Service
{
    /// <summary>
    /// Business service for handling the image related operations
    /// </summary>
    /// <seealso cref="RadElement.Core.Services.IImageService" />
    public class ImageService : IImageService
    {
        /// <summary>
        /// The RAD element database context
        /// </summary>
        private RadElementDbContext radElementDbContext;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageService"/> class.
        /// </summary>
        /// <param name="radElementDbContext">The RAD element database context.</param>
        /// <param name="logger">The logger.</param>
        public ImageService(
            RadElementDbContext radElementDbContext,
            ILogger logger)
        {
            this.radElementDbContext = radElementDbContext;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the images.
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetImages()
        {
            try
            {
                var images = radElementDbContext.Image.ToList();
                return await Task.FromResult(new JsonResult(images, HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetImages()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Gets the imagee.
        /// </summary>
        /// <param name="imageId">The image identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> GetImage(int imageId)
        {
            try
            {
                if (imageId != 0)
                {
                    var image = radElementDbContext.Image.Where(x => x.Id == imageId).FirstOrDefault();
                    if (image != null)
                    {
                        return await Task.FromResult(new JsonResult(image, HttpStatusCode.OK));
                    }
                }
                return await Task.FromResult(new JsonResult(string.Format("No such image with id '{0}'.", imageId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetImage(int imageId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Searches the images.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<JsonResult> SearchImages(string searchKeyword)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    var filteredImage = radElementDbContext.Image.Where(x => x.Caption.Contains(searchKeyword, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    if (filteredImage != null && filteredImage.Any())
                    {
                        return await Task.FromResult(new JsonResult(filteredImage, HttpStatusCode.OK));
                    }
                    else
                    {
                        return await Task.FromResult(new JsonResult(string.Format("No such image with keyword '{0}'.", searchKeyword), HttpStatusCode.NotFound));
                    }
                }

                return await Task.FromResult(new JsonResult("Keyword given is invalid.", HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'SearchImages(string searchKeyword)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Creates the imagee.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public async Task<JsonResult> CreateImage(CreateUpdateImage image)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (image == null)
                    {
                        return await Task.FromResult(new JsonResult("Image fields are invalid.", HttpStatusCode.BadRequest));
                    }

                    var matchedImage = radElementDbContext.Image.ToList().Where(
                        x => string.Equals(x.SourceUrl.Trim(), image.SourceUrl.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                             string.Equals(x.LocalUrl.Trim(), image.LocalUrl.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                             string.Equals(x.Caption.Trim(), image.Caption.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                             string.Equals(x.Rights.Trim(), image.Rights.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                             x.Width == image.Width && x.Height == image.Height).FirstOrDefault();

                    if (matchedImage != null)
                    {

                        var imageDetails = new ImageIdDetails()
                        {
                            ImageId = matchedImage.Id.ToString()
                        };

                        return await Task.FromResult(new JsonResult(imageDetails, HttpStatusCode.OK));
                    }

                    var imageData = new Image()
                    {
                        LocalUrl = image.LocalUrl,
                        Caption = image.Caption,
                        SourceUrl = image.SourceUrl,
                        Height = image.Height,
                        Width = image.Width,
                        Rights = image.Rights,
                    };

                    radElementDbContext.Image.Add(imageData);
                    radElementDbContext.SaveChanges();
                    transaction.Commit();

                    return await Task.FromResult(new JsonResult(new ImageIdDetails() { ImageId = imageData.Id.ToString() }, HttpStatusCode.Created));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'CreateImage(CreateUpdateImage image)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Updates the image.
        /// </summary>
        /// <param name="imageId">The image identifier.</param>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public async Task<JsonResult> UpdateImage(int imageId, CreateUpdateImage image)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (image == null)
                    {
                        return await Task.FromResult(new JsonResult("Image fields are invalid.", HttpStatusCode.BadRequest));
                    }

                    if (imageId != 0)
                    {
                        var imageDetails = radElementDbContext.Image.Where(x => x.Id == imageId).FirstOrDefault();

                        if (imageDetails != null)
                        {
                            imageDetails.LocalUrl = image.LocalUrl;
                            imageDetails.Caption = image.Caption;
                            imageDetails.SourceUrl = image.SourceUrl;
                            imageDetails.Height = image.Height;
                            imageDetails.Width = image.Width;
                            imageDetails.Rights = image.Rights;

                            radElementDbContext.SaveChanges();
                            transaction.Commit();

                            return await Task.FromResult(new JsonResult(string.Format("Image with id '{0}' is updated.", imageId), HttpStatusCode.OK));
                        }
                    }

                    return await Task.FromResult(new JsonResult(string.Format("No such image with id '{0}'.", imageId), HttpStatusCode.NotFound));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'UpdateImage(int imageId, CreateUpdateImage image)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Deletes the image.
        /// </summary>
        /// <param name="imageId">The image identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> DeleteImage(int imageId)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (imageId != 0)
                    {
                        var image = radElementDbContext.Image.Where(x => x.Id == imageId).FirstOrDefault();

                        if (image != null)
                        {
                            RemoveImages(image);

                            radElementDbContext.Image.Remove(image);
                            radElementDbContext.SaveChanges();
                            transaction.Commit();

                            return await Task.FromResult(new JsonResult(string.Format("Image with id '{0}' is deleted.", imageId), HttpStatusCode.OK));
                        }
                    }
                    return await Task.FromResult(new JsonResult(string.Format("No such image with id '{0}'.", imageId), HttpStatusCode.NotFound));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'DeleteImage(int imageId)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Removes the images.
        /// </summary>
        /// <param name="image">The image.</param>
        private void RemoveImages(Image image)
        {
            var imageRefs = radElementDbContext.ImageRef.Where(x => x.Image_Id == image.Id);
            if (imageRefs != null && imageRefs.Any())
            {
                radElementDbContext.ImageRef.RemoveRange(imageRefs);
                radElementDbContext.SaveChanges();
            }
        }
    }
}
