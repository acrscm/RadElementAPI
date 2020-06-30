
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
    public class ReferenceServiceShould
    {
        /// <summary>
        /// The person service
        /// </summary>
        private readonly ReferenceService service;

        /// <summary>
        /// The mock RAD element context
        /// </summary>
        private readonly Mock<RadElementDbContext> mockRadElementContext;

        /// <summary>
        /// The mock logger
        /// </summary>
        private readonly Mock<ILogger> mockLogger;

        private const string connectionString = "server=localhost;user id=root;password=root;persistsecurityinfo=True;database=radelement;Convert Zero Datetime=True";
        private const string referenceNotFoundMessage = "No such reference with id '{0}'.";
        private const string referenceNotFoundMessageWithSearchMessage = "No such reference with keyword '{0}'.";
        private const string invalidSearchMessage = "Keyword given is invalid.";
        private const string referenceInvalidMessage = "Reference fields are invalid.";
        private const string referenceUpdateMessage = "Reference with id '{0}' is updated.";
        private const string referenceDeletedMessage = "Reference with id '{0}' is deleted.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceServiceShould"/> class.
        /// </summary>
        public ReferenceServiceShould()
        {
            mockRadElementContext = new Mock<RadElementDbContext>();
            mockLogger = new Mock<ILogger>();

            service = new ReferenceService(mockRadElementContext.Object, mockLogger.Object);
        }

        #region GetReferences

        [Fact]
        public async void GetReferencesShouldThrowInternalServerErrorForExceptions()
        {
            IntializeMockData(false);
            var result = await service.GetReferences();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Fact]
        public async void GetReferencesShouldReturnAllReferences()
        {
            IntializeMockData(true);
            var result = await service.GetReferences();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<Reference>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetReference

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetReferenceShouldThrowInternalServerErrorForExceptions(int referenceId)
        {
            IntializeMockData(false);
            var result = await service.GetReference(referenceId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void GetReferenceShouldReturnNotFoundIfDoesnotExists(int referenceId)
        {
            IntializeMockData(true);
            var result = await service.GetReference(referenceId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(referenceNotFoundMessage, referenceId), result.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetReferenceShouldReturnReferenceBasedOnReferenceId(int referenceId)
        {
            IntializeMockData(true);
            var result = await service.GetReference(referenceId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<Reference>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region SearchReferences

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async void SearchReferencesShouldReturnBadRequestIfSearchKeywordIsInvalid(string searchKeyword)
        {
            IntializeMockData(true);
            var result = await service.SearchReferences(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(string.Format(invalidSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("citation_test1")]
        [InlineData("citation_test2")]
        public async void SearchReferencesShouldReturnNotFoundIfSearchKeywordDoesnotExists(string searchKeyword)
        {
            IntializeMockData(true);
            var result = await service.SearchReferences(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(referenceNotFoundMessageWithSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("citation")]
        public async void SearchReferencesShouldReturnThrowInternalServerErrorForExceptions(string searchKeyword)
        {
            IntializeMockData(false);
            var result = await service.SearchReferences(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("citation")]
        public async void SearchReferencesShouldReturnReferencesIfSearchedReferenceExists(string searchKeyword)
        {
            IntializeMockData(true);
            var result = await service.SearchReferences(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<Reference>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region CreateReference

        [Theory]
        [InlineData(null)]
        public async void CreateReferenceShouldReturnBadRequestIfReferenceDetailsAreInvalid(CreateUpdateReference reference)
        {
            IntializeMockData(true);
            var result = await service.CreateReference(reference);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(referenceInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("Citation_new_1", "https://api.radelement.org", "https://api.radelement.org ")]
        [InlineData("Citation_new_2", "https://api.radelement.org", "https://api.radelement.org")]
        public async void CreateReferenceShouldReturnThrowInternalServerErrorForExceptions(string citation, string doi_uri, string url)
        {
            var reference = new CreateUpdateReference();
            reference.Citation = citation;
            reference.DoiUri = doi_uri;
            reference.Url = url;

            IntializeMockData(false);
            var result = await service.CreateReference(reference);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }


        [Theory]
        [InlineData("Citation_new_1", "https://api.radelement.org", "https://api.radelement.org ")]
        [InlineData("Citation_new_2", "https://api.radelement.org", "https://api.radelement.org")]
        public async void CreateReferenceShouldReturnReferenceIdIfCreated(string citation, string doi_uri, string url)
        {
            var reference = new CreateUpdateReference();
            reference.Citation = citation;
            reference.DoiUri = doi_uri;
            reference.Url = url;

            IntializeMockData(true);
            var result = await service.CreateReference(reference);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ReferenceIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        #endregion

        #region UpdateReference

        [Theory]
        [InlineData(1, null)]
        [InlineData(2, null)]
        public async void UpdateReferenceShouldReturnBadRequestIfReferenceDetailsAreInvalid(int referenceId, CreateUpdateReference reference)
        {
            IntializeMockData(true);
            var result = await service.UpdateReference(referenceId, reference);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(referenceInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData(60)]
        [InlineData(70)]
        public async void UpdateReferenceShouldReturnENotFoundIfReferenceDoesNotExists(int referenceId)
        {
            var reference = new CreateUpdateReference();
            reference.Citation = "Citation_New";
            reference.DoiUri = "https://api.radelement.org";
            reference.Url = "https://api.radelement.org";

            IntializeMockData(true);
            var result = await service.UpdateReference(referenceId, reference);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(referenceNotFoundMessage, referenceId), result.Value);
        }

        [Theory]
        [InlineData(2)]
        public async void UpdateReferenceShouldReturnThrowInternalServerErrorForExceptions(int referenceId)
        {
            var reference = new CreateUpdateReference();
            reference.Citation = "Citation_New";
            reference.DoiUri = "https://api.radelement.org";
            reference.Url = "https://api.radelement.org";

            IntializeMockData(false);
            var result = await service.UpdateReference(referenceId, reference);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData(2)]
        public async void UpdateReferenceShouldReturnReferenceIdIfReferenceDetailsAreValid(int referenceId)
        {
            var reference = new CreateUpdateReference();
            reference.Citation = "Citation_New";
            reference.DoiUri = "https://api.radeleemnt.org";
            reference.Url = "https://api.radeleemnt.org";

            IntializeMockData(true);
            var result = await service.UpdateReference(referenceId, reference);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(referenceUpdateMessage, referenceId), result.Value);
        }

        #endregion

        #region DeleteReference

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        [InlineData(102)]
        [InlineData(103)]
        [InlineData(104)]
        public async void DeleteReferenceShouldReturnNotFoundIfReferenceIdIdIsInvalid(int referenceId)
        {
            IntializeMockData(true);
            var result = await service.DeleteReference(referenceId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(referenceNotFoundMessage, referenceId), result.Value);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        public async void DeleteReferenceShouldThrowInternalServerErrorForExceptions(int referenceId)
        {
            IntializeMockData(false);
            var result = await service.DeleteReference(referenceId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async void DeleteReferenceShouldDeleteReferenceIfReferenceIdIsValid(int referenceId)
        {
            IntializeMockData(true);
            var result = await service.DeleteReference(referenceId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(referenceDeletedMessage, referenceId), result.Value);
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
            }

            var mockConfigurationManager = new Mock<IConfigurationManager>();
            var options = new DbContextOptionsBuilder<RadElementDbContext>().UseMySql(connectionString).Options;

            mockRadElementContext.Setup(c => c.Database).Returns(new DatabaseFacade(new RadElementDbContext(options, mockConfigurationManager.Object)));
        }

        #endregion
    }
}
