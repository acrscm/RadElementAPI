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

namespace RadElement.Service.Tests
{
    public class PersonServiceShould
    {
        /// <summary>
        /// The service
        /// </summary>
        private readonly PersonService service;

        /// <summary>
        /// The mock element service
        /// </summary>
        private readonly ElementService elementService;

        /// <summary>
        /// The mock element set service
        /// </summary>
        private readonly ElementSetService elementSetService;
        
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

        private const string personNotFoundMessage = "No such person with id '{0}'.";
        private const string personWithSetIdNotFoundMessage = "No such person with set id '{0}'.";
        private const string personWithElementIdNotFoundMessage = "No such person with element id '{0}'.";
        private const string elemenIdInvalidMessage = "No such element with element id '{0}'.";
        private const string setIdInvalidMessage = "No such set with set id '{0}'.";
        private const string personNotFoundMessageWithSearchMessage = "No such person with keyword '{0}'.";
        private const string invalidSearchMessage = "Keyword '{0}' given is invalid.";
        private const string personInvalidMessage = "Person fields are invalid.";
        private const string personExistsMessage = "Person with same details already exists.";
        private const string personUpdateMessage = "Person with id '{0}' is updated.";
        private const string personDeletedMessage = "Person with id '{0}' is deleted.";

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonServiceShould"/> class.
        /// </summary>
        public PersonServiceShould()
        {
            mockRadElementContext = new Mock<RadElementDbContext>();
            mockLogger = new Mock<ILogger>();

            var elementProfile = new ElementProfile();
            var elementSetProfile = new ElementSetProfile();
            var personProfile = new PersonProfile();
            var organizationProfile = new OrganizationProfile();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(elementProfile);
                cfg.AddProfile(elementSetProfile);
                cfg.AddProfile(personProfile);
                cfg.AddProfile(organizationProfile);
            });

