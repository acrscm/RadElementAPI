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
    public class IndexCodeServiceShould
    {
        /// <summary>
        /// The person service
        /// </summary>
        private readonly IndexCodeService service;

        /// <summary>
        /// The mock RAD element context
        /// </summary>
        private readonly Mock<RadElementDbContext> mockRadElementContext;

        /// <summary>
        /// The mock logger
        /// </summary>
        private readonly Mock<ILogger> mockLogger;

        private const string connectionString = "server=localhost;user id=root;password=root;persistsecurityinfo=True;database=radelement;Convert Zero Datetime=True";
        private const string indexCodeNotFoundMessage = "No such index code with id '{0}'.";
        private const string indexCodeSystemNotFoundMessage = "No such index code system with code '{0}'.";
        private const string indexCodeNotFoundMessageWithSearchMessage = "No such index code with keyword '{0}'.";
        private const string invalidSearchMessage = "Keyword given is invalid.";
        private const string indexCodeInvalidMessage = "Index code fields are invalid.";
        private const string indexCodeUpdateMessage = "Index code with id '{0}' is updated.";
        private const string indexCodeDeletedMessage = "Index code with id '{0}' is deleted.";

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexCodeServiceShould"/> class.
        /// </summary>
        public IndexCodeServiceShould()
        {
            mockRadElementContext = new Mock<RadElementDbContext>();
            mockLogger = new Mock<ILogger>();

            service = new IndexCodeService(mockRadElementContext.Object, mockLogger.Object);
        }

        #region GetIndexCodes

        [Fact]
        public async void GetIndexCodesShouldThrowInternalServerErrorForExceptions()
        {
            IntializeMockData(false);
            var result = await service.GetIndexCodes();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Fact]
        public async void GetIndexCodesShouldReturnAllIndexCodes()
        {
            IntializeMockData(true);
            var result = await service.GetIndexCodes();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<IndexCode>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetIndexCode

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetIndexCodeShouldThrowInternalServerErrorForExceptions(int indexCodeId)
        {
            IntializeMockData(false);
            var result = await service.GetIndexCode(indexCodeId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void GetIndexCodeShouldReturnNotFoundIfDoesnotExists(int indexCodeId)
        {
            IntializeMockData(true);
            var result = await service.GetIndexCode(indexCodeId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(indexCodeNotFoundMessage, indexCodeId), result.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetIndexCodeShouldReturnIndexCodeBasedOnIndexCodeId(int indexCodeId)
        {
            IntializeMockData(true);
            var result = await service.GetIndexCode(indexCodeId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<IndexCode>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region SearchIndexCodes

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async void SearchIndexCodesShouldReturnBadRequestIfSearchKeywordIsInvalid(string searchKeyword)
        {
            IntializeMockData(true);
            var result = await service.SearchIndexCodes(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(string.Format(invalidSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("RADLEX1")]
        [InlineData("RADLEX2")]
        public async void SearchIndexCodesShouldReturnNotFoundIfSearchKeywordDoesnotExists(string searchKeyword)
        {
            IntializeMockData(true);
            var result = await service.SearchIndexCodes(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(indexCodeNotFoundMessageWithSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("RADLEX")]
        public async void SearchIndexCodesShouldReturnThrowInternalServerErrorForExceptions(string searchKeyword)
        {
            IntializeMockData(false);
            var result = await service.SearchIndexCodes(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("RADLEX")]
        public async void SearchIndexCodesShouldReturnIndexCodesIfSearchedIndexCodeExists(string searchKeyword)
        {
            IntializeMockData(true);
            var result = await service.SearchIndexCodes(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<IndexCode>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region CreateIndexCode

        [Theory]
        [InlineData(null)]
        public async void CreateIndexCodeShouldReturnBadRequestIfIndexCodeDetailsAreInvalid(CreateUpdateIndexCode indexCode)
        {
            IntializeMockData(true);
            var result = await service.CreateIndexCode(indexCode);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(indexCodeInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("RID28662", "RADLEX", "Attenuation 1")]
        [InlineData("RID11086", "RADLEX", "Attenuation 2")]
        public async void CreateIndexCodeShouldReturnThrowInternalServerErrorForExceptions(string code, string system, string display)
        {
            var indexCode = new CreateUpdateIndexCode();
            indexCode.Code = code;
            indexCode.System = system;
            indexCode.Display = display;

            IntializeMockData(false);
            var result = await service.CreateIndexCode(indexCode);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("RID28662", "RADCT", "Attenuation 1")]
        [InlineData("RID11086", "RADCT", "Attenuation 2")]
        public async void CreateIndexCodeShouldReturnNotFoundIfIndexCodeSystemDoesnotExists(string code, string system, string display)
        {
            var indexCode = new CreateUpdateIndexCode();
            indexCode.Code = code;
            indexCode.System = system;
            indexCode.Display = display;

            IntializeMockData(true);
            var result = await service.CreateIndexCode(indexCode);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(indexCodeSystemNotFoundMessage, system), result.Value);
        }

        [Theory]
        [InlineData("RID28662", "RADLEX", "Attenuation 1")]
        [InlineData("RID11086", "RADLEX", "Attenuation 2")]
        public async void CreateIndexCodeShouldReturnIndexCodeIdIfAlreadyExists(string code, string system, string display)
        {
            var indexCode = new CreateUpdateIndexCode();
            indexCode.Code = code;
            indexCode.System = system;
            indexCode.Display = display;

            IntializeMockData(true);
            var result = await service.CreateIndexCode(indexCode);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<IndexCodeIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        [Theory]
        [InlineData("RID2866256", "RADLEX", "Attenuation 1")]
        [InlineData("RID1108657", "RADLEX", "Attenuation 2")]
        public async void CreateIndexCodeShouldReturnIndexCodeIdIfCreated(string code, string system, string display)
        {
            var indexCode = new CreateUpdateIndexCode();
            indexCode.Code = code;
            indexCode.System = system;
            indexCode.Display = display;

            IntializeMockData(true);
            var result = await service.CreateIndexCode(indexCode);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<IndexCodeIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        #endregion

        #region UpdateIndexCode

        [Theory]
        [InlineData(1, null)]
        [InlineData(2, null)]
        public async void UpdateIndexCodeShouldReturnBadRequestIfIndexCodeDetailsAreInvalid(int indexCodeId, CreateUpdateIndexCode indexCode)
        {
            IntializeMockData(true);
            var result = await service.UpdateIndexCode(indexCodeId, indexCode);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(indexCodeInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData(60)]
        [InlineData(70)]
        public async void UpdateIndexCodeShouldReturnENotFoundIfIndexCodeDoesNotExists(int indexCodeId)
        {
            var indexCode = new CreateUpdateIndexCode();
            indexCode.Code = "RID2866256";
            indexCode.System = "RADLEX";
            indexCode.Display = "Attenuation";

            IntializeMockData(true);
            var result = await service.UpdateIndexCode(indexCodeId, indexCode);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(indexCodeNotFoundMessage, indexCodeId), result.Value);
        }

        [Theory]
        [InlineData(2)]
        public async void UpdateIndexCodeShouldReturnThrowInternalServerErrorForExceptions(int indexCodeId)
        {
            var indexCode = new CreateUpdateIndexCode();
            indexCode.Code = "RID2866256";
            indexCode.System = "RADLEX";
            indexCode.Display = "Attenuation";

            IntializeMockData(false);
            var result = await service.UpdateIndexCode(indexCodeId, indexCode);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData(2)]
        public async void UpdateIndexCodeShouldReturnIndexCodeIdIfIndexCodeDetailsAreValid(int indexCodeId)
        {
            var indexCode = new CreateUpdateIndexCode();
            indexCode.Code = "RID2866256";
            indexCode.System = "RADLEX";
            indexCode.Display = "Attenuation";

            IntializeMockData(true);
            var result = await service.UpdateIndexCode(indexCodeId, indexCode);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(indexCodeUpdateMessage, indexCodeId), result.Value);
        }

        #endregion

        #region DeleteIndexCode

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        [InlineData(102)]
        [InlineData(103)]
        [InlineData(104)]
        public async void DeleteIndexCodeShouldReturnNotFoundIfIndexCodeIdIsInvalid(int indexCodeId)
        {
            IntializeMockData(true);
            var result = await service.DeleteIndexCode(indexCodeId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(indexCodeNotFoundMessage, indexCodeId), result.Value);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        public async void DeleteIndexCodeShouldThrowInternalServerErrorForExceptions(int indexCodeId)
        {
            IntializeMockData(false);
            var result = await service.DeleteIndexCode(indexCodeId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async void DeleteIndexCodeShouldDeleteIndexCodeIfIndexCodeIdIsValid(int indexCodeId)
        {
            IntializeMockData(true);
            var result = await service.DeleteIndexCode(indexCodeId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(indexCodeDeletedMessage, indexCodeId), result.Value);
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
            }

            var mockConfigurationManager = new Mock<IConfigurationManager>();
            var options = new DbContextOptionsBuilder<RadElementDbContext>().UseMySql(connectionString).Options;

            mockRadElementContext.Setup(c => c.Database).Returns(new DatabaseFacade(new RadElementDbContext(options, mockConfigurationManager.Object)));
        }

        #endregion
    }
}
