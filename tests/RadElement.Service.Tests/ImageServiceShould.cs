
using Microsoft.EntityFrameworkCore;
using Moq;
using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Core.Data;
using RadElement.Service.Tests.Mocks.Data;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RadElement.Core.Infrastructure;

namespace RadElement.Service.Tests
{
    public class ImageServiceShould
    {
        /// <summary>
        /// The service
        /// </summary>
        private readonly ImageService service;

        /// <summary>
        /// The mock RAD element context
        /// </summary>
        private readonly Mock<RadElementDbContext> mockRadElementContext;

        /// <summary>
        /// The mock logger
        /// </summary>
        private readonly Mock<ILogger> mockLogger;

        private const string connectionString = "server=localhost;user id=root;password=root;persistsecurityinfo=True;database=radelement;Convert Zero Datetime=True";
        private const string imageNotFoundMessage = "No such image with id '{0}'.";
        private const string imageNotFoundMessageWithSearchMessage = "No such image with keyword '{0}'.";
        private const string invalidSearchMessage = "Keyword given is invalid.";
        private const string imageInvalidMessage = "Image fields are invalid.";
        private const string imageUpdateMessage = "Image with id '{0}' is updated.";
        private const string imageDeletedMessage = "Image with id '{0}' is deleted.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageServiceShould"/> class.
        /// </summary>
        public ImageServiceShould()
        {
            mockRadElementContext = new Mock<RadElementDbContext>();
            mockLogger = new Mock<ILogger>();

            service = new ImageService(mockRadElementContext.Object, mockLogger.Object);
        }

        #region GetImages

