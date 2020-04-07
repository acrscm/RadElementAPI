using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using RadElement.Core.Data;
using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Core.Profile;
using RadElement.Service.Tests.Mocks.Data;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;

namespace RadElement.Service.Tests
{
    public class ElementServiceShould
    {
        /// <summary>
        /// The sut
        /// </summary>
        private readonly ElementService service;

        /// <summary>
        /// The mock RAD element context
        /// </summary>
        private readonly Mock<IRadElementDbContext> mockRadElementContext;

        /// <summary>
        /// The mock logger
        /// </summary>
        private readonly Mock<ILogger> mockLogger;
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper mapper;

        private const string elementNotFoundMessage = "No such element with id '{0}'";
        private const string elementSetIdNotFoundMessage = "No such elements with set id '{0}'.";
        private const string elementNotFoundMessageWithSearchMessage = "No such element with keyword '{0}'.";
        private const string dataElementInvalidMessage = "Dataelement fields are invalid in request";
        private const string labelInvalidMessage = "'Label' field is missing in request";
        private const string choiceInvalidMessage = "'Options' field is missing for Choice type elements in request";
        private const string elemenIdandSetIdInvalidMessage = "No such element with set id {0} and element id {1}.";
        private const string setIdInvalidMessage = "No such element with set id {0}.";
        private const string elementUpdateMessage = "Element with set id {0} and element id {1} is updated.";
        private const string elemenIdDeletedMessage = "Element with set id {0} and element id {1} is deleted.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementServiceShould"/> class.
        /// </summary>
        public ElementServiceShould()
        {
            mockRadElementContext = new Mock<IRadElementDbContext>();
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
            var result = await service.GetElements();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Fact]
        public async void GetElementsShouldReturnAllElements()
        {
            IntializeMockData();
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
            IntializeMockData();
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
            IntializeMockData();
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
            IntializeMockData();
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
            IntializeMockData();
            var result = await service.GetElementsBySetId(setId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<ElementDetails>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region SearchElement
        
        [Theory]
        [InlineData("test")]
        [InlineData("test1")]
        public async void SearchElementShouldReturnEmpyElementsIfSearchKeywordDoesnotExists(string searchKeyword)
        {
            IntializeMockData();
            var result = await service.SearchElement(new SearchKeyword { Keyword = searchKeyword });

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
            var result = await service.SearchElement(new SearchKeyword { Keyword = searchKeyword });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("Tumuor")]
        public async void GetElementShouldReturnElementsIfSearchedElementExists(string searchKeyword)
        {
            IntializeMockData();
            var result = await service.SearchElement(new SearchKeyword { Keyword = searchKeyword });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<ElementDetails>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region CreateElement

        [Theory]
        [InlineData("RDES1", DataElementType.Choice, null)]
        [InlineData("RDES2", DataElementType.Integer, null)]
        public async void CreateElementShouldReturnBadRequestIfDataElementIsInvalid(string setId, DataElementType elementType, CreateUpdateElement dataElement)
        {
            IntializeMockData();
            var result = await service.CreateElement(setId, elementType, dataElement);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(dataElementInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("RDES1", DataElementType.Choice)]
        [InlineData("RDES2", DataElementType.Integer)]
        public async void CreateElementShouldReturnBadRequestIfDataElementLabelIsInvalid(string setId, DataElementType elementType)
        {
            IntializeMockData();
            var dataElement = new CreateUpdateElement();
            var result = await service.CreateElement(setId, elementType, dataElement);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(labelInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("RDES1", DataElementType.Choice)]
        [InlineData("RDES2", DataElementType.Choice)]
        public async void CreateElementShouldReturnBadRequestIfDataElementChoiceIsInvalid(string setId, DataElementType elementType)
        {
            IntializeMockData();
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";

            var result = await service.CreateElement(setId, elementType, dataElement);

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
            IntializeMockData();
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";

            var result = await service.CreateElement(setId, elementType, dataElement);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(setIdInvalidMessage, setId), result.Value);
        }

        [Theory]
        [InlineData("RDES74", DataElementType.Choice)]
        [InlineData("RDES72", DataElementType.Numeric)]
        [InlineData("RDES66", DataElementType.Integer)]
        [InlineData("RDES53", DataElementType.MultiChoice)]
        public async void CreateElementShouldReturnThrowInternalServerErrorForExceptions(string setId, DataElementType elementType)
        {
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";
            dataElement.Definition = "Tumuor vein";

            if (elementType == DataElementType.Integer)
            {
                dataElement.ValueMin = 1;
                dataElement.ValueMax = 3;
            }
            else if (elementType == DataElementType.Numeric)
            {
                dataElement.ValueMin = 1f;
                dataElement.ValueMax = 3f;
            }
            else if (elementType == DataElementType.Choice || elementType == DataElementType.MultiChoice)
            {
                dataElement.Options = new List<Core.DTO.Option>();
                dataElement.Options.AddRange(
                    new List<Core.DTO.Option>()
                    {
                        new Core.DTO.Option { Label = "value1", Value = "1" },
                        new Core.DTO.Option { Label = "value2", Value = "2" },
                        new Core.DTO.Option { Label = "value3", Value = "3" }
                    }
                );
            }

            var result = await service.CreateElement(setId, elementType, dataElement);

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
            IntializeMockData();
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";
            dataElement.Definition = "Tumuor vein";

            if (elementType == DataElementType.Integer)
            {
                dataElement.ValueMin = 1;
                dataElement.ValueMax = 3;
            }
            else if (elementType == DataElementType.Numeric)
            {
                dataElement.ValueMin = 1f;
                dataElement.ValueMax = 3f;
            }
            else if (elementType == DataElementType.Choice || elementType == DataElementType.MultiChoice)
            {
                dataElement.Options = new List<Core.DTO.Option>();
                dataElement.Options.AddRange(
                    new List<Core.DTO.Option>()
                    {
                        new Core.DTO.Option { Label = "value1", Value = "1" },
                        new Core.DTO.Option { Label = "value2", Value = "2" },
                        new Core.DTO.Option { Label = "value3", Value = "3" }
                    }
                );
            }

            var result = await service.CreateElement(setId, elementType, dataElement);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ElementIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        #endregion

        #region UpdateElement

        [Theory]
        [InlineData("RDES1", "RDE1", DataElementType.Choice, null)]
        [InlineData("RDES2", "RDE2", DataElementType.Integer, null)]
        public async void UpdateElementShouldReturnBadRequestIfDataElementIsInvalid(string setId, string elementId, DataElementType elementType, CreateUpdateElement dataElement)
        {
            IntializeMockData();
            var result = await service.UpdateElement(setId, elementId, elementType, dataElement);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(dataElementInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("RDES1", "RDE1", DataElementType.Choice)]
        [InlineData("RDES2", "RDE2", DataElementType.Integer)]
        public async void UpdateElementShouldReturnBadRequestIfDataElementLabelIsInvalid(string setId, string elementId, DataElementType elementType)
        {
            IntializeMockData();
            var dataElement = new CreateUpdateElement();
            var result = await service.UpdateElement(setId, elementId, elementType, dataElement);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(labelInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("RDES1", "RDE1", DataElementType.Choice)]
        [InlineData("RDES2", "RDE2", DataElementType.Choice)]
        public async void UpdateElementShouldReturnBadRequestIfDataElementChoiceIsInvalid(string setId, string elementId, DataElementType elementType)
        {
            IntializeMockData();
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";

            var result = await service.UpdateElement(setId, elementId, elementType, dataElement);

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
            IntializeMockData();
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";

            var result = await service.UpdateElement(setId, elementId, elementType, dataElement);

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
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";
            dataElement.Definition = "Tumuor vein";

            if (elementType == DataElementType.Integer)
            {
                dataElement.ValueMin = 1;
                dataElement.ValueMax = 3;
            }
            else if (elementType == DataElementType.Numeric)
            {
                dataElement.ValueMin = 1f;
                dataElement.ValueMax = 3f;
            }
            else if (elementType == DataElementType.Choice || elementType == DataElementType.MultiChoice)
            {
                dataElement.Options = new List<Core.DTO.Option>();
                dataElement.Options.AddRange(
                    new List<Core.DTO.Option>()
                    {
                        new Core.DTO.Option { Label = "value1", Value = "1" },
                        new Core.DTO.Option { Label = "value2", Value = "2" },
                        new Core.DTO.Option { Label = "value3", Value = "3" }
                    }
                );
            }

            var result = await service.UpdateElement(setId, elementId, elementType, dataElement);

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
            IntializeMockData();
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";
            dataElement.Definition = "Tumuor vein";

            if (elementType == DataElementType.Integer)
            {
                dataElement.ValueMin = 1;
                dataElement.ValueMax = 3;
            }
            else if (elementType == DataElementType.Numeric)
            {
                dataElement.ValueMin = 1f;
                dataElement.ValueMax = 3f;
            }
            else if (elementType == DataElementType.Choice || elementType == DataElementType.MultiChoice)
            {
                dataElement.Options = new List<Core.DTO.Option>();
                dataElement.Options.AddRange(
                    new List<Core.DTO.Option>()
                    {
                        new Core.DTO.Option { Label = "value1", Value = "1" },
                        new Core.DTO.Option { Label = "value2", Value = "2" },
                        new Core.DTO.Option { Label = "value3", Value = "3" }
                    }
                );
            }

            var result = await service.UpdateElement(setId, elementId, elementType, dataElement);

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
            IntializeMockData();
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";

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
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";

            var result = await service.DeleteElement(setId, elementId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("RDES74", "RDE340")]
        public async void DeleteElementShouldDeleteElementIfElementIdAndSetIdIsValid(string setId, string elementId)
        {
            IntializeMockData();
            var result = await service.DeleteElement(setId, elementId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(elemenIdDeletedMessage, setId, elementId), result.Value);
        }

        #endregion

        #region Private Methods

        private void IntializeMockData()
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

            mockRadElementContext.Setup(c => c.Element).Returns(mockElement.Object);
            mockRadElementContext.Setup(c => c.ElementSet).Returns(mockSet.Object);
            mockRadElementContext.Setup(c => c.ElementSetRef).Returns(mockElementSetRef.Object);
            mockRadElementContext.Setup(c => c.ElementValue).Returns(mockElementValue.Object);
        }

        #endregion
    }
}
