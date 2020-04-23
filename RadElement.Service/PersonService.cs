using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Core.Services;
using Serilog;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementSetService" /> class.
        /// </summary>
        /// <param name="radElementDbContext">The RAD element database context.</param>
        /// <param name="logger">The logger.</param>
        public PersonService(
            RadElementDbContext radElementDbContext,
            ILogger logger)
        {
            this.radElementDbContext = radElementDbContext;
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
                return await Task.FromResult(new JsonResult(persons, HttpStatusCode.OK));
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
                        return await Task.FromResult(new JsonResult(person, HttpStatusCode.OK));
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
        /// Searches the person.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<JsonResult> SearchPersons(SearchKeyword searchKeyword)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchKeyword.Keyword))
                {
                    var persons = radElementDbContext.Person.ToList();
                    var filteredPersons = persons.Where(x => x.Name.ToLower().Contains(searchKeyword.Keyword.ToLower())).ToList();
                    if (filteredPersons != null && filteredPersons.Any())
                    {
                        return await Task.FromResult(new JsonResult(filteredPersons, HttpStatusCode.OK));
                    }
                    else
                    {
                        return await Task.FromResult(new JsonResult(string.Format("No such person with keyword '{0}'.", searchKeyword.Keyword), HttpStatusCode.NotFound));
                    }
                }

                return await Task.FromResult(new JsonResult("Keyword given is invalid.", HttpStatusCode.BadRequest));
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

                    var isMatchingPerson = radElementDbContext.Person.ToList().Where(x => string.Equals(x.Name, person.Name, StringComparison.OrdinalIgnoreCase) &&
                                                                                           string.Equals(x.Orcid, person.Orcid, StringComparison.OrdinalIgnoreCase) &&
                                                                                           string.Equals(x.Url, person.Url, StringComparison.OrdinalIgnoreCase) &&
                                                                                           string.Equals(x.TwitterHandle, person.TwitterHandle, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (isMatchingPerson != null)
                    {
                        var details = new PersonIdDetails() { PersonId = isMatchingPerson.Id.ToString(), Message = "Person already exists with the available information." };
                        return await Task.FromResult(new JsonResult(details, HttpStatusCode.Created));
                    }

                    var personData = new Person()
                    {
                        Name = person.Name,
                        Orcid = person.Orcid,
                        Url = person.Url,
                        TwitterHandle = person.TwitterHandle,
                    };

                    radElementDbContext.Person.Add(personData);
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

                    if (personId != 0)
                    {
                        var persons = radElementDbContext.Person.ToList();
                        var isMatchingPerson = persons.Exists(x => x.Id != personId &&
                                                                   string.Equals(x.Name, person.Name, StringComparison.OrdinalIgnoreCase) &&
                                                                   string.Equals(x.Orcid, person.Orcid, StringComparison.OrdinalIgnoreCase) &&
                                                                   string.Equals(x.Url, person.Url, StringComparison.OrdinalIgnoreCase) &&
                                                                   string.Equals(x.TwitterHandle, person.TwitterHandle, StringComparison.OrdinalIgnoreCase));

                        if (isMatchingPerson)
                        {
                            return await Task.FromResult(new JsonResult("Person already exists with the available information.", HttpStatusCode.BadRequest));
                        }

                        var personDetails = persons.Find(x => x.Id == personId);

                        if (personDetails != null)
                        {
                            personDetails.Name = person.Name;
                            personDetails.Orcid = person.Orcid;
                            personDetails.Url = person.Url;
                            personDetails.TwitterHandle = person.TwitterHandle;

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
        /// Removes the person element set references.
        /// </summary>
        /// <param name="person">The person.</param>
        private void RemovePersonElementSetReferences(Person person)
        {
            var personElementSetRefs = radElementDbContext.PersonRoleElementSetRef.ToList().Where(x => x.PersonID == person.Id);
            if (personElementSetRefs != null && personElementSetRefs.Any())
            {
                radElementDbContext.PersonRoleElementSetRef.RemoveRange(personElementSetRefs);
                radElementDbContext.SaveChanges();
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
                radElementDbContext.SaveChanges();
            }
        }
    }
}
