using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RadElement.Core.Data;
using RadElement.Core.DTO;
using RadElement.Core.Infrastructure;
using RadElement.Data;
using RadElement.Infrastructure;
using RadElement.Service;
using Serilog;
using System;
using System.Net;
using System.Xml;
using Xunit;

namespace RadElement.API.IntegrationTests
{
    public class ModuleServiceShould
    {
        private IRadElementDbContext radElementContext;
        private IConfigurationManager configurationManager;
        private readonly ILogger logger;

        private const string xmlContentInvalidMessage = "Xml content provided is invalid";
        private const string setIdNotFoundMessage = "No such set with id '{0}'";
        private const string setIdUpdatedMessage = "Set with id {0} is updated.";
        private const string setIdDeletedMessage = "Set with id {0} is deleted.";

        public ModuleServiceShould()
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

        #region CreateModule

        [Theory]
        [InlineData("//XMLFiles//Invalid.xml")]
        public async void CreateModuleReturnBadRequestIfXmlContentIsInvalid(string xmlPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + xmlPath);
            XmlElement root = doc.DocumentElement;

            var sut = new ModuleService(radElementContext, logger);
            var result = await sut.CreateModule(root);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(xmlContentInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("//XMLFiles//Valid.xml")]
        public async void CreateModuleShouldThrowInternalServerErrorForExceptions(string xmlPath)
        {
            SetInvalidAppSettingsConfig();
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + xmlPath);
            XmlElement root = doc.DocumentElement;

            var sut = new ModuleService(radElementContext, logger);
            var result = await sut.CreateModule(root);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("//XMLFiles//Valid.xml")]
        public async void CreateModuleReturnSetIDIfXmlContentIsValid(string xmlPath)
        {
            // Adds temporarily module
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + xmlPath);
            XmlElement root = doc.DocumentElement;

            var sut = new ModuleService(radElementContext, logger);
            var addResult = await sut.CreateModule(root);

            Assert.NotNull(addResult);
            Assert.NotNull(addResult.Value);
            Assert.IsType<SetIdDetails>(addResult.Value);
            Assert.Equal(HttpStatusCode.Created, addResult.Code);

            var setDetails = addResult.Value as SetIdDetails;

            // Removes the temporarily inserted module
            var setService = new ElementSetService(radElementContext, logger);
            var deleteResult = await setService.DeleteSet(setDetails.SetId);

            Assert.Equal(HttpStatusCode.OK, deleteResult.Code);
            Assert.Equal(string.Format(setIdDeletedMessage, setDetails.SetId), deleteResult.Value);
        }

        #endregion

        #region UpdateModule

        [Theory]
        [InlineData("RDES1", "//XMLFiles//Invalid.xml")]
        [InlineData("RDES2", "//XMLFiles//Invalid.xml")]
        public async void UpdateModuleReturnBadRequestIfXmlContentIsInvalid(string setId, string xmlPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + xmlPath);
            XmlElement root = doc.DocumentElement;

            var sut = new ModuleService(radElementContext, logger);
            var result = await sut.UpdateModule(root, setId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(xmlContentInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("RD1", "//XMLFiles//Valid.xml")]
        [InlineData("RD2", "//XMLFiles//Valid.xml")]
        public async void UpdateModuleReturnBadRequestIfSetIdIsInvalid(string setId, string xmlPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + xmlPath);
            XmlElement root = doc.DocumentElement;

            var sut = new ModuleService(radElementContext, logger);
            var result = await sut.UpdateModule(root, setId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(setIdNotFoundMessage, setId), result.Value);
        }

        [Theory]
        [InlineData("//XMLFiles//Valid.xml")]
        public async void UpdateModuleShouldThrowInternalServerErrorForExceptions(string xmlPath)
        {
            SetInvalidAppSettingsConfig();
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + xmlPath);
            XmlElement root = doc.DocumentElement;

            var sut = new ModuleService(radElementContext, logger);
            var result = await sut.CreateModule(root);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("//XMLFiles//Valid.xml")]
        public async void UpdateModuleReturnSetIdIfXmlContentAndSetIdIsValid(string xmlPath)
        {
            // Adds temporarily module
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + xmlPath);
            XmlElement root = doc.DocumentElement;

            var sut = new ModuleService(radElementContext, logger);
            var createdResult = await sut.CreateModule(root);

            Assert.NotNull(createdResult);
            Assert.NotNull(createdResult.Value);
            Assert.IsType<SetIdDetails>(createdResult.Value);
            Assert.Equal(HttpStatusCode.Created, createdResult.Code);

            var setDetails = createdResult.Value as SetIdDetails;

            // Updates temporarily module 

            var updatedResult = await sut.UpdateModule(root, setDetails.SetId);

            Assert.NotNull(updatedResult);
            Assert.NotNull(updatedResult.Value);
            Assert.IsType<string>(updatedResult.Value);
            Assert.Equal(HttpStatusCode.OK, updatedResult.Code);
            Assert.Equal(string.Format(setIdUpdatedMessage, setDetails.SetId), updatedResult.Value);

            // Removes the temporarily inserted module
            var setService = new ElementSetService(radElementContext, logger);
            var deleteResult = await setService.DeleteSet(setDetails.SetId);

            Assert.Equal(HttpStatusCode.OK, deleteResult.Code);
            Assert.Equal(string.Format(setIdDeletedMessage, setDetails.SetId), deleteResult.Value);
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
