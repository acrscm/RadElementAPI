using Microsoft.EntityFrameworkCore;
using Moq;
using RadElement.Core.Domain;
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
    public class SpecialtyServiceShould
    {
        /// <summary>
        /// The person service
        /// </summary>
        private readonly SpecialtyService service;

        /// <summary>
        /// The mock RAD element context
        /// </summary>
        private readonly Mock<RadElementDbContext> mockRadElementContext;

        /// <summary>
        /// The mock logger
        /// </summary>
        private readonly Mock<ILogger> mockLogger;

        private const string connectionString = "server=localhost;user id=root;password=root;persistsecurityinfo=True;database=radelement;Convert Zero Datetime=True";
        private const string specialtyNotFoundMessage = "No such specialty with id '{0}'.";
        private const string specialtyNotFoundMessageWithSearchMessage = "No such specialty with keyword '{0}'.";
        private const string invalidSearchMessage = "Keyword given is invalid.";

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialtyServiceShould"/> class.
        /// </summary>
        public SpecialtyServiceShould()
        {
            mockRadElementContext = new Mock<RadElementDbContext>();
            mockLogger = new Mock<ILogger>();

            service = new SpecialtyService(mockRadElementContext.Object, mockLogger.Object);
        }

        #region GetSpecialties

        [Fact]
        public async void GetSpecialtiesShouldThrowInternalServerErrorForExceptions()
        {
            IntializeMockData(false);
            var result = await service.GetSpecialties();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Fact]
        public async void GetSpecialtiesShouldReturnAllSpecialties()
        {
            IntializeMockData(true);
            var result = await service.GetSpecialties();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<Specialty>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetSpecialty

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetSpecialtyShouldThrowInternalServerErrorForExceptions(int specialtyId)
        {
            IntializeMockData(false);
            var result = await service.GetSpecialty(specialtyId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void GetSpecialtyShouldReturnNotFoundIfDoesnotExists(int specialtyId)
        {
            IntializeMockData(true);
            var result = await service.GetSpecialty(specialtyId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(specialtyNotFoundMessage, specialtyId), result.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetSpecialtyShouldReturnSpecialtyBasedOnSpecialtyId(int specialtyId)
        {
            IntializeMockData(true);
            var result = await service.GetSpecialty(specialtyId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<Specialty>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region SearchSpecialties

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async void SearchSpecialtiesShouldReturnBadRequestIfSearchKeywordIsInvalid(string searchKeyword)
        {
            IntializeMockData(true);
            var result = await service.SearchSpecialties(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(string.Format(invalidSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("MT")]
        [InlineData("ZT")]
        public async void SearchSpecialtiesShouldReturnNotFoundIfSearchKeywordDoesnotExists(string searchKeyword)
        {
            IntializeMockData(true);
            var result = await service.SearchSpecialties(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(specialtyNotFoundMessageWithSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("MT")]
        public async void SearchSpecialtiesShouldReturnThrowInternalServerErrorForExceptions(string searchKeyword)
        {
            IntializeMockData(false);
            var result = await service.SearchSpecialties(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("Breast")]
        [InlineData("Chest")]
        public async void SearchSpecialtiesShouldReturnSpecialtiesIfSearchedSpecialtyExists(string searchKeyword)
        {
            IntializeMockData(true);
            var result = await service.SearchSpecialties(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<Specialty>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region Private Methods

        private void IntializeMockData(bool mockDatabaseData)
        {
            if (mockDatabaseData)
            {
                var mockSpecialty = new Mock<DbSet<Specialty>>();
                mockSpecialty.As<IQueryable<Specialty>>().Setup(m => m.Provider).Returns(MockDataContext.specialtyDb.Provider);
                mockSpecialty.As<IQueryable<Specialty>>().Setup(m => m.Expression).Returns(MockDataContext.specialtyDb.Expression);
                mockSpecialty.As<IQueryable<Specialty>>().Setup(m => m.ElementType).Returns(MockDataContext.specialtyDb.ElementType);
                mockSpecialty.As<IQueryable<Specialty>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.specialtyDb.GetEnumerator());

                mockRadElementContext.Setup(c => c.Specialty).Returns(mockSpecialty.Object);
            }

            var mockConfigurationManager = new Mock<IConfigurationManager>();
            var options = new DbContextOptionsBuilder<RadElementDbContext>().UseMySql(connectionString).Options;

            mockRadElementContext.Setup(c => c.Database).Returns(new DatabaseFacade(new RadElementDbContext(options, mockConfigurationManager.Object)));
        }

        #endregion
    }
}
