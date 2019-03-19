using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RadElement.Core.Data;
using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Core.Infrastructure;
using RadElement.Data;
using RadElement.Infrastructure;
using RadElement.Service;
using Serilog;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace RadElement.API.IntegrationTests
{
    public class ElementServiceShould
    {
        private readonly IRadElementDbContext radElementContext;
        private readonly IConfigurationManager configurationManager;
        private readonly ILogger logger;

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
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("AppSettings.json");
            IConfiguration configuration = configurationBuilder.Build();
            configurationManager = new ConfigurationManager(configuration);
            var optionsBuilder = new DbContextOptionsBuilder<RadElementDbContext>();
            radElementContext = new RadElementDbContext(optionsBuilder.Options, configurationManager);
            logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        #region GetElements

        [Fact]
        public async void GetElementsShouldReturnAllElements()
        {
            var sut = new ElementService(radElementContext, logger);
            var result = await sut.GetElements();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<ElementDetails>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetElement By ElementId

        [Theory]
        [InlineData("RDEF1")]
        [InlineData("RDEF2")]
        public async void GetElementByIdShouldReturnNotFoundIfDoesnotExists(string elementId)
        {
            var sut = new ElementService(radElementContext, logger);
            var result = await sut.GetElement(elementId);
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
            var sut = new ElementService(radElementContext, logger);
            var result = await sut.GetElement(elementId);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ElementDetails>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetElement By SetId

        [Theory]
        [InlineData("RDESF1")]
        [InlineData("RDESF2")]
        public async void GetElementBySetIdShouldReturnNotFoundIfDoesnotExists(string setId)
        {
            var sut = new ElementService(radElementContext, logger);
            var result = await sut.GetElementsBySetId(setId);
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
            var sut = new ElementService(radElementContext, logger);
            var result = await sut.GetElementsBySetId(setId);
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
        public async void SearchElementShouldReturnBadRequestIfSearchKeywordIsInvalid(string searchKeyword)
        {
            var sut = new ElementService(radElementContext, logger);
            var result = await sut.SearchElement(searchKeyword);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(string.Format(invalidSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("InvalidModule")]
        [InlineData("InvalidElement")]
        public async void SearchElementShouldReturnEmpyElementsIfSearchKeywordDoesnotExists(string searchKeyword)
        {
            var sut = new ElementService(radElementContext, logger);
            var result = await sut.SearchElement(searchKeyword);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(elementNotFoundMessageWithSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("Glasgow")]
        public async void GetElementShouldReturnElementsIfSearchedElementExists(string searchKeyword)
        {
            var sut = new ElementService(radElementContext, logger);
            var result = await sut.SearchElement(searchKeyword);
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
            var sut = new ElementService(radElementContext, logger);
            var result = await sut.CreateElement(setId, elementType, dataElement);
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
            var dataElement = new CreateUpdateElement();
            var sut = new ElementService(radElementContext, logger);
            var result = await sut.CreateElement(setId, elementType, dataElement);
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
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";

            var sut = new ElementService(radElementContext, logger);
            var result = await sut.CreateElement(setId, elementType, dataElement);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(choiceInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("RDESF100", DataElementType.Integer)]
        public async void CreateElementShouldReturnBadRequestIfSetIdIsInvalid(string setId, DataElementType elementType)
        {
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";

            var sut = new ElementService(radElementContext, logger);
            var result = await sut.CreateElement(setId, elementType, dataElement);
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
        public async void CreateElementShouldReturnElementIdIfDataElementIsValid(string setId, DataElementType elementType)
        {
            // Creates temporary element
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

            var sut = new ElementService(radElementContext, logger);
            var createdResult = await sut.CreateElement(setId, elementType, dataElement);
            Assert.NotNull(createdResult);
            Assert.NotNull(createdResult.Value);
            Assert.IsType<ElementIdDetails>(createdResult.Value);
            Assert.Equal(HttpStatusCode.Created, createdResult.Code);

            var elementDetails = createdResult.Value as ElementIdDetails;

            // Deletes temporary element
            var deleteResult = await sut.DeleteElement(setId, elementDetails.ElementId);
            Assert.Equal(HttpStatusCode.OK, deleteResult.Code);
            Assert.Equal(string.Format(elemenIdDeletedMessage, setId, elementDetails.ElementId), deleteResult.Value);
        }

        #endregion

        #region UpdateElement

        [Theory]
        [InlineData("RDES1", "RDE1", DataElementType.Choice, null)]
        [InlineData("RDES2", "RDE2", DataElementType.Integer, null)]
        public async void UpdateElementShouldReturnBadRequestIfDataElementIsInvalid(string setId, string elementId, DataElementType elementType, CreateUpdateElement dataElement)
        {
            var sut = new ElementService(radElementContext, logger);
            var result = await sut.UpdateElement(setId, elementId, elementType, dataElement);
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
            var dataElement = new CreateUpdateElement();
            var sut = new ElementService(radElementContext, logger);
            var result = await sut.UpdateElement(setId, elementId, elementType, dataElement);
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
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";

            var sut = new ElementService(radElementContext, logger);
            var result = await sut.UpdateElement(setId, elementId, elementType, dataElement);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(choiceInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("RDESF100", "RDEF100", DataElementType.Integer)]
        [InlineData("RDESF100", "RDEF100", DataElementType.Numeric)]
        public async void UpdateElementShouldReturnNotFoundIfSetIdAndElementIdIsInvalid(string setId, string elementId, DataElementType elementType)
        {
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";

            var sut = new ElementService(radElementContext, logger);
            var result = await sut.UpdateElement(setId, elementId, elementType, dataElement);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(elemenIdandSetIdInvalidMessage, setId, elementId), result.Value);
        }

        [Theory]
        [InlineData("RDES74", DataElementType.Choice)]
        [InlineData("RDES72", DataElementType.Numeric)]
        [InlineData("RDES66", DataElementType.Integer)]
        [InlineData("RDES53", DataElementType.MultiChoice)]
        public async void UpdateElementShouldReturnElementIdIfDataElementIsValid(string setId, DataElementType elementType)
        {
            // Creates temporary element
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

            var sut = new ElementService(radElementContext, logger);
            var createdResult = await sut.CreateElement(setId, elementType, dataElement);
            Assert.NotNull(createdResult);
            Assert.NotNull(createdResult.Value);
            Assert.IsType<ElementIdDetails>(createdResult.Value);
            Assert.Equal(HttpStatusCode.Created, createdResult.Code);

            var elementDetails = createdResult.Value as ElementIdDetails;

            //Updates temporary element
            dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuoredit";
            dataElement.Definition = "Tumuor vein edit";

            if (elementType == DataElementType.Integer)
            {
                dataElement.ValueMin = 5;
                dataElement.ValueMax = 6;
            }
            else if (elementType == DataElementType.Numeric)
            {
                dataElement.ValueMin = 5f;
                dataElement.ValueMax = 6f;
            }
            else if (elementType == DataElementType.Choice || elementType == DataElementType.MultiChoice)
            {
                dataElement.Options = new List<Core.DTO.Option>();
                dataElement.Options.AddRange(
                    new List<Core.DTO.Option>()
                    {
                        new Core.DTO.Option { Label = "value11", Value = "11" },
                        new Core.DTO.Option { Label = "value22", Value = "22" },
                        new Core.DTO.Option { Label = "value33", Value = "33" }
                    }
                );
            }

            var updatedResult = await sut.UpdateElement(setId, elementDetails.ElementId, elementType, dataElement);
            Assert.NotNull(updatedResult);
            Assert.NotNull(updatedResult.Value);
            Assert.IsType<string>(updatedResult.Value);
            Assert.Equal(HttpStatusCode.OK, updatedResult.Code);
            Assert.NotEqual(0, updatedResult.Value);
            Assert.Equal(HttpStatusCode.OK, updatedResult.Code);
            Assert.Equal(string.Format(elementUpdateMessage, setId, elementDetails.ElementId), updatedResult.Value);

            // Deletes temporary element
            var deleteResult = await sut.DeleteElement(setId, elementDetails.ElementId);
            Assert.Equal(HttpStatusCode.OK, deleteResult.Code);
            Assert.Equal(string.Format(elemenIdDeletedMessage, setId, elementDetails.ElementId), deleteResult.Value);
        }

        #endregion

        #region DeleteElement

        [Theory]
        [InlineData("RDES100", "RDE100")]
        [InlineData("RDES200", "RDE200")]
        public async void DeleteElementShouldReturnNotFoundIfSetIdAndElementIdIsInvalid(string setId, string elementId)
        {
            var dataElement = new CreateUpdateElement();
            dataElement.Label = "Tumuor";

            var sut = new ElementService(radElementContext, logger);
            var result = await sut.DeleteElement(setId, elementId);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(elemenIdandSetIdInvalidMessage, setId, elementId), result.Value);
        }

        [Theory]
        [InlineData("RDES74", DataElementType.Choice)]
        public async void DeleteElementShouldReturnDeleteElementIfElementIdIsInvalid(string setId, DataElementType elementType)
        {
            // Creates temporary element
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

            var sut = new ElementService(radElementContext, logger);
            var createdResult = await sut.CreateElement(setId, elementType, dataElement);
            Assert.NotNull(createdResult);
            Assert.NotNull(createdResult.Value);
            Assert.IsType<ElementIdDetails>(createdResult.Value);
            Assert.Equal(HttpStatusCode.Created, createdResult.Code);

            var elementDetails = createdResult.Value as ElementIdDetails;

            // Deletes temporary element
            var deleteResult = await sut.DeleteElement(setId, elementDetails.ElementId);
            Assert.NotNull(deleteResult);
            Assert.NotNull(deleteResult.Value);
            Assert.IsType<string>(deleteResult.Value);
            Assert.Equal(HttpStatusCode.OK, deleteResult.Code);
            Assert.Equal(string.Format(elemenIdDeletedMessage, setId, elementDetails.ElementId), deleteResult.Value);
        }

        #endregion
    }
}