            mapper = new Mapper(mapperConfig);
            elementService = new ElementService(mockRadElementContext.Object, mapper, mockLogger.Object);
            elementSetService = new ElementSetService(mockRadElementContext.Object, mapper, mockLogger.Object);
            service = new PersonService(mockRadElementContext.Object, elementSetService, elementService, mapper, mockLogger.Object);
        }

        #region GetPersons

        [Fact]
        public async void GetPersonsShouldThrowInternalServerErrorForExceptions()
        {
            var result = await service.GetPersons();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Fact]
        public async void GetPersonsShouldReturnAllPersons()
        {
            IntializeMockData();
            var result = await service.GetPersons();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<PersonDetails>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetPerson

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetPersonShouldThrowInternalServerErrorForExceptions(int personId)
        {
            var result = await service.GetPerson(personId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData(7)]
        [InlineData(8)]
        public async void GetPersonShouldReturnNotFoundIfDoesnotExists(int personId)
        {
            IntializeMockData();
            var result = await service.GetPerson(personId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(personNotFoundMessage, personId), result.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetPersonShouldReturnPersonBasedOnPersonId(int personId)
        {
            IntializeMockData();
            var result = await service.GetPerson(personId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<PersonDetails>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetPersonBySetId
        
        [Theory]
        [InlineData("RD1")]
        [InlineData("RD2")]
        public async void GetPersonBySetIdShouldReturnNotFoundIfDoesnotExists(string setId)
        {
            IntializeMockData();
            var result = await service.GetPersonBySetId(setId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(personWithSetIdNotFoundMessage, setId), result.Value);
        }

        [Theory]
        [InlineData("RDES53")]
        [InlineData("RDES66")]
        public async void GetPersonBySetIdShouldReturnPersonsBasedOnElementId(string setId)
        {
            IntializeMockData();
            var result = await service.GetPersonBySetId(setId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<PersonDetails>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetPersonByElementId

        [Theory]
        [InlineData("RD1")]
        [InlineData("RD2")]
        public async void GetPersonByElementIdShouldReturnNotFoundIfDoesnotExists(string elementId)
        {
            IntializeMockData();
            var result = await service.GetPersonByElementId(elementId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(personWithElementIdNotFoundMessage, elementId), result.Value);
        }

        [Theory]
        [InlineData("RDE338")]
        [InlineData("RDE340")]
        public async void GetPersonByElementIdShouldReturnPersonsBasedOnElementId(string elementId)
        {
            IntializeMockData();
            var result = await service.GetPersonByElementId(elementId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<PersonDetails>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region SearchPerson

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async void SearchPersonShouldReturnBadRequestIfSearchKeywordIsInvalid(string searchKeyword)
        {
            IntializeMockData();
            var result = await service.SearchPerson(new SearchKeyword { Keyword = searchKeyword });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(string.Format(invalidSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("test1")]
        public async void SearchPersonShouldReturnEmpyPersonsIfSearchKeywordDoesnotExists(string searchKeyword)
        {
            IntializeMockData();
            var result = await service.SearchPerson(new SearchKeyword { Keyword = searchKeyword });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(personNotFoundMessageWithSearchMessage, searchKeyword), result.Value);
        }

        [Theory]
        [InlineData("Tumuor")]
        public async void SearchPersonShouldReturnThrowInternalServerErrorForExceptions(string searchKeyword)
        {
            var result = await service.SearchPerson(new SearchKeyword { Keyword = searchKeyword });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData("Adam")]
        public async void SearchPersonShouldReturnPersonssIfSearchedPersonExists(string searchKeyword)
        {
            IntializeMockData();
            var result = await service.SearchPerson(new SearchKeyword { Keyword = searchKeyword });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<PersonDetails>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region CreatePerson

        [Theory]
        [InlineData(null)]
        public async void CreatePersonShouldReturnBadRequestIfPersonIsInvalid(CreateUpdatePerson person)
        {
            IntializeMockData();
            var result = await service.CreatePerson(person);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(personInvalidMessage, result.Value);
        }
        
        [Theory]
        [InlineData("RDES100")]
        public async void CreatePersonShouldReturnNotFoundIfSetIdIsInvalid(string setId)
        {
            IntializeMockData();
            var person = new CreateUpdatePerson();
            person.Name = "Tumuor";
            person.Orcid = "Orcidr";
            person.SetId = setId;

            var result = await service.CreatePerson(person);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(setIdInvalidMessage, setId), result.Value);
        }

        [Theory]
        [InlineData("RDE1500")]
        public async void CreatePersonShouldReturnNotFoundIfElementIdIsInvalid(string elementId)
        {
            IntializeMockData();
            var person = new CreateUpdatePerson();
            person.Name = "Tumuor";
            person.Orcid = "Orcidr";
            person.ElementId = elementId;

            var result = await service.CreatePerson(person);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(elemenIdInvalidMessage, elementId), result.Value);
        }

        [Theory]
        [InlineData("RDES74", "RDE338")]
        [InlineData("RDES72", "RDE340")]
        [InlineData("RDES66", "RDE307")]
        [InlineData("RDES53", "RDE283")]
        public async void CreatePersonShouldReturnBadRequestIfPersonExists(string setId, string elementId)
        {
            IntializeMockData();
            var person = new CreateUpdatePerson();
            person.Name = "Adam Flanders, MD, MS";
            person.SetId = setId;
            person.ElementId = elementId;

            var result = await service.CreatePerson(person);
            
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(personExistsMessage, result.Value);
        }

        [Theory]
        [InlineData("RDES74", "RDE338")]
        [InlineData("RDES72", "RDE340")]
        [InlineData("RDES66", "RDE307")]
        [InlineData("RDES53", "RDE283")]
        public async void CreatePersonShouldReturnElementIdIfPersonsAreValid(string setId, string elementId)
        {
            IntializeMockData();
            var person = new CreateUpdatePerson();
            person.Name = "Tumuor";
            person.Orcid = "Orcidr";
            person.SetId = setId;
            person.ElementId = elementId;
            
            var result = await service.CreatePerson(person);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<PersonIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        #endregion

        #region UpdatePerson

        [Theory]
        [InlineData(1, null)]
        [InlineData(2, null)]
        public async void UpdatePersonShouldReturnBadRequestIfPersonIsInvalid(int personId, CreateUpdatePerson person)
        {
            IntializeMockData();
            var result = await service.UpdatePerson(personId, person);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(personInvalidMessage, result.Value);
        }

        [Theory]
        [InlineData(1, "RDES100")]
        public async void UpdatePersonShouldReturnNotFoundIfSetIdIsInvalid(int personId, string setId)
        {
            IntializeMockData();
            var person = new CreateUpdatePerson();
            person.Name = "Tumuor";
            person.Orcid = "Orcidr";
            person.SetId = setId;

            var result = await service.UpdatePerson(personId, person);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(setIdInvalidMessage, setId), result.Value);
        }

        [Theory]
        [InlineData(1, "RDE1500")]
        public async void UpdatePersonShouldReturnNotFoundIfElementIdIsInvalid(int personId, string elementId)
        {
            IntializeMockData();
            var person = new CreateUpdatePerson();
            person.Name = "Tumuor";
            person.Orcid = "Orcidr";
            person.ElementId = elementId;

            var result = await service.UpdatePerson(personId, person);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(elemenIdInvalidMessage, elementId), result.Value);
        }

        [Theory]
        [InlineData(1, "RDES74", "RDE338")]
        [InlineData(2, "RDES72", "RDE340")]
        [InlineData(1, "RDES66", "RDE307")]
        [InlineData(2, "RDES53", "RDE283")]
        public async void UpdatePersonShouldReturnBadRequestIfPersonExists(int personId, string setId, string elementId)
        {
            IntializeMockData();
            var person = new CreateUpdatePerson();
            person.Name = "Woojin Kim, MD";
            person.SetId = setId;
            person.ElementId = elementId;

            var result = await service.UpdatePerson(personId, person);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Equal(personExistsMessage, result.Value);
        }

        [Theory]
        [InlineData(2)]
        public async void UpdatePersonShouldReturnPersonIdIfPersonsAreValid(int personId)
        {
            IntializeMockData();
            var person = new CreateUpdatePerson();
            person.Name = "Tumuor";
            person.Orcid = "Orcidr";

            var result = await service.UpdatePerson(personId, person);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(personUpdateMessage, personId), result.Value);
        }

        #endregion

        #region DeletePerson

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        public async void DeletePersonShouldReturnNotFoundIfSetIdAndElementIdIsInvalid(int personId)
        {
            IntializeMockData();

            var result = await service.DeletePerson(personId);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
            Assert.Equal(string.Format(personNotFoundMessage, personId), result.Value);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        public async void DeletePersonShouldThrowInternalServerErrorForExceptions(int personId)
        {
            var result = await service.DeletePerson(personId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async void DeletePersonShouldDeletePersonIfPersonIdIsValid(int personId)
        {
            IntializeMockData();
            var result = await service.DeletePerson(personId);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
            Assert.Equal(string.Format(personDeletedMessage, personId), result.Value);
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
            mockSet.Setup(d => d.Add(It.IsAny<ElementSet>())).Callback<ElementSet>((s) => MockDataContext.elementSetDb.ToList().Add(s));

            var mockElementSetRef = new Mock<DbSet<ElementSetRef>>();
            mockElementSetRef.As<IQueryable<ElementSetRef>>().Setup(m => m.Provider).Returns(MockDataContext.elementSetRefDb.Provider);
            mockElementSetRef.As<IQueryable<ElementSetRef>>().Setup(m => m.Expression).Returns(MockDataContext.elementSetRefDb.Expression);
            mockElementSetRef.As<IQueryable<ElementSetRef>>().Setup(m => m.ElementType).Returns(MockDataContext.elementSetRefDb.ElementType);
            mockElementSetRef.As<IQueryable<ElementSetRef>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.elementSetRefDb.GetEnumerator());
            mockElementSetRef.Setup(d => d.Add(It.IsAny<ElementSetRef>())).Callback<ElementSetRef>((s) => MockDataContext.elementSetRefDb.ToList().Add(s));

            var mockElementValue = new Mock<DbSet<ElementValue>>();
            mockElementValue.As<IQueryable<ElementValue>>().Setup(m => m.Provider).Returns(MockDataContext.elementValueDb.Provider);
            mockElement.As<IQueryable<ElementValue>>().Setup(m => m.Expression).Returns(MockDataContext.elementValueDb.Expression);
            mockElementValue.As<IQueryable<ElementValue>>().Setup(m => m.ElementType).Returns(MockDataContext.elementValueDb.ElementType);
            mockElementValue.As<IQueryable<ElementValue>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.elementValueDb.GetEnumerator());
            mockElementValue.Setup(d => d.Add(It.IsAny<ElementValue>())).Callback<ElementValue>((s) => MockDataContext.elementValueDb.ToList().Add(s));

            var mockPerson = new Mock<DbSet<Person>>();
            mockPerson.As<IQueryable<Person>>().Setup(m => m.Provider).Returns(MockDataContext.personDb.Provider);
            mockPerson.As<IQueryable<Person>>().Setup(m => m.Expression).Returns(MockDataContext.personDb.Expression);
            mockPerson.As<IQueryable<Person>>().Setup(m => m.ElementType).Returns(MockDataContext.personDb.ElementType);
            mockPerson.As<IQueryable<Person>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.personDb.GetEnumerator());
            mockPerson.Setup(d => d.Add(It.IsAny<Person>())).Callback<Person>((s) => MockDataContext.personDb.ToList().Add(s));
            
            var mockPersonElementRef = new Mock<DbSet<PersonRoleElementRef>>();
            mockPersonElementRef.As<IQueryable<PersonRoleElementRef>>().Setup(m => m.Provider).Returns(MockDataContext.personElementRefDb.Provider);
            mockPersonElementRef.As<IQueryable<PersonRoleElementRef>>().Setup(m => m.Expression).Returns(MockDataContext.personElementRefDb.Expression);
            mockPersonElementRef.As<IQueryable<PersonRoleElementRef>>().Setup(m => m.ElementType).Returns(MockDataContext.personElementRefDb.ElementType);
            mockPersonElementRef.As<IQueryable<PersonRoleElementRef>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.personElementRefDb.GetEnumerator());
            mockPersonElementRef.Setup(d => d.Add(It.IsAny<PersonRoleElementRef>())).Callback<PersonRoleElementRef>((s) => MockDataContext.personElementRefDb.ToList().Add(s));

            var mockPersonElementSetRef = new Mock<DbSet<PersonRoleElementSetRef>>();
            mockPersonElementSetRef.As<IQueryable<PersonRoleElementSetRef>>().Setup(m => m.Provider).Returns(MockDataContext.personElementSetRefDb.Provider);
            mockPersonElementSetRef.As<IQueryable<PersonRoleElementSetRef>>().Setup(m => m.Expression).Returns(MockDataContext.personElementSetRefDb.Expression);
            mockPersonElementSetRef.As<IQueryable<PersonRoleElementSetRef>>().Setup(m => m.ElementType).Returns(MockDataContext.personElementSetRefDb.ElementType);
            mockPersonElementSetRef.As<IQueryable<PersonRoleElementSetRef>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.personElementSetRefDb.GetEnumerator());
            mockPersonElementSetRef.Setup(d => d.Add(It.IsAny<PersonRoleElementSetRef>())).Callback<PersonRoleElementSetRef>((s) => MockDataContext.personElementSetRefDb.ToList().Add(s));

            mockRadElementContext.Setup(c => c.Element).Returns(mockElement.Object);
            mockRadElementContext.Setup(c => c.ElementSet).Returns(mockSet.Object);
            mockRadElementContext.Setup(c => c.ElementSetRef).Returns(mockElementSetRef.Object);
            mockRadElementContext.Setup(c => c.ElementValue).Returns(mockElementValue.Object);
            mockRadElementContext.Setup(c => c.Person).Returns(mockPerson.Object);
            mockRadElementContext.Setup(c => c.PersonRoleElementRef).Returns(mockPersonElementRef.Object);
            mockRadElementContext.Setup(c => c.PersonRoleElementSetRef).Returns(mockPersonElementSetRef.Object);
        }

        #endregion
    }
}
