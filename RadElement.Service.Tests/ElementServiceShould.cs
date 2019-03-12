using Moq;
using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Core.Services;
using RadElement.Service.Tests.Mocks.Data;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace RadElement.Service.Tests
{
    public class ElementServiceShould
    {
        private readonly Mock<IElementService> mockElementService;
        
        private const string elementNotFoundMessage = "No such element with id '{0}'";
        private const string elementSetIdNotFoundMessage = "No such elements with set id '{0}'.";
        private const string elementNotFoundMessageWithSearchMessage = "No such element with keyword '{0}'.";
        private const string invalidSearchMessage = "Keyword '{0}' given is invalid";
        private const string dataElementInvalidMessage = "Dataelement fields are invalid in request";
        private const string labelInvalidMessage = "'Label' field is missing in request";
        private const string choiceInvalidMessage = "'Options' field is missing for Choice type elements in request";
        private const string elemenIdandSetIdInvalidMessage = "No such element with set id {0} and element id {1}.";
        private const string elementUpdateMessage = "Element with set id {0} and element id {1} is updated.";
        private const string elemenIdDeletedMessage = "Element with set id {0} and element id {1} is deleted.";

        public ElementServiceShould()
        {
            mockElementService = new Mock<IElementService>();
        }

        #region GetElements

        [Fact]
        public async void GetElementsShouldReturnAllElements()
        {
            mockElementService.Setup(x => x.GetElements()).ReturnsAsync(MockElementDataContext.GetElements());            
            var result = await mockElementService.Object.GetElements();
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
            mockElementService.Setup(x => x.GetElement(It.IsAny<int>())).ReturnsAsync(MockElementDataContext.GetElement(elementId));
            var result = await mockElementService.Object.GetElement(elementId);
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
            mockElementService.Setup(x => x.GetElement(It.IsAny<int>())).ReturnsAsync(MockElementDataContext.GetElement(elementId));
            var result = await mockElementService.Object.GetElement(elementId);
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
            mockElementService.Setup(x => x.GetElementsBySetId(It.IsAny<int>())).ReturnsAsync(MockElementDataContext.GetElementsBySetId(setId));
            var result = await mockElementService.Object.GetElementsBySetId(setId);
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
            mockElementService.Setup(x => x.GetElementsBySetId(It.IsAny<int>())).ReturnsAsync(MockElementDataContext.GetElementsBySetId(setId));
            var result = await mockElementService.Object.GetElementsBySetId(setId);
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
            mockElementService.Setup(x => x.SearchElement(It.IsAny<string>())).ReturnsAsync(MockElementDataContext.SearchElement(searchKeyword));
            var result = await mockElementService.Object.SearchElement(searchKeyword);
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
            mockElementService.Setup(x => x.SearchElement(It.IsAny<string>())).ReturnsAsync(MockElementDataContext.SearchElement(searchKeyword));
            var result = await mockElementService.Object.SearchElement(searchKeyword);
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
            mockElementService.Setup(x => x.SearchElement(It.IsAny<string>())).ReturnsAsync(MockElementDataContext.SearchElement(searchKeyword));
            var result = await mockElementService.Object.SearchElement(searchKeyword);
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
            mockElementService.Setup(x => x.CreateElement(It.IsAny<int>(), It.IsAny<DataElementType>(), It.IsAny<CreateUpdateElement>()))
                              .ReturnsAsync(MockElementDataContext.CreateElement(setId, elementType, dataElement));
            var result = await mockElementService.Object.CreateElement(setId, elementType, dataElement);
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
            mockElementService.Setup(x => x.CreateElement(It.IsAny<int>(), It.IsAny<DataElementType>(), It.IsAny<CreateUpdateElement>()))
                              .ReturnsAsync(MockElementDataContext.CreateElement(setId, elementType, dataElement));
            var result = await mockElementService.Object.CreateElement(setId, elementType, dataElement);
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

            mockElementService.Setup(x => x.CreateElement(It.IsAny<int>(), It.IsAny<DataElementType>(), It.IsAny<CreateUpdateElement>()))
                              .ReturnsAsync(MockElementDataContext.CreateElement(setId, elementType, dataElement));
            var result = await mockElementService.Object.CreateElement(setId, elementType, dataElement);
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

            mockElementService.Setup(x => x.CreateElement(It.IsAny<int>(), It.IsAny<DataElementType>(), It.IsAny<CreateUpdateElement>()))
                              .ReturnsAsync(MockElementDataContext.CreateElement(setId, elementType, dataElement));
            var result = await mockElementService.Object.CreateElement(setId, elementType, dataElement);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ElementIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
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

            mockElementService.Setup(x => x.CreateElement(It.IsAny<int>(), It.IsAny<DataElementType>(), It.IsAny<CreateUpdateElement>()))
                              .ReturnsAsync(MockElementDataContext.CreateElement(setId, elementType, dataElement));
            var result = await mockElementService.Object.CreateElement(setId, elementType, dataElement);
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
            mockElementService.Setup(x => x.UpdateElement(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DataElementType>(), It.IsAny<CreateUpdateElement>()))
                              .ReturnsAsync(MockElementDataContext.UpdateElement(setId, elementId, elementType, dataElement));
            var result = await mockElementService.Object.UpdateElement(setId, elementId, elementType, dataElement);
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
            mockElementService.Setup(x => x.UpdateElement(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DataElementType>(), It.IsAny<CreateUpdateElement>()))
                              .ReturnsAsync(MockElementDataContext.UpdateElement(setId, elementId, elementType, dataElement));
            var result = await mockElementService.Object.UpdateElement(setId, elementId, elementType, dataElement);
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

            mockElementService.Setup(x => x.UpdateElement(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DataElementType>(), It.IsAny<CreateUpdateElement>()))
                              .ReturnsAsync(MockElementDataContext.UpdateElement(setId, elementId, elementType, dataElement));
            var result = await mockElementService.Object.UpdateElement(setId, elementId, elementType, dataElement);
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

            mockElementService.Setup(x => x.UpdateElement(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DataElementType>(), It.IsAny<CreateUpdateElement>()))
                              .ReturnsAsync(MockElementDataContext.UpdateElement(setId, elementId, elementType, dataElement));
            var result = await mockElementService.Object.UpdateElement(setId, elementId, elementType, dataElement);
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

            mockElementService.Setup(x => x.UpdateElement(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DataElementType>(), It.IsAny<CreateUpdateElement>()))
                              .ReturnsAsync(MockElementDataContext.UpdateElement(setId, elementId, elementType, dataElement));
            var result = await mockElementService.Object.UpdateElement(setId, elementId, elementType, dataElement);
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

            mockElementService.Setup(x => x.DeleteElement(It.IsAny<int>(), It.IsAny<int>()))
                              .ReturnsAsync(MockElementDataContext.DeleteElement(setId, elementId));
            var result = await mockElementService.Object.DeleteElement(setId, elementId);
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
            mockElementService.Setup(x => x.DeleteElement(It.IsAny<int>(), It.IsAny<int>()))
                              .ReturnsAsync(MockElementDataContext.DeleteElement(setId, elementId));
            var result = await mockElementService.Object.DeleteElement(setId, elementId);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(elemenIdDeletedMessage, setId, elementId), result.Value);
        }

        #endregion
    }
}
