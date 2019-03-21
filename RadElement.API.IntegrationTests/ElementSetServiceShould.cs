using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RadElement.Core.Data;
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
    public class ElementSetServiceShould
    {
        private IRadElementDbContext radElementContext;
        private IConfigurationManager configurationManager;
        private readonly ILogger logger;

        private const string setNotFoundMessage = "No such set with id '{0}'.";
        private const string setNotFoundMessageWithSearchMessage = "No such set with keyword '{0}'.";
        private const string setInvalidMessage = "Element set is invalid";
        private const string invalidSearchMessage = "Keyword '{0}' given is invalid";
        private const string setIdInvalidMessage = "No such element with set id {0}.";
        private const string setUpdatedMessage = "Set with id {0} is updated.";
        private const string setDeletedMessage = "Set with id {0} is deleted.";

        public ElementSetServiceShould()
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
        public async void GetSetssShouldThrowInternalServerErrorForExceptions()
        {
            SetInvalidAppSettingsConfig();
            var sut = new ElementSetService(radElementContext, logger);
            var result = await sut.GetSets();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Fact]
        public async void GetSetssShouldReturnAllSets()
        {
            var sut = new ElementSetService(radElementContext, logger);
            var result = await sut.GetSets();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<ElementSetDetails>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetSet By SetId

        [Theory]
        [InlineData("RD1")]
        [InlineData("RD2")]
        public async void GetSetByIdShouldReturnNotFoundIfDoesnotExists(string setId)
        {
            var sut = new ElementSetService(radElementContext, logger);
            var result = await sut.GetSet(setId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(setNotFoundMessage, setId), result.Value);
        }

        [Theory]
        [InlineData("RDES55")]
        [InlineData("RDES56")]
        public async void GetSetByIdShouldThrowInternalServerErrorForExceptions(string setId)
        {
            SetInvalidAppSettingsConfig();
            var sut = new ElementSetService(radElementContext, logger);
            var result = await sut.GetSet(setId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("RDES55")]
        [InlineData("RDES56")]
        public async void GetSetByIdShouldReturnSetsBasedOnSetId(string setId)
        {
            var sut = new ElementSetService(radElementContext, logger);
            var result = await sut.GetSet(setId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ElementSetDetails>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion
        
        #region SearchSet

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async void SearchSetShouldReturnBadRequestIfSearchKeywordIsInvalid(string searchKeyword)
        {
            var sut = new ElementSetService(radElementContext, logger);
            var result = await sut.SearchSet(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(string.Format(invalidSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("test1")]
        public async void SearchSetShouldReturnEmpySetIfSearchKeywordDoesnotExists(string searchKeyword)
        {
            var sut = new ElementSetService(radElementContext, logger);
            var result = await sut.SearchSet(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(setNotFoundMessageWithSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("Pulmonary")]
        [InlineData("Kimberly")]
        public async void GetSetShouldThrowInternalServerErrorForExceptions(string searchKeyword)
        {
            SetInvalidAppSettingsConfig();
            var sut = new ElementSetService(radElementContext, logger);
            var result = await sut.SearchSet(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("Pulmonary")]
        [InlineData("Kimberly")]
        public async void GetSetShouldReturnSetIfSearchedElementExists(string searchKeyword)
        {
            var sut = new ElementSetService(radElementContext, logger);
            var result = await sut.SearchSet(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<ElementSetDetails>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region CreateSet

        [Theory]
        [InlineData(null)]
        public async void CreateSetShouldReturnBadRequestIfSetIsInvalid(CreateUpdateSet set)
        {
            var sut = new ElementSetService(radElementContext, logger);
            var result = await sut.CreateSet(set);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(setInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("", "", "")]
        public async void CreateSetShouldReturnBadRequestIfSetContentsAreInvalid(string moduleName, string contactName, string description)
        {
            var set = new CreateUpdateSet();
            set.ModuleName = moduleName;
            set.ContactName = contactName;
            set.Description = description;

            var sut = new ElementSetService(radElementContext, logger);
            var result = await sut.CreateSet(set);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(setInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("Tumuor1", "Tumuor2", "Tumuor3")]
        [InlineData("Sinus1", "Sinus2", "Sinus3")]
        public async void CreateSetShouldThrowInternalServerErrorForExceptions(string moduleName, string contactName, string description)
        {
            SetInvalidAppSettingsConfig();
            var set = new CreateUpdateSet();
            set.ModuleName = moduleName;
            set.ContactName = contactName;
            set.Description = description;

            var sut = new ElementSetService(radElementContext, logger);
            var result = await sut.CreateSet(set);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("Tumuor1", "Tumuor2", "Tumuor3")]
        [InlineData("Sinus1", "Sinus2", "Sinus3")]
        public async void CreateSetShouldReturnSetIdIfSetIsValid(string moduleName, string contactName, string description)
        {
            // Creates temporary set
            var set = new CreateUpdateSet();
            set.ModuleName = moduleName;
            set.ContactName = contactName;
            set.Description = description;

            var sut = new ElementSetService(radElementContext, logger);
            var createdResult = await sut.CreateSet(set);

            Assert.NotNull(createdResult);
            Assert.NotNull(createdResult.Value);
            Assert.IsType<SetIdDetails>(createdResult.Value);
            Assert.Equal(HttpStatusCode.Created, createdResult.Code);

            var setDetails = createdResult.Value as SetIdDetails;

            // Deletes temporary set
            var deleteResult = await sut.DeleteSet(setDetails.SetId);

            Assert.Equal(HttpStatusCode.OK, deleteResult.Code);
            Assert.Equal(string.Format(setDeletedMessage, setDetails.SetId), deleteResult.Value);
        }

        #endregion

        #region UpdateSet

        [Theory]
        [InlineData("RDES53", null)]
        public async void UpdateSetShouldReturnBadRequestIfSetIsInvalid(string setId, CreateUpdateSet set)
        {
            var sut = new ElementSetService(radElementContext, logger);
            var result = await sut.UpdateSet(setId, set);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(setInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("RDESF1", "Tumuor1", "Tumuor2", "Tumuor3")]
        [InlineData("RDESF2", "Sinus1", "Sinus2", "Sinus3")]
        public async void UpdateSetShouldReturnBadRequestIfSetContentsAreInvalid(string setId, string moduleName, string contactName, string description)
        {
            var set = new CreateUpdateSet();
            set.ModuleName = moduleName;
            set.ContactName = contactName;
            set.Description = description;

            var sut = new ElementSetService(radElementContext, logger);
            var result = await sut.UpdateSet(setId, set);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(setNotFoundMessage, setId), result.Value);
        }

        [Theory]
        [InlineData("Tumuor1", "Tumuor2", "Tumuor3")]
        [InlineData("Sinus1", "Sinus2", "Sinus3")]
        public async void UpdateSetShouldThrowInternalServerErrorForExceptions(string moduleName, string contactName, string description)
        {
            SetInvalidAppSettingsConfig();
            var createset = new CreateUpdateSet();
            createset.ModuleName = moduleName;
            createset.ContactName = contactName;
            createset.Description = description;

            var sut = new ElementSetService(radElementContext, logger);
            var result = await sut.CreateSet(createset);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("Tumuor1", "Tumuor2", "Tumuor3")]
        [InlineData("Sinus1", "Sinus2", "Sinus3")]
        public async void UpdateSetShouldReturnSetIdIfSetIsValid(string moduleName, string contactName, string description)
        {
            // Creates temporary set
            var createset = new CreateUpdateSet();
            createset.ModuleName = moduleName;
            createset.ContactName = contactName;
            createset.Description = description;

            var sut = new ElementSetService(radElementContext, logger);
            var createdResult = await sut.CreateSet(createset);

            Assert.NotNull(createdResult);
            Assert.NotNull(createdResult.Value);
            Assert.IsType<SetIdDetails>(createdResult.Value);
            Assert.Equal(HttpStatusCode.Created, createdResult.Code);

            var setDetails = createdResult.Value as SetIdDetails;

            // Updates temporary set
            var updateSet = new CreateUpdateSet();
            updateSet.ModuleName = moduleName;
            updateSet.ContactName = contactName;
            updateSet.Description = description;

            var result = await sut.UpdateSet(setDetails.SetId, updateSet);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(setUpdatedMessage, setDetails.SetId), result.Value);

            // Removes the temporarily inserted module
            var deleteResult = await sut.DeleteSet(setDetails.SetId);

            Assert.Equal(HttpStatusCode.OK, deleteResult.Code);
            Assert.Equal(string.Format(setDeletedMessage, setDetails.SetId), deleteResult.Value);
        }

        #endregion

        #region DeleteElement

        [Theory]
        [InlineData("RDES100")]
        [InlineData("RDES200")]
        public async void DeleteSetShouldReturnNotFoundIfSetIdDoesNotExists(string setId)
        {
            var sut = new ElementSetService(radElementContext, logger);
            var result = await sut.DeleteSet(setId);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(setNotFoundMessage, setId), result.Value);
        }

        [Theory]
        [InlineData("Tumuor1", "Tumuor2", "Tumuor3")]
        [InlineData("Sinus1", "Sinus2", "Sinus3")]
        public async void DeleteSetShouldThrowInternalServerErrorForExceptions(string moduleName, string contactName, string description)
        {
            SetInvalidAppSettingsConfig();
            var createset = new CreateUpdateSet();
            createset.ModuleName = moduleName;
            createset.ContactName = contactName;
            createset.Description = description;

            var sut = new ElementSetService(radElementContext, logger);
            var result = await sut.CreateSet(createset);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("Tumuor1", "Tumuor2", "Tumuor3")]
        [InlineData("Sinus1", "Sinus2", "Sinus3")]
        public async void DeleteSetShouldDeleteSetIfSetIdIsValid(string moduleName, string contactName, string description)
        {
            // Creates temporary set
            var createset = new CreateUpdateSet();
            createset.ModuleName = moduleName;
            createset.ContactName = contactName;
            createset.Description = description;

            var sut = new ElementSetService(radElementContext, logger);
            var createdResult = await sut.CreateSet(createset);

            Assert.NotNull(createdResult);
            Assert.NotNull(createdResult.Value);
            Assert.IsType<SetIdDetails>(createdResult.Value);
            Assert.Equal(HttpStatusCode.Created, createdResult.Code);

            var setDetails = createdResult.Value as SetIdDetails;

            // Removes temporary set
            var result = await sut.DeleteSet(setDetails.SetId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(setDeletedMessage, setDetails.SetId), result.Value);
        }

        #endregion

        #region Private methods

        private void SetInvalidAppSettingsConfig()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings_invalid.json");
            IConfiguration configuration = configurationBuilder.Build();
            configurationManager = new ConfigurationManager(configuration);
            var optionsBuilder = new DbContextOptionsBuilder<RadElementDbContext>();
            radElementContext = new RadElementDbContext(optionsBuilder.Options, configurationManager);
        }

        #endregion
    }
}
