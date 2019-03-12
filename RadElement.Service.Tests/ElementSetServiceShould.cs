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
            IntializeMockData();
        }

        #region GetElements

        [Fact]
        public async void GetSetssShouldReturnAllSets()
        {
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.GetSets();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<ElementSet>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetSet By SetId

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetSetByIdShouldReturnNotFoundIfDoesnotExists(int setId)
        {
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.GetSet(setId);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(setNotFoundMessage, setId), result.Value);
        }

        [Theory]
        [InlineData(53)]
        [InlineData(66)]
        public async void GetSetByIdShouldReturnSetsBasedOnSetId(int setId)
        {
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.GetSet(setId);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ElementSet>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region SearchSet

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async void SearchSetShouldReturnBadRequestIfSearchKeywordIsInvalid(string searchKeyword)
        {
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
        public async void GetSetShouldReturnSetIfSearchedElementExists(string searchKeyword)
        {
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.SearchSet(searchKeyword);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<ElementSet>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region CreateSet

        [Theory]
        [InlineData(null)]
        public async void CreateSetShouldReturnBadRequestIfSetIsInvalid(CreateUpdateSet set)
        {
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
        public async void CreateSetShouldReturnSetIdIfSetIsValid(string moduleName, string contactName, string description)
        {
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
            Assert.NotEqual(0, result.Value);
        }

        #endregion

        #region UpdateSet

        [Theory]
        [InlineData(53, null)]
        public async void UpdateSetShouldReturnBadRequestIfSetIsInvalid(int setId, CreateUpdateSet set)
        {
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.UpdateSet(setId, set);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(setInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData(1, "Tumuor1", "Tumuor2", "Tumuor3")]
        [InlineData(2, "Sinus1", "Sinus2", "Sinus3")]
        public async void UpdateSetShouldReturnBadRequestIfSetContentsAreInvalid(int setId, string moduleName, string contactName, string description)
        {
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
        [InlineData(53, "Tumuor1", "Tumuor2", "Tumuor3")]
        [InlineData(66, "Sinus1", "Sinus2", "Sinus3")]
        public async void UpdateSetShouldReturnSetIdIfSetIsValid(int setId, string moduleName, string contactName, string description)
        {
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
        [InlineData(100)]
        [InlineData(200)]
        public async void DeleteSetShouldReturnNotFoundIfSetIdDoesNotExists(int setId)
        {
            var sut = new ElementSetService(mockRadElementContext.Object, mockLogger.Object);
            var result = await sut.DeleteSet(setId);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(setNotFoundMessage, setId), result.Value);
        }

        [Theory]
        [InlineData(53)]
        [InlineData(66)]
        public async void DeleteSetShouldDeleteSetIfSetIdIsValid(int setId)
        {
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
