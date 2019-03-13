using Microsoft.EntityFrameworkCore;
using Moq;
using RadElement.Core.Data;
using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Service.Tests.Mocks.Data;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml;
using Xunit;

namespace RadElement.Service.Tests
{
    public class ModuleServiceShould
    {
        private readonly Mock<IRadElementDbContext> mockRadElementContext;
        private readonly Mock<ILogger> mockLogger;

        private const string xmlContetnInvalidMessage = "Xml content provided is invalid";
        private const string setIdNotFoundMessage = "No such set with id '{0}'";
        private const string setIdUpdatedMessage = "Set with id {0} is updated.";

        public ModuleServiceShould()
        {
            mockRadElementContext = new Mock<IRadElementDbContext>();
            mockLogger = new Mock<ILogger>();
            IntializeMockData();
        }

        #region CreateModule

        [Theory]
        [InlineData("//XMLFiles//Invalid.xml")]
        public async void CreateModuleReturnBadRequestIfXmlContentIsInvalid(string xmlPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + xmlPath);
            XmlElement root = doc.DocumentElement;
   
            var sut = new ModuleService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.CreateModule(root);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(xmlContetnInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("//XMLFiles//Valid.xml")]
        public async void CreateModuleReturnSetIDIfXmlContentIsValid(string xmlPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + xmlPath);
            XmlElement root = doc.DocumentElement;

            var sut = new ModuleService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.CreateModule(root);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<SetIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        #endregion

        #region UpdateModule

        [Theory]
        [InlineData(1, "//XMLFiles//Invalid.xml")]
        [InlineData(2, "//XMLFiles//Invalid.xml")]
        public async void UpdateModuleReturnBadRequestIfXmlContentIsInvalid(int setId, string xmlPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + xmlPath);
            XmlElement root = doc.DocumentElement;

            var sut = new ModuleService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.UpdateModule(root, setId);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(xmlContetnInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData(1, "//XMLFiles//Valid.xml")]
        [InlineData(2, "//XMLFiles//Valid.xml")]
        public async void UpdateModuleReturnBadRequestIfSetIdIsInvalid(int setId, string xmlPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + xmlPath);
            XmlElement root = doc.DocumentElement;

            var sut = new ModuleService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.UpdateModule(root, setId);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(setIdNotFoundMessage, setId), result.Value);
        }

        [Theory]
        [InlineData(53, "//XMLFiles//Valid.xml")]
        [InlineData(66, "//XMLFiles//Valid.xml")]
        public async void UpdateModuleReturnSetIdIfXmlContentAndSetIdIsValid(int setId, string xmlPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + xmlPath);
            XmlElement root = doc.DocumentElement;

            var sut = new ModuleService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.UpdateModule(root, setId);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(setIdUpdatedMessage, setId), result.Value);
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
            mockRadElementContext.Setup(x => x.SaveChanges()).Returns(1);
        }

        #endregion
    }
}
