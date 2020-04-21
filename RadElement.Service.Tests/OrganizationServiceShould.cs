using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Core.Data;
using RadElement.Core.Profile;
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
    public class OrganizationServiceShould
    {
        /// <summary>
        /// The service
        /// </summary>
        private readonly OrganizationService service;

        /// <summary>
        /// The mock RAD element context
        /// </summary>
        private readonly Mock<RadElementDbContext> mockRadElementContext;

        /// <summary>
        /// The mock logger
        /// </summary>
        private readonly Mock<ILogger> mockLogger;
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper mapper;

        private const string connectionString = "server=localhost;user id=root;password=root;persistsecurityinfo=True;database=radelement;Convert Zero Datetime=True";
        private const string organizationNotFoundMessage = "No such organization with id '{0}'.";
        private const string elemenIdInvalidMessage = "No such element with element id '{0}'.";
        private const string setIdInvalidMessage = "No such set with set id '{0}'.";
        private const string organizationNotFoundMessageWithSearchMessage = "No such organization with keyword '{0}'.";
        private const string invalidSearchMessage = "Keyword '{0}' given is invalid.";
        private const string organizationInvalidMessage = "Organization fields are invalid.";
        private const string organizationExistsMessage = "Organization with same details already exists.";
        private const string organizationUpdateMessage = "Organization with id '{0}' is updated.";
        private const string organizationDeletedMessage = "Organization with id '{0}' is deleted.";

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationServiceShould"/> class.
        /// </summary>
        public OrganizationServiceShould()
        {
            mockRadElementContext = new Mock<RadElementDbContext>();
            mockLogger = new Mock<ILogger>();

            var elementProfile = new ElementProfile();
            var elementSetProfile = new ElementSetProfile();
            var organizationProfile = new OrganizationProfile();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(elementProfile);
                cfg.AddProfile(elementSetProfile);
                cfg.AddProfile(organizationProfile);
            });

            mapper = new Mapper(mapperConfig);
            service = new OrganizationService(mockRadElementContext.Object, mapper, mockLogger.Object);
        }

        #region GetOrganizations

        [Fact]
        public async void GetOrganizationsShouldThrowInternalServerErrorForExceptions()
        {
            IntializeMockData(false);
            var result = await service.GetOrganizations();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Fact]
        public async void GetOrganizationsShouldReturnAllOrganization()
        {
            IntializeMockData(true);
            var result = await service.GetOrganizations();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<OrganizationDetails>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetOrganization

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetOrganizationShouldThrowInternalServerErrorForExceptions(int organizationId)
        {
            IntializeMockData(false);
            var result = await service.GetOrganization(organizationId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData(7)]
        [InlineData(8)]
        public async void GetOrganizationShouldReturnNotFoundIfDoesnotExists(int organizationId)
        {
            IntializeMockData(true);
            var result = await service.GetOrganization(organizationId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(organizationNotFoundMessage, organizationId), result.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetOrganizationShouldReturnOrganizationBasedOnOrganizationId(int organizationId)
        {
            IntializeMockData(true);
            var result = await service.GetOrganization(organizationId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<OrganizationDetails>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region SearchOrganization

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async void SearchOrganizationShouldReturnBadRequestIfSearchKeywordIsInvalid(string searchKeyword)
        {
            IntializeMockData(true);
            var result = await service.SearchOrganization(new SearchKeyword { Keyword = searchKeyword });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(string.Format(invalidSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("test1")]
        public async void SearchOrganizationShouldReturnEmpyOrganizationIfSearchKeywordDoesnotExists(string searchKeyword)
        {
            IntializeMockData(true);
            var result = await service.SearchOrganization(new SearchKeyword { Keyword = searchKeyword });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(organizationNotFoundMessageWithSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("Tumuor")]
        public async void SearchOrganizationShouldReturnThrowInternalServerErrorForExceptions(string searchKeyword)
        {
            IntializeMockData(false);
            var result = await service.SearchOrganization(new SearchKeyword { Keyword = searchKeyword });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("American College of Radiology - Data Science Institute")]
        public async void SearchOrganizationShouldReturnOrganizationIfSearchedOrganizationExists(string searchKeyword)
        {
            IntializeMockData(true);
            var result = await service.SearchOrganization(new SearchKeyword { Keyword = searchKeyword });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<OrganizationDetails>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region CreateOrganization

        [Theory]
        [InlineData(null)]
        public async void CreateOrganizationShouldReturnBadRequestIfOrganizationIsInvalid(CreateUpdateOrganization organization)
        {
            IntializeMockData(true);
            var result = await service.CreateOrganization(organization);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(organizationInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("American College of Radiology - Data Science Institute", "ACR-DSI", "http://www.acrdsi.org")]
        [InlineData("American College of Radiology", "ACR", "http://www.acr.org")]
        public async void CreateOrganizationShouldReturnBadRequestIfOrganizationExists(string name, string abbreviation, string Url)
        {
            var organization = new CreateUpdateOrganization();
            organization.Name = name;
            organization.Abbreviation = abbreviation;
            organization.Url = Url;

            IntializeMockData(true);
            var result = await service.CreateOrganization(organization);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(organizationExistsMessage, result.Value);
        }

        [Theory]
        [InlineData("ACR", "ACR")]
        [InlineData("ASNR", "ASNR")]
        public async void CreateOrganizationShouldReturnElementIdIOrganizationIsValid(string name, string abbreviation)
        {
            var organization = new CreateUpdateOrganization();
            organization.Name = name;
            organization.Abbreviation = abbreviation;

            IntializeMockData(true);
            var result = await service.CreateOrganization(organization);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<OrganizationIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        #endregion

        #region UpdateOrganization

        [Theory]
        [InlineData(1, null)]
        [InlineData(2, null)]
        public async void UpdateOrganizationShouldReturnBadRequestIfOrganizationIsInvalid(int organizationId, CreateUpdateOrganization organization)
        {
            IntializeMockData(true);
            var result = await service.UpdateOrganization(organizationId, organization);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(organizationInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void UpdateOrganizationShouldReturnBadRequestIfOrganizationExists(int organizationId)
        {
            var organization = new CreateUpdateOrganization();
            organization.Name = "American College of Radiology - Data Science Institute";
            organization.Abbreviation = "ACR-DSI";
            organization.Url = "http://www.acrdsi.org";

            IntializeMockData(true);
            var result = await service.UpdateOrganization(organizationId, organization);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(organizationExistsMessage, result.Value);
        }

        [Theory]
        [InlineData(2)]
        public async void UpdateOrganizationShouldReturnOrganizationIdIfOrganizationIsValid(int organizationId)
        {
            var organization = new CreateUpdateOrganization();
            organization.Name = "ACR New";
            organization.Abbreviation = "ACR New";

            IntializeMockData(true);
            var result = await service.UpdateOrganization(organizationId, organization);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(organizationUpdateMessage, organizationId), result.Value);
        }

        #endregion

        #region DeleteOrganization

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        public async void DeleteOrganizationShouldReturnNotFoundIfSetIdAndElementIdIsInvalid(int organizationId)
        {
            IntializeMockData(true);
            var result = await service.DeleteOrganization(organizationId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(organizationNotFoundMessage, organizationId), result.Value);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        public async void DeleteOrganizationShouldThrowInternalServerErrorForExceptions(int organizationId)
        {
            IntializeMockData(false);
            var result = await service.DeleteOrganization(organizationId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async void DeleteOrganizationShouldDeleteOrganizationIfOrganizationIdIsValid(int organizationId)
        {
            IntializeMockData(true);
            var result = await service.DeleteOrganization(organizationId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(organizationDeletedMessage, organizationId), result.Value);
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
                mockElement.Setup(d => d.Add(It.IsAny<Element>())).Callback<Element>((s) => MockDataContext.elementsDB.ToList().Add(s));

                var mockSet = new Mock<DbSet<ElementSet>>();
                mockSet.As<IQueryable<ElementSet>>().Setup(m => m.Provider).Returns(MockDataContext.elementSetDb.Provider);
                mockSet.As<IQueryable<ElementSet>>().Setup(m => m.Expression).Returns(MockDataContext.elementSetDb.Expression);
                mockSet.As<IQueryable<ElementSet>>().Setup(m => m.ElementType).Returns(MockDataContext.elementSetDb.ElementType);
                mockSet.As<IQueryable<ElementSet>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.elementSetDb.GetEnumerator());
                mockSet.Setup(d => d.Add(It.IsAny<ElementSet>())).Callback<ElementSet>((s) => MockDataContext.elementSetDb.ToList().Add(s));

                var mockElementSetRef = new Mock<DbSet<ElementSetRef>>();
                mockElementSetRef.As<IQueryable<ElementSetRef>>().Setup(m => m.Provider).Returns(MockDataContext.elementSetRefDb.Provider);
                mockElementSetRef.As<IQueryable<ElementSetRef>>().Setup(m => m.Expression).Returns(MockDataContext.elementSetRefDb.Expression);
                mockElementSetRef.As<IQueryable<ElementSetRef>>().Setup(m => m.ElementType).Returns(MockDataContext.elementSetRefDb.ElementType);
                mockElementSetRef.As<IQueryable<ElementSetRef>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.elementSetRefDb.GetEnumerator());
                mockElementSetRef.Setup(d => d.Add(It.IsAny<ElementSetRef>())).Callback<ElementSetRef>((s) => MockDataContext.elementSetRefDb.ToList().Add(s));

                var mockElementValue = new Mock<DbSet<ElementValue>>();
                mockElementValue.As<IQueryable<ElementValue>>().Setup(m => m.Provider).Returns(MockDataContext.elementValueDb.Provider);
                mockElement.As<IQueryable<ElementValue>>().Setup(m => m.Expression).Returns(MockDataContext.elementValueDb.Expression);
                mockElementValue.As<IQueryable<ElementValue>>().Setup(m => m.ElementType).Returns(MockDataContext.elementValueDb.ElementType);
                mockElementValue.As<IQueryable<ElementValue>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.elementValueDb.GetEnumerator());
                mockElementValue.Setup(d => d.Add(It.IsAny<ElementValue>())).Callback<ElementValue>((s) => MockDataContext.elementValueDb.ToList().Add(s));

                var mockOrganization = new Mock<DbSet<Organization>>();
                mockOrganization.As<IQueryable<Organization>>().Setup(m => m.Provider).Returns(MockDataContext.organizationDb.Provider);
                mockOrganization.As<IQueryable<Organization>>().Setup(m => m.Expression).Returns(MockDataContext.organizationDb.Expression);
                mockOrganization.As<IQueryable<Organization>>().Setup(m => m.ElementType).Returns(MockDataContext.organizationDb.ElementType);
                mockOrganization.As<IQueryable<Organization>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.organizationDb.GetEnumerator());
                mockOrganization.Setup(d => d.Add(It.IsAny<Organization>())).Callback<Organization>((s) => MockDataContext.organizationDb.ToList().Add(s));

                var mockOrganizationElementRef = new Mock<DbSet<OrganizationRoleElementRef>>();
                mockOrganizationElementRef.As<IQueryable<OrganizationRoleElementRef>>().Setup(m => m.Provider).Returns(MockDataContext.organizationElementRefDb.Provider);
                mockOrganizationElementRef.As<IQueryable<OrganizationRoleElementRef>>().Setup(m => m.Expression).Returns(MockDataContext.organizationElementRefDb.Expression);
                mockOrganizationElementRef.As<IQueryable<OrganizationRoleElementRef>>().Setup(m => m.ElementType).Returns(MockDataContext.organizationElementRefDb.ElementType);
                mockOrganizationElementRef.As<IQueryable<OrganizationRoleElementRef>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.organizationElementRefDb.GetEnumerator());
                mockOrganizationElementRef.Setup(d => d.Add(It.IsAny<OrganizationRoleElementRef>())).Callback<OrganizationRoleElementRef>((s) => MockDataContext.organizationElementRefDb.ToList().Add(s));

                var mockOrganizationElementSetRef = new Mock<DbSet<OrganizationRoleElementSetRef>>();
                mockOrganizationElementSetRef.As<IQueryable<OrganizationRoleElementSetRef>>().Setup(m => m.Provider).Returns(MockDataContext.organizationElementSetRefDb.Provider);
                mockOrganizationElementSetRef.As<IQueryable<OrganizationRoleElementSetRef>>().Setup(m => m.Expression).Returns(MockDataContext.organizationElementSetRefDb.Expression);
                mockOrganizationElementSetRef.As<IQueryable<OrganizationRoleElementSetRef>>().Setup(m => m.ElementType).Returns(MockDataContext.organizationElementSetRefDb.ElementType);
                mockOrganizationElementSetRef.As<IQueryable<OrganizationRoleElementSetRef>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.organizationElementSetRefDb.GetEnumerator());
                mockOrganizationElementSetRef.Setup(d => d.Add(It.IsAny<OrganizationRoleElementSetRef>())).Callback<OrganizationRoleElementSetRef>((s) => MockDataContext.organizationElementSetRefDb.ToList().Add(s));

                mockRadElementContext.Setup(c => c.Element).Returns(mockElement.Object);
                mockRadElementContext.Setup(c => c.ElementSet).Returns(mockSet.Object);
                mockRadElementContext.Setup(c => c.ElementSetRef).Returns(mockElementSetRef.Object);
                mockRadElementContext.Setup(c => c.ElementValue).Returns(mockElementValue.Object);
                mockRadElementContext.Setup(c => c.Organization).Returns(mockOrganization.Object);
                mockRadElementContext.Setup(c => c.OrganizationRoleElementRef).Returns(mockOrganizationElementRef.Object);
                mockRadElementContext.Setup(c => c.OrganizationRoleElementSetRef).Returns(mockOrganizationElementSetRef.Object);
            }

            var mockConfigurationManager = new Mock<IConfigurationManager>();
            var options = new DbContextOptionsBuilder<RadElementDbContext>().UseMySql(connectionString).Options;

            mockRadElementContext.Setup(c => c.Database).Returns(new DatabaseFacade(new RadElementDbContext(options, mockConfigurationManager.Object)));
        }

        #endregion
    }
}
