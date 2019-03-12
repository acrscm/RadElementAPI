using Microsoft.EntityFrameworkCore;
using Moq;
using RadElement.Core.Data;
using RadElement.Core.Domain;
using RadElement.Core.DTO;
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
        private readonly Mock<IRadElementDbContext> mockRadElementContext;
        private readonly Mock<ILogger> mockLogger;

        private const string elementNotFoundMessage = "No such element with id '{0}'";
        private const string elementSetIdNotFoundMessage = "No such elements with set id '{0}'.";
        private const string elementNotFoundMessageWithSearchMessage = "No such element with keyword '{0}'.";
        private const string invalidSearchMessage = "Keyword '{0}' given is invalid";
        private const string dataElementInvalidMessage = "Dataelement fields are invalid in request";
        private const string labelInvalidMessage = "'Label' field is missing in request";
        private const string choiceInvalidMessage = "'Options' field is missing for Choice type elements in request";
        private const string elemenIdandSetIdInvalidMessage = "No such element with set id {0} and element id {1}.";
        private const string setIdInvalidMessage = "No such element with set id {0}.";
        private const string elementUpdateMessage = "Element with set id {0} and element id {1} is updated.";
        private const string elemenIdDeletedMessage = "Element with set id {0} and element id {1} is deleted.";

        public ElementServiceShould()
        {
            mockRadElementContext = new Mock<IRadElementDbContext>();
            mockLogger = new Mock<ILogger>();
            IntializeMockData();
        }

        #region GetElements

        [Fact]
        public async void GetElementsShouldReturnAllElements()
        {
            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.GetElements();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<Element>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetElement By ElementId

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetElementByIdShouldReturnNotFoundIfDoesnotExists(int elementId)
        {
            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.GetElement(elementId);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(elementNotFoundMessage, elementId), result.Value);
        }

        [Theory]
        [InlineData(338)]
        [InlineData(340)]
        public async void GetElementByIdShouldReturnElementsBasedOnElementId(int elementId)
        {
            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.GetElement(elementId);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<Element>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetElement By ElementId

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetElementBySetIdShouldReturnNotFoundIfDoesnotExists(int setId)
        {
            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.GetElementsBySetId(setId);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(elementSetIdNotFoundMessage, setId), result.Value);
        }

        [Theory]
        [InlineData(53)]
        [InlineData(66)]
        public async void GetElementBySetIdShouldReturnElementsBasedOnElementId(int setId)
        {
            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.GetElementsBySetId(setId);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<Element>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region SearchElement

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async void SearchElementShouldReturnBadRequestIfSearchKeywordIsInvalid(string searchKeyword)
        {
            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.SearchElement(searchKeyword);
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
            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.SearchElement(searchKeyword);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(elementNotFoundMessageWithSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("Tumuor")]
        public async void GetElementShouldReturnElementsIfSearchedElementExists(string searchKeyword)
        {
            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.SearchElement(searchKeyword);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<Element>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region CreateElement

        [Theory]
        [InlineData(1, DataElementType.Choice, null)]
        [InlineData(2, DataElementType.Integer, null)]
        public async void CreateElementShouldReturnBadRequestIfDataElementIsInvalid(int setId, DataElementType elementType, CreateUpdateElement dataElement)
        {
            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.CreateElement(setId, elementType, dataElement);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(dataElementInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData(1, DataElementType.Choice)]
        [InlineData(2, DataElementType.Integer)]
        public async void CreateElementShouldReturnBadRequestIfDataElementLabelIsInvalid(int setId, DataElementType elementType)
        {
            var dataElement = new CreateUpdateElement();
            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.CreateElement(setId, elementType, dataElement);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(labelInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData(1, DataElementType.Choice)]
        [InlineData(2, DataElementType.Choice)]
        public async void CreateElementShouldReturnBadRequestIfDataElementChoiceIsInvalid(int setId, DataElementType elementType)
        {
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";

            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.CreateElement(setId, elementType, dataElement);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(choiceInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData(100, DataElementType.Integer)]
        public async void CreateElementShouldReturnBadRequestIfSetIdIsInvalid(int setId, DataElementType elementType)
        {
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";

            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.CreateElement(setId, elementType, dataElement);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(setIdInvalidMessage, setId), result.Value);
        }

        [Theory]
        [InlineData(74, DataElementType.Choice)]
        [InlineData(72, DataElementType.Numeric)]
        [InlineData(66, DataElementType.Integer)]
        [InlineData(53, DataElementType.MultiChoice)]
        public async void CreateElementShouldReturnElementIdIfDataElementIsValid(int setId, DataElementType elementType)
        {
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";
            dataElement.Definition = "Tumuor vein";

            if (elementType == DataElementType.Integer)
            {
                dataElement.ValueMin = 1;
                dataElement.ValueMin = 3;
            }
            else if (elementType == DataElementType.Numeric)
            {
                dataElement.ValueMin = 1f;
                dataElement.ValueMin = 3f;
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

            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.CreateElement(setId, elementType, dataElement);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ElementIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
            Assert.NotEqual(0, result.Value);
        }

        #endregion

        #region UpdateElement

        [Theory]
        [InlineData(1, 1, DataElementType.Choice, null)]
        [InlineData(2, 2, DataElementType.Integer, null)]
        public async void UpdateElementShouldReturnBadRequestIfDataElementIsInvalid(int setId, int elementId, DataElementType elementType, CreateUpdateElement dataElement)
        {
            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.UpdateElement(setId, elementId, elementType, dataElement);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(dataElementInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData(1, 1, DataElementType.Choice)]
        [InlineData(2, 2, DataElementType.Integer)]
        public async void UpdateElementShouldReturnBadRequestIfDataElementLabelIsInvalid(int setId, int elementId, DataElementType elementType)
        {
            var dataElement = new CreateUpdateElement();
            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.UpdateElement(setId, elementId, elementType, dataElement);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(labelInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData(1, 1, DataElementType.Choice)]
        [InlineData(2, 2, DataElementType.Choice)]
        public async void UpdateElementShouldReturnBadRequestIfDataElementChoiceIsInvalid(int setId, int elementId, DataElementType elementType)
        {
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";

            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.UpdateElement(setId, elementId, elementType, dataElement);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(choiceInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData(100, 100, DataElementType.Integer)]
        [InlineData(100, 100, DataElementType.Numeric)]
        public async void UpdateElementShouldReturnNotFoundIfSetIdAndElementIdIsInvalid(int setId, int elementId, DataElementType elementType)
        {
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";

            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.UpdateElement(setId, elementId, elementType, dataElement);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(elemenIdandSetIdInvalidMessage, setId, elementId), result.Value);
        }

        [Theory]
        [InlineData(74, 340, DataElementType.Choice)]
        [InlineData(72, 338, DataElementType.Numeric)]
        [InlineData(66, 307, DataElementType.Integer)]
        [InlineData(53, 283, DataElementType.MultiChoice)]
        public async void UpdateElementShouldReturnElementIdIfDataElementIsValid(int setId, int elementId, DataElementType elementType)
        {
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";
            dataElement.Definition = "Tumuor vein";

            if (elementType == DataElementType.Integer)
            {
                dataElement.ValueMin = 1;
                dataElement.ValueMin = 3;
            }
            else if (elementType == DataElementType.Numeric)
            {
                dataElement.ValueMin = 1f;
                dataElement.ValueMin = 3f;
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

            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.UpdateElement(setId, elementId, elementType, dataElement);
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
        [InlineData(100, 100)]
        [InlineData(200, 200)]
        public async void DeleteElementShouldReturnNotFoundIfSetIdAndElementIdIsInvalid(int setId, int elementId)
        {
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";

            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.DeleteElement(setId, elementId);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(elemenIdandSetIdInvalidMessage, setId, elementId), result.Value);
        }

        [Theory]
        [InlineData(74, 340)]
        public async void DeleteElementShouldReturnDeleteElementIfElementIdIsInvalid(int setId, int elementId)
        {
            var sut = new ElementService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.DeleteElement(setId, elementId);
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