        [Fact]
        public async void GetImagesShouldThrowInternalServerErrorForExceptions()
        {
            IntializeMockData(false);
            var result = await service.GetImages();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Fact]
        public async void GetImagesShouldReturnAllImages()
        {
            IntializeMockData(true);
            var result = await service.GetImages();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<Image>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetImage

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetImageShouldThrowInternalServerErrorForExceptions(int imageId)
        {
            IntializeMockData(false);
            var result = await service.GetImage(imageId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void GetImageShouldReturnNotFoundIfDoesnotExists(int imageId)
        {
            IntializeMockData(true);
            var result = await service.GetImage(imageId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(imageNotFoundMessage, imageId), result.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetImageShouldReturnImageBasedOnImageId(int imageId)
        {
            IntializeMockData(true);
            var result = await service.GetImage(imageId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<Image>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region SearchImages

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async void SearchImagesShouldReturnBadRequestIfSearchKeywordIsInvalid(string searchKeyword)
        {
            IntializeMockData(true);
            var result = await service.SearchImages(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(string.Format(invalidSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("Image_1")]
        [InlineData("Image_2")]
        public async void SearchImagesShouldReturnNotFoundIfSearchKeywordDoesnotExists(string searchKeyword)
        {
            IntializeMockData(true);
            var result = await service.SearchImages(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(imageNotFoundMessageWithSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("Image_1")]
        [InlineData("Image_2")]
        public async void SearchImagesShouldReturnThrowInternalServerErrorForExceptions(string searchKeyword)
        {
            IntializeMockData(false);
            var result = await service.SearchImages(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("image 1")]
        [InlineData("image 2")]
        public async void SearchImagesShouldReturnImagesIfSearcheImageExists(string searchKeyword)
        {
            IntializeMockData(true);
            var result = await service.SearchImages(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<Image>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region CreateImage

        [Theory]
        [InlineData(null)]
        public async void CreateImageShouldReturnBadRequestIfImageDetailsAreInvalid(CreateUpdateImage image)
        {
            IntializeMockData(true);
            var result = await service.CreateImage(image);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(imageInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("https://assist.acr.org", "https://assist.acr.org", "Image 5")]
        [InlineData("https://assist.acr.org", "https://assist.acr.org", "Image 56")]
        public async void CreateImageShouldReturnThrowInternalServerErrorForExceptions(string localurl, string caption, string sourceUrl)
        {
            var image = new CreateUpdateImage();
            image.LocalUrl = localurl;
            image.Caption = caption;
            image.SourceUrl = sourceUrl;
            image.Width = 0;
            image.Height = 0;
            image.Rights = "none";

            IntializeMockData(false);
            var result = await service.CreateImage(image);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }


        [Theory]
        [InlineData("https://assist.acr.org", "https://assist.acr.org", "Image 5")]
        [InlineData("https://assist.acr.org", "https://assist.acr.org", "Image 56")]
        public async void CreateImageShouldReturnImageIdIfCreated(string localurl, string caption, string sourceUrl)
        {
            var image = new CreateUpdateImage();
            image.LocalUrl = localurl;
            image.Caption = caption;
            image.SourceUrl = sourceUrl;
            image.Width = 0;
            image.Height = 0;
            image.Rights = "none";

            IntializeMockData(true);
            var result = await service.CreateImage(image);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ImageIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        #endregion

        #region UpdateImage

        [Theory]
        [InlineData(1, null)]
        [InlineData(2, null)]
        public async void UpdateImageShouldReturnBadRequestIfImageDetailsAreInvalid(int imageId, CreateUpdateImage image)
        {
            IntializeMockData(true);
            var result = await service.UpdateImage(imageId, image);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(imageInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData(60)]
        [InlineData(70)]
        public async void UpdateImageShouldReturnENotFoundIfImageDoesNotExists(int imageId)
        {
            var image = new CreateUpdateImage();
            image.LocalUrl = "https://uat-assist.acr.org";
            image.Caption = "https://uat-assist.acr.org";
            image.SourceUrl = "image test 5";
            image.Width = 0;
            image.Height = 0;
            image.Rights = "none";

            IntializeMockData(true);
            var result = await service.UpdateImage(imageId, image);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(imageNotFoundMessage, imageId), result.Value);
        }

        [Theory]
        [InlineData(2)]
        public async void UpdateImageShouldReturnThrowInternalServerErrorForExceptions(int imageId)
        {
            var image = new CreateUpdateImage();
            image.LocalUrl = "https://uat-assist.acr.org";
            image.Caption = "https://uat-assist.acr.org";
            image.SourceUrl = "image test 5";
            image.Width = 0;
            image.Height = 0;
            image.Rights = "none";

            IntializeMockData(false);
            var result = await service.UpdateImage(imageId, image);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData(2)]
        public async void UpdateImageShouldReturnImageIdIfImageDetailsAreValid(int imageId)
        {
            var image = new CreateUpdateImage();
            image.LocalUrl = "https://uat-assist.acr.org";
            image.Caption = "https://uat-assist.acr.org";
            image.SourceUrl = "image test 5";
            image.Width = 0;
            image.Height = 0;
            image.Rights = "none";

            IntializeMockData(true);
            var result = await service.UpdateImage(imageId, image);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(imageUpdateMessage, imageId), result.Value);
        }

        #endregion

        #region DeleteImage

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        [InlineData(102)]
        [InlineData(103)]
        [InlineData(104)]
        public async void DeleteImageShouldReturnNotFoundIfImageIdIsInvalid(int imageId)
        {
            IntializeMockData(true);
            var result = await service.DeleteImage(imageId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(imageNotFoundMessage, imageId), result.Value);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        public async void DeleteImageShouldThrowInternalServerErrorForExceptions(int imageId)
        {
            IntializeMockData(false);
            var result = await service.DeleteImage(imageId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async void DeleteImageShouldDeleteImageIfImageIdIsValid(int imageId)
        {
            IntializeMockData(true);
            var result = await service.DeleteImage(imageId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(imageDeletedMessage, imageId), result.Value);
        }

        #endregion

        #region Private Methods

        private void IntializeMockData(bool mockDatabaseData)
        {
            if (mockDatabaseData)
            {
                var mockElement = new Mock<DbSet<Element>>();
                mockElement.As<IQueryable<Element>>().Setup(m => m.Provider).Returns(MockDataContext.elementsDB.Provider);
                mockElement.As<IQueryable<Element>>().Setup(m => m.Expression).Returns(MockDataContext.elementsDB.Expression);
                mockElement.As<IQueryable<Element>>().Setup(m => m.ElementType).Returns(MockDataContext.elementsDB.ElementType);
                mockElement.As<IQueryable<Element>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.elementsDB.GetEnumerator());

                var mockSet = new Mock<DbSet<ElementSet>>();
                mockSet.As<IQueryable<ElementSet>>().Setup(m => m.Provider).Returns(MockDataContext.elementSetDb.Provider);
                mockSet.As<IQueryable<ElementSet>>().Setup(m => m.Expression).Returns(MockDataContext.elementSetDb.Expression);
                mockSet.As<IQueryable<ElementSet>>().Setup(m => m.ElementType).Returns(MockDataContext.elementSetDb.ElementType);
                mockSet.As<IQueryable<ElementSet>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.elementSetDb.GetEnumerator());

                var mockElementSetRef = new Mock<DbSet<ElementSetRef>>();
                mockElementSetRef.As<IQueryable<ElementSetRef>>().Setup(m => m.Provider).Returns(MockDataContext.elementSetRefDb.Provider);
                mockElementSetRef.As<IQueryable<ElementSetRef>>().Setup(m => m.Expression).Returns(MockDataContext.elementSetRefDb.Expression);
                mockElementSetRef.As<IQueryable<ElementSetRef>>().Setup(m => m.ElementType).Returns(MockDataContext.elementSetRefDb.ElementType);
                mockElementSetRef.As<IQueryable<ElementSetRef>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.elementSetRefDb.GetEnumerator());

                var mockElementValue = new Mock<DbSet<ElementValue>>();
                mockElementValue.As<IQueryable<ElementValue>>().Setup(m => m.Provider).Returns(MockDataContext.elementValueDb.Provider);
                mockElementValue.As<IQueryable<ElementValue>>().Setup(m => m.Expression).Returns(MockDataContext.elementValueDb.Expression);
                mockElementValue.As<IQueryable<ElementValue>>().Setup(m => m.ElementType).Returns(MockDataContext.elementValueDb.ElementType);
                mockElementValue.As<IQueryable<ElementValue>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.elementValueDb.GetEnumerator());

                var mockIndexCodeSystem = new Mock<DbSet<IndexCodeSystem>>();
                mockIndexCodeSystem.As<IQueryable<IndexCodeSystem>>().Setup(m => m.Provider).Returns(MockDataContext.indexCodeSystemDb.Provider);
                mockIndexCodeSystem.As<IQueryable<IndexCodeSystem>>().Setup(m => m.Expression).Returns(MockDataContext.indexCodeSystemDb.Expression);
                mockIndexCodeSystem.As<IQueryable<IndexCodeSystem>>().Setup(m => m.ElementType).Returns(MockDataContext.indexCodeSystemDb.ElementType);
                mockIndexCodeSystem.As<IQueryable<IndexCodeSystem>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.indexCodeSystemDb.GetEnumerator());

                var mockIndexCode = new Mock<DbSet<IndexCode>>();
                mockIndexCode.As<IQueryable<IndexCode>>().Setup(m => m.Provider).Returns(MockDataContext.indexCodeDb.Provider);
                mockIndexCode.As<IQueryable<IndexCode>>().Setup(m => m.Expression).Returns(MockDataContext.indexCodeDb.Expression);
                mockIndexCode.As<IQueryable<IndexCode>>().Setup(m => m.ElementType).Returns(MockDataContext.indexCodeDb.ElementType);
                mockIndexCode.As<IQueryable<IndexCode>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.indexCodeDb.GetEnumerator());

                var mockIndexCodeElementRef = new Mock<DbSet<IndexCodeElementRef>>();
                mockIndexCodeElementRef.As<IQueryable<IndexCodeElementRef>>().Setup(m => m.Provider).Returns(MockDataContext.indexCodeElementDb.Provider);
                mockIndexCodeElementRef.As<IQueryable<IndexCodeElementRef>>().Setup(m => m.Expression).Returns(MockDataContext.indexCodeElementDb.Expression);
                mockIndexCodeElementRef.As<IQueryable<IndexCodeElementRef>>().Setup(m => m.ElementType).Returns(MockDataContext.indexCodeElementDb.ElementType);
                mockIndexCodeElementRef.As<IQueryable<IndexCodeElementRef>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.indexCodeElementDb.GetEnumerator());

                var mockIndexCodeElementSetRef = new Mock<DbSet<IndexCodeElementSetRef>>();
                mockIndexCodeElementSetRef.As<IQueryable<IndexCodeElementSetRef>>().Setup(m => m.Provider).Returns(MockDataContext.indexCodeElementSetDb.Provider);
                mockIndexCodeElementSetRef.As<IQueryable<IndexCodeElementSetRef>>().Setup(m => m.Expression).Returns(MockDataContext.indexCodeElementSetDb.Expression);
                mockIndexCodeElementSetRef.As<IQueryable<IndexCodeElementSetRef>>().Setup(m => m.ElementType).Returns(MockDataContext.indexCodeElementSetDb.ElementType);
                mockIndexCodeElementSetRef.As<IQueryable<IndexCodeElementSetRef>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.indexCodeElementSetDb.GetEnumerator());

                var mockIndexCodeElementValueRef = new Mock<DbSet<IndexCodeElementValueRef>>();
                mockIndexCodeElementValueRef.As<IQueryable<IndexCodeElementValueRef>>().Setup(m => m.Provider).Returns(MockDataContext.indexCodeElementValueDb.Provider);
                mockIndexCodeElementValueRef.As<IQueryable<IndexCodeElementValueRef>>().Setup(m => m.Expression).Returns(MockDataContext.indexCodeElementValueDb.Expression);
                mockIndexCodeElementValueRef.As<IQueryable<IndexCodeElementValueRef>>().Setup(m => m.ElementType).Returns(MockDataContext.indexCodeElementValueDb.ElementType);
                mockIndexCodeElementValueRef.As<IQueryable<IndexCodeElementValueRef>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.indexCodeElementValueDb.GetEnumerator());

                var mockPerson = new Mock<DbSet<Person>>();
                mockPerson.As<IQueryable<Person>>().Setup(m => m.Provider).Returns(MockDataContext.personDb.Provider);
                mockPerson.As<IQueryable<Person>>().Setup(m => m.Expression).Returns(MockDataContext.personDb.Expression);
                mockPerson.As<IQueryable<Person>>().Setup(m => m.ElementType).Returns(MockDataContext.personDb.ElementType);
                mockPerson.As<IQueryable<Person>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.personDb.GetEnumerator());

                var mockPersonElementRef = new Mock<DbSet<PersonRoleElementRef>>();
                mockPersonElementRef.As<IQueryable<PersonRoleElementRef>>().Setup(m => m.Provider).Returns(MockDataContext.personElementRefDb.Provider);
                mockPersonElementRef.As<IQueryable<PersonRoleElementRef>>().Setup(m => m.Expression).Returns(MockDataContext.personElementRefDb.Expression);
                mockPersonElementRef.As<IQueryable<PersonRoleElementRef>>().Setup(m => m.ElementType).Returns(MockDataContext.personElementRefDb.ElementType);
                mockPersonElementRef.As<IQueryable<PersonRoleElementRef>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.personElementRefDb.GetEnumerator());

                var mockPersonElementSetRef = new Mock<DbSet<PersonRoleElementSetRef>>();
                mockPersonElementSetRef.As<IQueryable<PersonRoleElementSetRef>>().Setup(m => m.Provider).Returns(MockDataContext.personElementSetRefDb.Provider);
                mockPersonElementSetRef.As<IQueryable<PersonRoleElementSetRef>>().Setup(m => m.Expression).Returns(MockDataContext.personElementSetRefDb.Expression);
                mockPersonElementSetRef.As<IQueryable<PersonRoleElementSetRef>>().Setup(m => m.ElementType).Returns(MockDataContext.personElementSetRefDb.ElementType);
                mockPersonElementSetRef.As<IQueryable<PersonRoleElementSetRef>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.personElementSetRefDb.GetEnumerator());

                var mockRerence = new Mock<DbSet<Reference>>();
                mockRerence.As<IQueryable<Reference>>().Setup(m => m.Provider).Returns(MockDataContext.referenceDb.Provider);
                mockRerence.As<IQueryable<Reference>>().Setup(m => m.Expression).Returns(MockDataContext.referenceDb.Expression);
                mockRerence.As<IQueryable<Reference>>().Setup(m => m.ElementType).Returns(MockDataContext.referenceDb.ElementType);
                mockRerence.As<IQueryable<Reference>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.referenceDb.GetEnumerator());

                var mockRerenceRef = new Mock<DbSet<ReferenceRef>>();
                mockRerenceRef.As<IQueryable<ReferenceRef>>().Setup(m => m.Provider).Returns(MockDataContext.referenceRefDb.Provider);
                mockRerenceRef.As<IQueryable<ReferenceRef>>().Setup(m => m.Expression).Returns(MockDataContext.referenceRefDb.Expression);
                mockRerenceRef.As<IQueryable<ReferenceRef>>().Setup(m => m.ElementType).Returns(MockDataContext.referenceRefDb.ElementType);
                mockRerenceRef.As<IQueryable<ReferenceRef>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.referenceRefDb.GetEnumerator());

                var mockImage= new Mock<DbSet<Image>>();
                mockImage.As<IQueryable<Image>>().Setup(m => m.Provider).Returns(MockDataContext.imagesDb.Provider);
                mockImage.As<IQueryable<Image>>().Setup(m => m.Expression).Returns(MockDataContext.imagesDb.Expression);
                mockImage.As<IQueryable<Image>>().Setup(m => m.ElementType).Returns(MockDataContext.imagesDb.ElementType);
                mockImage.As<IQueryable<Image>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.imagesDb.GetEnumerator());

                var mockImageRef = new Mock<DbSet<ImageRef>>();
                mockImageRef.As<IQueryable<ImageRef>>().Setup(m => m.Provider).Returns(MockDataContext.imageRefDb.Provider);
                mockImageRef.As<IQueryable<ImageRef>>().Setup(m => m.Expression).Returns(MockDataContext.imageRefDb.Expression);
                mockImageRef.As<IQueryable<ImageRef>>().Setup(m => m.ElementType).Returns(MockDataContext.imageRefDb.ElementType);
                mockImageRef.As<IQueryable<ImageRef>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.imageRefDb.GetEnumerator());

                mockRadElementContext.Setup(c => c.Element).Returns(mockElement.Object);
                mockRadElementContext.Setup(c => c.ElementSet).Returns(mockSet.Object);
                mockRadElementContext.Setup(c => c.ElementSetRef).Returns(mockElementSetRef.Object);
                mockRadElementContext.Setup(c => c.ElementValue).Returns(mockElementValue.Object);
                mockRadElementContext.Setup(c => c.IndexCodeSystem).Returns(mockIndexCodeSystem.Object);
                mockRadElementContext.Setup(c => c.IndexCode).Returns(mockIndexCode.Object);
                mockRadElementContext.Setup(c => c.IndexCodeElementRef).Returns(mockIndexCodeElementRef.Object);
                mockRadElementContext.Setup(c => c.IndexCodeElementSetRef).Returns(mockIndexCodeElementSetRef.Object);
                mockRadElementContext.Setup(c => c.IndexCodeElementValueRef).Returns(mockIndexCodeElementValueRef.Object);
                mockRadElementContext.Setup(c => c.Person).Returns(mockPerson.Object);
                mockRadElementContext.Setup(c => c.PersonRoleElementRef).Returns(mockPersonElementRef.Object);
                mockRadElementContext.Setup(c => c.PersonRoleElementSetRef).Returns(mockPersonElementSetRef.Object);
                mockRadElementContext.Setup(c => c.Reference).Returns(mockRerence.Object);
                mockRadElementContext.Setup(c => c.ReferenceRef).Returns(mockRerenceRef.Object);
                mockRadElementContext.Setup(c => c.Image).Returns(mockImage.Object);
                mockRadElementContext.Setup(c => c.ImageRef).Returns(mockImageRef.Object);
            }

            var mockConfigurationManager = new Mock<IConfigurationManager>();
            var options = new DbContextOptionsBuilder<RadElementDbContext>().UseMySql(connectionString).Options;

            mockRadElementContext.Setup(c => c.Database).Returns(new DatabaseFacade(new RadElementDbContext(options, mockConfigurationManager.Object)));
        }

        #endregion
    }
}
