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
    public class ElementServiceShould
    {
        /// <summary>
        /// The service
        /// </summary>
        private readonly ElementService service;

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
        private const string elementNotFoundMessage = "No such element with id '{0}'.";
        private const string elementSetIdNotFoundMessage = "No such elements with set id '{0}'.";
        private const string elementNotFoundMessageWithSearchMessage = "No such element with keyword '{0}'.";
        private const string invalidSearchMessage = "Keyword given is invalid.";
        private const string dataElementInvalidMessage = "Element fields are invalid.";
        private const string choiceInvalidMessage = "'Options' field are missing for Choice type elements.";
        private const string elemenIdandSetIdInvalidMessage = "No such element with set id '{0}' and element id '{1}'.";
        private const string elemenIdInvalidMessage = "No such element with element id '{0}'.";
        private const string elementUpdateMessage = "Element with set id '{0}' and element id '{1}' is updated.";
        private const string elemenIdDeletedMessage = "Element with set id '{0}' and element id '{1}' is deleted.";
        private const string elemenIdMappedMessage = "Element with set id '{0}' and element id '{1}' is mapped.";
        private const string setIdInvalidMessage = "No such set with set id {0}.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementServiceShould"/> class.
        /// </summary>
        public ElementServiceShould()
        {
            mockRadElementContext = new Mock<RadElementDbContext>();
            mockLogger = new Mock<ILogger>();

            var elementProfile = new ElementProfile();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(elementProfile);
            });

            mapper = new Mapper(mapperConfig);
            service = new ElementService(mockRadElementContext.Object, mapper, mockLogger.Object);
        }

        #region GetElements

        [Fact]
        public async void GetElementsShouldThrowInternalServerErrorForExceptions()
        {
            IntializeMockData(false);
            var result = await service.GetElements();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Fact]
        public async void GetElementsShouldReturnAllElements()
        {
            IntializeMockData(true);
            var result = await service.GetElements();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<ElementDetails>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetElement By ElementId

        [Theory]
        [InlineData("RDE1")]
        [InlineData("RDE2")]
        public async void GetElementByIdShouldThrowInternalServerErrorForExceptions(string elementId)
        {
            IntializeMockData(false);
            var result = await service.GetElement(elementId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("RD1")]
        [InlineData("RD2")]
        public async void GetElementByIdShouldReturnNotFoundIfDoesnotExists(string elementId)
        {
            IntializeMockData(true);
            var result = await service.GetElement(elementId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(elementNotFoundMessage, elementId), result.Value);
        }

        [Theory]
        [InlineData("RDE338")]
        [InlineData("RDE340")]
        public async void GetElementByIdShouldReturnElementsBasedOnElementId(string elementId)
        {
            IntializeMockData(true);
            var result = await service.GetElement(elementId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ElementDetails>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetElement By SetId

        [Theory]
        [InlineData("RDES1")]
        [InlineData("RDES2")]
        public async void GetElementBySetIdShouldThrowInternalServerErrorForExceptions(string setId)
        {
            IntializeMockData(false);
            var result = await service.GetElementsBySetId(setId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("RD1")]
        [InlineData("RD2")]
        public async void GetElementBySetIdShouldReturnNotFoundIfDoesnotExists(string setId)
        {
            IntializeMockData(true);
            var result = await service.GetElementsBySetId(setId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(elementSetIdNotFoundMessage, setId), result.Value);
        }

        [Theory]
        [InlineData("RDES53")]
        [InlineData("RDES66")]
        public async void GetElementBySetIdShouldReturnElementsBasedOnElementId(string setId)
        {
            IntializeMockData(true);
            var result = await service.GetElementsBySetId(setId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<ElementDetails>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region SearchElement

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async void SearchSetShouldReturnBadRequestIfSearchKeywordIsInvalid(string searchKeyword)
        {
            IntializeMockData(true);
            var result = await service.SearchElements(new SearchKeyword { Keyword = searchKeyword });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(string.Format(invalidSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("test1")]
        public async void SearchElementShouldReturnEmpyElementsIfSearchKeywordDoesnotExists(string searchKeyword)
        {
            IntializeMockData(true);
            var result = await service.SearchElements(new SearchKeyword { Keyword = searchKeyword });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(elementNotFoundMessageWithSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("Tumuor")]
        public async void SearchElementShouldReturnThrowInternalServerErrorForExceptions(string searchKeyword)
        {
            IntializeMockData(false);
            var result = await service.SearchElements(new SearchKeyword { Keyword = searchKeyword });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("Tumuor")]
        public async void GetElementShouldReturnElementsIfSearchedElementExists(string searchKeyword)
        {
            IntializeMockData(true);
            var result = await service.SearchElements(new SearchKeyword { Keyword = searchKeyword });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<ElementDetails>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region CreateElement

        [Theory]
        [InlineData("RDES1", null)]
        [InlineData("RDES2", null)]
        public async void CreateElementShouldReturnBadRequestIfDataElementIsInvalid(string setId, CreateElement dataElement)
        {
            IntializeMockData(true);
            var result = await service.CreateElement(setId, dataElement);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(dataElementInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("RDES1", DataElementType.Choice)]
        [InlineData("RDES2", DataElementType.Choice)]
        public async void CreateElementShouldReturnBadRequestIfDataElementChoiceIsInvalid(string setId, DataElementType elementType)
        {
            var dataElement = new CreateElement();
            dataElement.Name = "Tumuor";
            dataElement.ValueType = elementType;

            IntializeMockData(true);
            var result = await service.CreateElement(setId, dataElement);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(choiceInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("RDES100", DataElementType.Integer)]
        public async void CreateElementShouldReturnBadRequestIfSetIdIsInvalid(string setId, DataElementType elementType)
        {
            IntializeMockData(true);
            var dataElement = new CreateElement();
            dataElement.Name = "Tumuor";
            dataElement.ValueType = elementType;

            var result = await service.CreateElement(setId, dataElement);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(setIdInvalidMessage, setId), result.Value);
        }

        [Theory]
        [InlineData("RDES74", "RDE1500", DataElementType.Integer)]
        public async void CreateElementShouldReturnBadRequestIfElementIdIsInvalid(string setId, string elementId, DataElementType elementType)
        {
            var dataElement = new CreateElement();
            dataElement.ElementId = elementId;
            dataElement.Name = "Tumuor";
            dataElement.ValueType = elementType;

            IntializeMockData(true);
            var result = await service.CreateElement(setId, dataElement);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(elemenIdInvalidMessage, elementId), result.Value);
        }

        [Theory]
        [InlineData("RDES74", DataElementType.Choice)]
        [InlineData("RDES72", DataElementType.Numeric)]
        [InlineData("RDES66", DataElementType.Integer)]
        [InlineData("RDES53", DataElementType.MultiChoice)]
        public async void CreateElementShouldReturnThrowInternalServerErrorForExceptions(string setId, DataElementType elementType)
        {
            var dataElement = new CreateElement();
            dataElement.Name = "Tumuor";
            dataElement.Definition = "Tumuor vein";
            dataElement.ValueType = elementType;

            if (dataElement.ValueType == DataElementType.Integer)
            {
                dataElement.ValueMin = 1;
                dataElement.ValueMax = 3;
            }
            else if (dataElement.ValueType == DataElementType.Numeric)
            {
                dataElement.ValueMin = 1f;
                dataElement.ValueMax = 3f;
            }
            else if (dataElement.ValueType == DataElementType.Choice || dataElement.ValueType == DataElementType.MultiChoice)
            {
                dataElement.Options = new List<Option>();
                dataElement.Options.AddRange(
                    new List<Option>()
                    {
                        new Option { Name = "value1", Value = "1", Definition = "1", Images = "1" },
                        new Option { Name = "value2", Value = "2", Definition = "2", Images = "2" },
                        new Option { Name = "value3", Value = "3", Definition = "3", Images = "3" }
                    }
                );
            }

            IntializeMockData(false);
            var result = await service.CreateElement(setId, dataElement);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("RDES74", DataElementType.Choice)]
        [InlineData("RDES72", DataElementType.Numeric)]
        [InlineData("RDES66", DataElementType.Integer)]
        [InlineData("RDES53", DataElementType.MultiChoice)]
        public async void CreateElementShouldReturnElementIdIfDataElementIsValid(string setId, DataElementType elementType)
        {
            var dataElement = new CreateElement();
            dataElement.Name = "Tumuor";
            dataElement.Definition = "Tumuor vein";
            dataElement.ValueType = elementType;

            if (dataElement.ValueType == DataElementType.Integer)
            {
                dataElement.ValueMin = 1;
                dataElement.ValueMax = 3;
            }
            else if (dataElement.ValueType == DataElementType.Numeric)
            {
                dataElement.ValueMin = 1f;
                dataElement.ValueMax = 3f;
            }
            else if (dataElement.ValueType == DataElementType.Choice || dataElement.ValueType == DataElementType.MultiChoice)
            {
                dataElement.Options = new List<Option>();
                dataElement.Options.AddRange(
                    new List<Option>()
                    {
                        new Option { Name = "value1", Value = "1", Definition = "1", Images = "1" },
                        new Option { Name = "value2", Value = "2", Definition = "2", Images = "2" },
                        new Option { Name = "value3", Value = "3", Definition = "3", Images = "3" }
                    }
                );
            }

            IntializeMockData(true);
            var result = await service.CreateElement(setId, dataElement);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ElementIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        [Theory]
        [InlineData("RDES74", "RDE340", DataElementType.Choice)]
        [InlineData("RDES72", "RDE338", DataElementType.Numeric)]
        [InlineData("RDES66", "RDE307", DataElementType.Integer)]
        [InlineData("RDES53", "RDE283", DataElementType.MultiChoice)]
        public async void CreateElementShouldReturnElementIdIfDataElementIsValid1(string setId, string elementId, DataElementType elementType)
        {
            var dataElement = new CreateElement();
            dataElement.ElementId = elementId;
            dataElement.Name = "Tumuor";
            dataElement.Definition = "Tumuor vein";
            dataElement.ValueType = elementType;

            if (dataElement.ValueType == DataElementType.Integer)
            {
                dataElement.ValueMin = 1;
                dataElement.ValueMax = 3;
            }
            else if (dataElement.ValueType == DataElementType.Numeric)
            {
                dataElement.ValueMin = 1f;
                dataElement.ValueMax = 3f;
            }
            else if (dataElement.ValueType == DataElementType.Choice || dataElement.ValueType == DataElementType.MultiChoice)
            {
                dataElement.Options = new List<Option>();
                dataElement.Options.AddRange(
                    new List<Option>()
                    {
                        new Option { Name = "value1", Value = "1", Definition = "1", Images = "1" },
                        new Option { Name = "value2", Value = "2", Definition = "2", Images = "2" },
                        new Option { Name = "value3", Value = "3", Definition = "3", Images = "3" }
                    }
                );
            }

            IntializeMockData(true);
            var result = await service.CreateElement(setId, dataElement);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(string.Format(elemenIdMappedMessage, setId, elementId), result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        #endregion

        #region UpdateElement

        [Theory]
        [InlineData("RDES1", "RDE1", null)]
        [InlineData("RDES2", "RDE2", null)]
        public async void UpdateElementShouldReturnBadRequestIfDataElementIsInvalid(string setId, string elementId, UpdateElement dataElement)
        {
            IntializeMockData(true);
            var result = await service.UpdateElement(setId, elementId, dataElement);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(dataElementInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("RDES1", "RDE1", DataElementType.Choice)]
        [InlineData("RDES2", "RDE2", DataElementType.Choice)]
        public async void UpdateElementShouldReturnBadRequestIfDataElementChoiceIsInvalid(string setId, string elementId, DataElementType elementType)
        {
            IntializeMockData(true);
            var dataElement = new UpdateElement();
            dataElement.Name = "Tumuor";
            dataElement.ValueType = elementType;

            var result = await service.UpdateElement(setId, elementId, dataElement);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(choiceInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("RDES100", "RDE100", DataElementType.Integer)]
        [InlineData("RDES100", "RDE100", DataElementType.Numeric)]
        public async void UpdateElementShouldReturnNotFoundIfSetIdAndElementIdIsInvalid(string setId, string elementId, DataElementType elementType)
        {
            var dataElement = new UpdateElement();
            dataElement.Name = "Tumuor";
            dataElement.ValueType = elementType;

            IntializeMockData(true);
            var result = await service.UpdateElement(setId, elementId, dataElement);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(elemenIdandSetIdInvalidMessage, setId, elementId), result.Value);
        }

        [Theory]
        [InlineData("RDES74", "RDE340", DataElementType.Choice)]
        [InlineData("RDES72", "RDE338", DataElementType.Numeric)]
        [InlineData("RDES66", "RDE307", DataElementType.Integer)]
        [InlineData("RDES53", "RDE283", DataElementType.MultiChoice)]
        public async void UpdateElementShouldReturnThrowInternalServerErrorForExceptions(string setId, string elementId, DataElementType elementType)
        {
            var dataElement = new UpdateElement();
            dataElement.Name = "Tumuor";
            dataElement.Definition = "Tumuor vein";
            dataElement.ValueType = elementType;

            if (dataElement.ValueType == DataElementType.Integer)
            {
                dataElement.ValueMin = 1;
                dataElement.ValueMax = 3;
            }
            else if (dataElement.ValueType == DataElementType.Numeric)
            {
                dataElement.ValueMin = 1f;
                dataElement.ValueMax = 3f;
            }
            else if (dataElement.ValueType == DataElementType.Choice || dataElement.ValueType == DataElementType.MultiChoice)
            {
                dataElement.Options = new List<Option>();
                dataElement.Options.AddRange(
                    new List<Option>()
                    {
                        new Option { Name = "value1", Value = "1", Definition = "1", Images = "1" },
                        new Option { Name = "value2", Value = "2", Definition = "2", Images = "2" },
                        new Option { Name = "value3", Value = "3", Definition = "3", Images = "3" }
                    }
                );
            }

            IntializeMockData(false);
            var result = await service.UpdateElement(setId, elementId, dataElement);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("RDES74", "RDE340", DataElementType.Choice)]
        [InlineData("RDES72", "RDE338", DataElementType.Numeric)]
        [InlineData("RDES66", "RDE307", DataElementType.Integer)]
        [InlineData("RDES53", "RDE283", DataElementType.MultiChoice)]
        public async void UpdateElementShouldReturnElementIdIfDataElementIsValid(string setId, string elementId, DataElementType elementType)
        {
            var dataElement = new UpdateElement();
            dataElement.Name = "Tumuor";
            dataElement.Definition = "Tumuor vein";
            dataElement.ValueType = elementType;

            if (dataElement.ValueType == DataElementType.Integer)
            {
                dataElement.ValueMin = 1;
                dataElement.ValueMax = 3;
            }
            else if (dataElement.ValueType == DataElementType.Numeric)
            {
                dataElement.ValueMin = 1f;
                dataElement.ValueMax = 3f;
            }
            else if (dataElement.ValueType == DataElementType.Choice || dataElement.ValueType == DataElementType.MultiChoice)
            {
                dataElement.Options = new List<Option>();
                dataElement.Options.AddRange(
                    new List<Option>()
                    {
                        new Option { Name = "value1", Value = "1", Definition = "1", Images = "1" },
                        new Option { Name = "value2", Value = "2", Definition = "2", Images = "2" },
                        new Option { Name = "value3", Value = "3", Definition = "3", Images = "3" }
                    }
                );
            }

            IntializeMockData(true);
            var result = await service.UpdateElement(setId, elementId, dataElement);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.NotEqual(0, result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(elementUpdateMessage, setId, elementId), result.Value);
        }

        #endregion

        #region DeleteElement

        [Theory]
        [InlineData("RDES100", "RDE100")]
        [InlineData("RDES200", "RDE200")]
        public async void DeleteElementShouldReturnNotFoundIfSetIdAndElementIdIsInvalid(string setId, string elementId)
        {
            IntializeMockData(true);
            var result = await service.DeleteElement(setId, elementId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(elemenIdandSetIdInvalidMessage, setId, elementId), result.Value);
        }

        [Theory]
        [InlineData("RDES74", "RDE340")]
        public async void DeleteElementShouldThrowInternalServerErrorForExceptions(string setId, string elementId)
        {
            IntializeMockData(false);
            var result = await service.DeleteElement(setId, elementId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("RDES74", "RDE340")]
        public async void DeleteElementShouldDeleteElementIfElementIdAndSetIdIsValid(string setId, string elementId)
        {
            IntializeMockData(true);
            var result = await service.DeleteElement(setId, elementId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(elemenIdDeletedMessage, setId, elementId), result.Value);
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

                var mockElementSetRef = new Mock<DbSet<ElementSetRef>>();
                mockElementSetRef.As<IQueryable<ElementSetRef>>().Setup(m => m.Provider).Returns(MockDataContext.elementSetRefDb.Provider);
                mockElementSetRef.As<IQueryable<ElementSetRef>>().Setup(m => m.Expression).Returns(MockDataContext.elementSetRefDb.Expression);
                mockElementSetRef.As<IQueryable<ElementSetRef>>().Setup(m => m.ElementType).Returns(MockDataContext.elementSetRefDb.ElementType);
                mockElementSetRef.As<IQueryable<ElementSetRef>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.elementSetRefDb.GetEnumerator());

                var mockElementValue = new Mock<DbSet<ElementValue>>();
                mockElementValue.As<IQueryable<ElementValue>>().Setup(m => m.Provider).Returns(MockDataContext.elementValueDb.Provider);
                mockElement.As<IQueryable<ElementValue>>().Setup(m => m.Expression).Returns(MockDataContext.elementValueDb.Expression);
                mockElementValue.As<IQueryable<ElementValue>>().Setup(m => m.ElementType).Returns(MockDataContext.elementValueDb.ElementType);
                mockElementValue.As<IQueryable<ElementValue>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.elementValueDb.GetEnumerator());

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
                mockRadElementContext.Setup(c => c.Person).Returns(mockPerson.Object);
                mockRadElementContext.Setup(c => c.PersonRoleElementRef).Returns(mockPersonElementRef.Object);
                mockRadElementContext.Setup(c => c.PersonRoleElementSetRef).Returns(mockPersonElementSetRef.Object);
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
