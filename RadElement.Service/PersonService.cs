using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Core.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using RadElement.Core.Data;

namespace RadElement.Service
{
    /// <summary>
    /// Business service for handling the person related operations
    /// </summary>
    /// <seealso cref="RadElement.Core.Services.IPersonService" />
    public class PersonService : IPersonService
    {
        /// <summary>
        /// The RAD element database context
        /// </summary>
        private RadElementDbContext radElementDbContext;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementSetService" /> class.
        /// </summary>
        /// <param name="radElementDbContext">The RAD element database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public PersonService(
            RadElementDbContext radElementDbContext,
            IMapper mapper,
            ILogger logger)
        {
            this.radElementDbContext = radElementDbContext;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the persons.
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetPersons()
        {
            try
            {
                var persons = radElementDbContext.Person.ToList();
                return await Task.FromResult(new JsonResult(GetPersonDetailsDto(persons), HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetPersons()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Gets the person.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> GetPerson(int personId)
        {
            try
            {
                if (personId != 0)
                {
                    var persons = radElementDbContext.Person.ToList();
                    var person = persons.Find(x => x.Id == personId);

                    if (person != null)
                    {
                        return await Task.FromResult(new JsonResult(GetPersonDetailsDto(person), HttpStatusCode.OK));
                    }
                }
                return await Task.FromResult(new JsonResult(string.Format("No such person with id '{0}'.", personId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetPerson(int personId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Gets the persons by set identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> GetPersonBySetId(string setId)
        {
            try
            {
                if (IsValidSetId(setId))
                {
                    int setInternalId = Convert.ToInt32(setId.Remove(0, 4));
                    var personSetRefs = radElementDbContext.PersonRoleElementSetRef.ToList();
                    var personIds = personSetRefs.Where(x => x.ElementSetID == setInternalId);
                    var persons = radElementDbContext.Person.ToList();

                    var selectedPersons = from personId in personIds
                                          join person in persons on personId.PersonID equals person.Id
                                          select person;

                    if (selectedPersons != null && selectedPersons.Any())
                    {
                        return await Task.FromResult(new JsonResult(GetPersonDetailsDto(selectedPersons.ToList()), HttpStatusCode.OK));
                    }
                }
                return await Task.FromResult(new JsonResult(string.Format("No such person with set id '{0}'.", setId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetPersonsBySetId(string setId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Gets the persons by element identifier.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> GetPersonByElementId(string elementId)
        {
            try
            {
                if (IsValidElementId(elementId))
                {
                    int elementInternalId = Convert.ToInt32(elementId.Remove(0, 3));
                    var personElementRefs = radElementDbContext.PersonRoleElementRef.ToList();
                    var personIds = personElementRefs.Where(x => x.ElementID == elementInternalId);
                    var persons = radElementDbContext.Person.ToList();

                    var selectedPersons = from personId in personIds
                                          join person in persons on personId.PersonID equals person.Id
                                          select person;

                    if (selectedPersons != null && selectedPersons.Any())
                    {
                        return await Task.FromResult(new JsonResult(GetPersonDetailsDto(selectedPersons.ToList()), HttpStatusCode.OK));
                    }
                }
                return await Task.FromResult(new JsonResult(string.Format("No such person with element id '{0}'.", elementId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetPersonsByElementId(string elementId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Searches the person.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<JsonResult> SearchPerson(SearchKeyword searchKeyword)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchKeyword.Keyword))
                {
                    var persons = radElementDbContext.Person.ToList();
                    var filteredPersons = persons.Where(x => x.Name.ToLower().Contains(searchKeyword.Keyword.ToLower())).ToList();
                    if (filteredPersons != null && filteredPersons.Any())
                    {
                        return await Task.FromResult(new JsonResult(GetPersonDetailsDto(filteredPersons), HttpStatusCode.OK));
                    }
                    else
                    {
                        return await Task.FromResult(new JsonResult(string.Format("No such person with keyword '{0}'.", searchKeyword.Keyword), HttpStatusCode.NotFound));
                    }
                }

                return await Task.FromResult(new JsonResult(string.Format("Keyword '{0}' given is invalid.", searchKeyword.Keyword), HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'SearchPersons(SearchKeyword searchKeyword)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Creates the person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<JsonResult> CreatePerson(CreateUpdatePerson person)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (person == null)
                    {
                        return await Task.FromResult(new JsonResult("Person fields are invalid.", HttpStatusCode.BadRequest));
                    }

                    if (!string.IsNullOrEmpty(person.SetId))
                    {
                        if (!IsValidSetId(person.SetId))
                        {
                            return await Task.FromResult(new JsonResult(string.Format("No such set with set id '{0}'.", person.SetId), HttpStatusCode.NotFound));
                        }
                    }

                    if (!string.IsNullOrEmpty(person.ElementId))
                    {
                        if (!IsValidElementId(person.ElementId))
                        {
                            return await Task.FromResult(new JsonResult(string.Format("No such element with element id '{0}'.", person.ElementId), HttpStatusCode.NotFound));
                        }
                    }

                    var isMatchingPerson = radElementDbContext.Person.ToList().Exists(x => string.Equals(x.Name, person.Name, StringComparison.OrdinalIgnoreCase) &&
                                                                                           string.Equals(x.Orcid, person.Orcid, StringComparison.OrdinalIgnoreCase) &&
                                                                                           string.Equals(x.Url, person.Url, StringComparison.OrdinalIgnoreCase) &&
                                                                                           string.Equals(x.TwitterHandle, person.TwitterHandle, StringComparison.OrdinalIgnoreCase));
                    if (isMatchingPerson)
                    {
                        return await Task.FromResult(new JsonResult("Person with same details already exists.", HttpStatusCode.BadRequest));
                    }


                    int setId = !string.IsNullOrEmpty(person.SetId) ? Convert.ToInt32(person.SetId.Remove(0, 4)) : 0;
                    int elementId = !string.IsNullOrEmpty(person.ElementId) ? Convert.ToInt32(person.ElementId.Remove(0, 4)) : 0;

                    var personData = new Person()
                    {
                        Name = person.Name,
                        Orcid = person.Orcid,
                        Url = person.Url,
                        TwitterHandle = person.TwitterHandle,
                    };

                    radElementDbContext.Person.Add(personData);
                    radElementDbContext.SaveChanges();

                    if (elementId != 0)
                    {
                        AddPersonElementReferences(elementId, personData.Id, person.Roles);
                    }
                    if (setId != 0)
                    {
                        AddPersonElementSetReferences(setId, personData.Id, person.Roles);
                    }

                    radElementDbContext.SaveChanges();
                    transaction.Commit();

                    return await Task.FromResult(new JsonResult(new PersonIdDetails() { PersonId = personData.Id.ToString() }, HttpStatusCode.Created));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'CreatePerson(CreateUpdatePerson person)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Updates the person.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        public async Task<JsonResult> UpdatePerson(int personId, CreateUpdatePerson person)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (person == null)
                    {
                        return await Task.FromResult(new JsonResult("Person fields are invalid.", HttpStatusCode.BadRequest));
                    }

                    if (!string.IsNullOrEmpty(person.SetId))
                    {
                        if (!IsValidSetId(person.SetId))
                        {
                            return await Task.FromResult(new JsonResult(string.Format("No such set with set id '{0}'.", person.SetId), HttpStatusCode.NotFound));
                        }
                    }

                    if (!string.IsNullOrEmpty(person.ElementId))
                    {
                        if (!IsValidElementId(person.ElementId))
                        {
                            return await Task.FromResult(new JsonResult(string.Format("No such element with element id '{0}'.", person.ElementId), HttpStatusCode.NotFound));
                        }
                    }

                    if (personId != 0)
                    {
                        var persons = radElementDbContext.Person.ToList();
                        var isMatchingPerson = persons.Exists(x => string.Equals(x.Name, person.Name, StringComparison.OrdinalIgnoreCase) &&
                                                                   string.Equals(x.Orcid, person.Orcid, StringComparison.OrdinalIgnoreCase) &&
                                                                   string.Equals(x.Url, person.Url, StringComparison.OrdinalIgnoreCase) &&
                                                                   string.Equals(x.TwitterHandle, person.TwitterHandle, StringComparison.OrdinalIgnoreCase));

                        if (isMatchingPerson)
                        {
                            return await Task.FromResult(new JsonResult("Person with same details already exists.", HttpStatusCode.BadRequest));
                        }

                        var personDetails = persons.Find(x => x.Id == personId);

                        if (personDetails != null)
                        {
                            int setId = !string.IsNullOrEmpty(person.SetId) ? Convert.ToInt32(person.SetId.Remove(0, 4)) : 0;
                            int elementId = !string.IsNullOrEmpty(person.ElementId) ? Convert.ToInt32(person.ElementId.Remove(0, 4)) : 0;

                            personDetails.Name = person.Name;
                            personDetails.Orcid = person.Orcid;
                            personDetails.Url = person.Url;
                            personDetails.TwitterHandle = person.TwitterHandle;

                            radElementDbContext.SaveChanges();

                            RemovePersonElementReferences(personDetails);
                            RemovePersonElementReferences(personDetails);

                            if (elementId != 0)
                            {
                                AddPersonElementReferences(elementId, personDetails.Id, person.Roles);
                            }
                            if (setId != 0)
                            {
                                AddPersonElementSetReferences(setId, personDetails.Id, person.Roles);
                            }

                            radElementDbContext.SaveChanges();
                            transaction.Commit();

                            return await Task.FromResult(new JsonResult(string.Format("Person with id '{0}' is updated.", personId), HttpStatusCode.OK));
                        }
                    }

                    return await Task.FromResult(new JsonResult(string.Format("No such person with id '{0}'.", personId), HttpStatusCode.NotFound));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method ' UpdatePerson(int personId, CreateUpdatePerson person)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Deletes the person.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> DeletePerson(int personId)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (personId != 0)
                    {
                        var persons = radElementDbContext.Person.ToList();
                        var person = persons.Find(x => x.Id == personId);

                        if (person != null)
                        {
                            RemovePersonElementReferences(person);
                            RemovePersonElementSetReferences(person);

                            radElementDbContext.Person.Remove(person);
                            radElementDbContext.SaveChanges();
                            transaction.Commit();

                            return await Task.FromResult(new JsonResult(string.Format("Person with id '{0}' is deleted.", personId), HttpStatusCode.OK));
                        }
                    }
                    return await Task.FromResult(new JsonResult(string.Format("No such person with id '{0}'.", personId), HttpStatusCode.NotFound));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'DeletePerson(int personId)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Determines whether [is valid element identifier] [the specified element identifier].
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is valid element identifier] [the specified element identifier]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidElementId(string elementId)
        {
            if (elementId.Length > 3 && string.Equals(elementId.Substring(0, 3), "RDE", StringComparison.OrdinalIgnoreCase))
            {
                bool result = int.TryParse(elementId.Remove(0, 3), out _);
                if (result)
                {
                    int elementInternalId = Convert.ToInt32(elementId.Remove(0, 3));
                    var elements = radElementDbContext.Element.ToList();
                    var element = elements.Find(x => x.Id == elementInternalId);
                    return element != null;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether [is valid set identifier] [the specified set identifier].
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is valid set identifier] [the specified set identifier]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidSetId(string setId)
        {
            if (setId.Length > 4 && string.Equals(setId.Substring(0, 4), "RDES", StringComparison.OrdinalIgnoreCase))
            {
                bool result = int.TryParse(setId.Remove(0, 4), out _);
                if (result)
                {
                    int id = Convert.ToInt32(setId.Remove(0, 4));
                    var sets = radElementDbContext.ElementSet.ToList();
                    var set = sets.Find(x => x.Id == id);
                    return set != null;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds the person element set references.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="personId">The person identifier.</param>
        /// <param name="roles">The roles.</param>
        private void AddPersonElementSetReferences(int setId, int personId, List<PersonRole> roles)
        {
            if (roles != null && roles.Any())
            {
                foreach (var role in roles)
                {
                    var setRef = new PersonRoleElementSetRef()
                    {
                        ElementSetID = setId,
                        PersonID = personId,
                        Role = role
                    };

                    radElementDbContext.PersonRoleElementSetRef.Add(setRef);
                }
            } 
            else
            {
                var setRef = new PersonRoleElementSetRef()
                {
                    ElementSetID = setId,
                    PersonID = personId
                };

                radElementDbContext.PersonRoleElementSetRef.Add(setRef);
            }
        }

        /// <summary>
        /// Removes the person elements references.
        /// </summary>
        /// <param name="person">The person.</param>
        private void AddPersonElementReferences(int elementId, int personId, List<PersonRole> roles)
        {
            if (roles != null && roles.Any())
            {
                foreach (var role in roles)
                {
                    var elementRef = new PersonRoleElementRef()
                    {
                        ElementID = elementId,
                        PersonID = personId,
                        Role = role
                    };

                    radElementDbContext.PersonRoleElementRef.Add(elementRef);
                }
            }
            else
            {
                var elementRef = new PersonRoleElementRef()
                {
                    ElementID = elementId,
                    PersonID = personId
                };

                radElementDbContext.PersonRoleElementRef.Add(elementRef);
            }
        }

        /// <summary>
        /// Removes the person element set references.
        /// </summary>
        /// <param name="person">The person.</param>
        private void RemovePersonElementSetReferences(Person person)
        {
            var personElementSetRefs = radElementDbContext.PersonRoleElementSetRef.ToList().Where(x => x.PersonID == person.Id);
            if (personElementSetRefs != null && personElementSetRefs.Any())
            {
                radElementDbContext.PersonRoleElementSetRef.RemoveRange(personElementSetRefs);
            }
        }

        /// <summary>
        /// Removes the person elements references.
        /// </summary>
        /// <param name="person">The person.</param>
        private void RemovePersonElementReferences(Person person)
        {
            var personElementsRefs = radElementDbContext.PersonRoleElementRef.ToList().Where(x => x.PersonID == person.Id);
            if (personElementsRefs != null && personElementsRefs.Any())
            {
                radElementDbContext.PersonRoleElementRef.RemoveRange(personElementsRefs);
            }
        }

        /// <summary>
        /// Gets the element set details dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private object GetPersonDetailsDto(object value)
        {
            if (value.GetType() == typeof(List<Person>))
            {
                return mapper.Map<List<Person>, List<PersonDetails>>(value as List<Person>);
            }
            else if (value.GetType() == typeof(Person))
            {
                return mapper.Map<PersonDetails>(value as Person);
            }

            return null;
        }
    }
}
