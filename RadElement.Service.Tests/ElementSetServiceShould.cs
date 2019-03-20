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
using Xunit;

namespace RadElement.Service.Tests
{
    public class ElementSetServiceShould
    {
        private readonly Mock<IRadElementDbContext> mockRadElementContext;
        private readonly Mock<ILogger> mockLogger;

        private const string setNotFoundMessage = "No such set with id '{0}'.";
        private const string setNotFoundMessageWithSearchMessage = "No such set with keyword '{0}'.";
        private const string setInvalidMessage = "Element set is invalid";
        private const string invalidSearchMessage = "Keyword '{0}' given is invalid";
        private const string setIdInvalidMessage = "No such element with set id {0}.";
        private const string setUpdatedMessage = "Set with id {0} is updated.";
        private const string setDeletedMessage = "Set with id {0} is deleted.";

        public ElementSetServiceShould()
        {
            mockRadElementContext = new Mock<IRadElementDbContext>();
            mockLogger = new Mock<ILogger>();
        }

        #region GetElements

        [Fact]
        public async void GetSetsShouldThrowInternalServerErrorForExceptions()
        {
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.GetSets();
            
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ArgumentNullException>(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Fact]
        public async void GetSetssShouldReturnAllSets()
        {
            IntializeMockData();
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.GetSets();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<ElementSetDetails>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetSet By SetId

        [Theory]
        [InlineData("RDES53")]
        [InlineData("RDES66")]
        public async void GetSetByIdShouldThrowInternalServerErrorForExceptions(string setId)
        {
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.GetSet(setId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ArgumentNullException>(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("RD1")]
        [InlineData("RD2")]
        public async void GetSetByIdShouldReturnNotFoundIfDoesnotExists(string setId)
        {
            IntializeMockData();
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.GetSet(setId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(setNotFoundMessage, setId), result.Value);
        }

        [Theory]
        [InlineData("RDES53")]
        [InlineData("RDES66")]
        public async void GetSetByIdShouldReturnSetsBasedOnSetId(string setId)
        {
            IntializeMockData();
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
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
            IntializeMockData();
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
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
            IntializeMockData();
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
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
        public async void GetSetShouldReturnThrowInternalServerErrorForExceptions(string searchKeyword)
        {
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.SearchSet(searchKeyword);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ArgumentNullException>(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("Pulmonary")]
        [InlineData("Kimberly")]
        public async void GetSetShouldReturnSetIfSearchedElementExists(string searchKeyword)
        {
            IntializeMockData();
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
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
            IntializeMockData();
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
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
            IntializeMockData();
            var set = new CreateUpdateSet();
            set.ModuleName = moduleName;
            set.ContactName = contactName;
            set.Description = description;

            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
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
            var set = new CreateUpdateSet();
            set.ModuleName = moduleName;
            set.ContactName = contactName;
            set.Description = description;

            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.CreateSet(set);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<NullReferenceException>(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("Tumuor1", "Tumuor2", "Tumuor3")]
        [InlineData("Sinus1", "Sinus2", "Sinus3")]
        public async void CreateSetShouldReturnSetIdIfSetIsValid(string moduleName, string contactName, string description)
        {
            IntializeMockData();
            var set = new CreateUpdateSet();
            set.ModuleName = moduleName;
            set.ContactName = contactName;
            set.Description = description;

            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.CreateSet(set);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<SetIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        #endregion

        #region UpdateSet

        [Theory]
        [InlineData("RDES53", null)]
        public async void UpdateSetShouldReturnBadRequestIfSetIsInvalid(string setId, CreateUpdateSet set)
        {
            IntializeMockData();
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.UpdateSet(setId, set);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(setInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData("RDES1", "Tumuor1", "Tumuor2", "Tumuor3")]
        [InlineData("RDES2", "Sinus1", "Sinus2", "Sinus3")]
        public async void UpdateSetShouldReturnBadRequestIfSetContentsAreInvalid(string setId, string moduleName, string contactName, string description)
        {
            IntializeMockData();
            var set = new CreateUpdateSet();
            set.ModuleName = moduleName;
            set.ContactName = contactName;
            set.Description = description;

            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.UpdateSet(setId, set);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(setNotFoundMessage, setId), result.Value);
        }

        [Theory]
        [InlineData("RDES53", "Tumuor1", "Tumuor2", "Tumuor3")]
        [InlineData("RDES66", "Sinus1", "Sinus2", "Sinus3")]
        public async void UpdateSetShouldThrowInternalServerErrorForExceptions(string setId, string moduleName, string contactName, string description)
        {
            var set = new CreateUpdateSet();
            set.ModuleName = moduleName;
            set.ContactName = contactName;
            set.Description = description;

            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.UpdateSet(setId, set);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ArgumentNullException>(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("RDES53", "Tumuor1", "Tumuor2", "Tumuor3")]
        [InlineData("RDES66", "Sinus1", "Sinus2", "Sinus3")]
        public async void UpdateSetShouldReturnSetIdIfSetIsValid(string setId, string moduleName, string contactName, string description)
        {
            IntializeMockData();
            var set = new CreateUpdateSet();
            set.ModuleName = moduleName;
            set.ContactName = contactName;
            set.Description = description;

            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.UpdateSet(setId, set);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(setUpdatedMessage, setId), result.Value);
        }

        #endregion

        #region DeleteElement

        [Theory]
        [InlineData("RDES100")]
        [InlineData("RDES200")]
        public async void DeleteSetShouldReturnNotFoundIfSetIdDoesNotExists(string setId)
        {
            IntializeMockData();
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.DeleteSet(setId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(setNotFoundMessage, setId), result.Value);
        }

        [Theory]
        [InlineData("RDES53")]
        [InlineData("RDES66")]
        public async void DeleteSetShouldThrowInternalServerErrorForExceptions(string setId)
        {
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.DeleteSet(setId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ArgumentNullException>(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("RDES53")]
        [InlineData("RDES66")]
        public async void DeleteSetShouldDeleteSetIfSetIdIsValid(string setId)
        {
            IntializeMockData();
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.DeleteSet(setId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(setDeletedMessage, setId), result.Value);
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
