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
    /// Business service for handling the element set related operations
    /// </summary>
    /// <seealso cref="RadElement.Core.Services.IElementSetService" />
    public class ElementSetService : IElementSetService
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
        public ElementSetService(
            RadElementDbContext radElementDbContext,
            IMapper mapper,
            ILogger logger)
        {
            this.radElementDbContext = radElementDbContext;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the set.
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetSets()
        {
            try
            {
                var sets = radElementDbContext.ElementSet.ToList();
                return await Task.FromResult(new JsonResult(GetElementSetDetailsDto(sets), HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetSets()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Gets the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> GetSet(string setId)
        {
            try
            {
                if (IsValidSetId(setId))
                {
                    int id = Convert.ToInt32(setId.Remove(0, 4));
                    var set = radElementDbContext.ElementSet.Where(x => x.Id == id).FirstOrDefault();

                    if (set != null)
                    {
                        return await Task.FromResult(new JsonResult(GetElementSetDetailsDto(set), HttpStatusCode.OK));
                    }
                }
                return await Task.FromResult(new JsonResult(string.Format("No such set with id '{0}'.", setId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetSet(string setId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Searches the cde set.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<JsonResult> SearchSets(string searchKeyword)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    var sets = radElementDbContext.ElementSet.Where(x => ("RDES" + x.Id.ToString()).ToLower().Contains(searchKeyword.ToLower()) ||
                                                                                  x.Name.ToLower().Contains(searchKeyword.ToLower())).ToList();
                    if (sets != null && sets.Any())
                    {
                        return await Task.FromResult(new JsonResult(GetElementSetDetailsDto(sets), HttpStatusCode.OK));
                    }
                    else
                    {
                        return await Task.FromResult(new JsonResult(string.Format("No such set with keyword '{0}'.", searchKeyword), HttpStatusCode.NotFound));
                    }
                }

                return await Task.FromResult(new JsonResult("Keyword given is invalid.", HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'SearchSets(string searchKeyword)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Creates the cde set.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<JsonResult> CreateSet(CreateUpdateSet content)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (content == null)
                    {
                        return await Task.FromResult(new JsonResult("Set fileds are invalid.", HttpStatusCode.BadRequest));
                    }

                    ElementSet set = new ElementSet()
                    {
                        Name = content.Name.Trim(),
                        Description = content.Description,
                        ContactName = content.ContactName,
                        ParentId = content.ParentId,
                        Status = "Proposed",
                        StatusDate = DateTime.UtcNow,
                        Modality = content.Modality != null && content.Modality.Any() ? string.Join(",", content.Modality) : null,
                        BiologicalSex = content.BiologicalSex != null && content.BiologicalSex.Any() ? string.Join(",", content.BiologicalSex) : null,
                        AgeLowerBound = content.AgeLowerBound,
                        AgeUpperBound = content.AgeUpperBound,
                        Version = content.Version
                    };

                    radElementDbContext.ElementSet.Add(set);
                    radElementDbContext.SaveChanges();

                    AddPersonReferences(set.Id, content.Persons);
                    AddOrganizationReferences(set.Id, content.Organizations);

                    transaction.Commit();

                    return await Task.FromResult(new JsonResult(new SetIdDetails() { SetId = "RDES" + set.Id.ToString() }, HttpStatusCode.Created));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'CreateSet(CreateUpdateSet content)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<JsonResult> UpdateSet(string setId, CreateUpdateSet content)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (IsValidSetId(setId))
                    {
                        int id = Convert.ToInt32(setId.Remove(0, 4));

                        if (content == null)
                        {
                            return await Task.FromResult(new JsonResult("Set fileds are invalid.", HttpStatusCode.BadRequest));
                        }

                        var elementSet = radElementDbContext.ElementSet.Where(x => x.Id == id).FirstOrDefault();

                        if (elementSet != null)
                        {
                            elementSet.Name = content.Name.Trim();
                            elementSet.Description = content.Description;
                            elementSet.ContactName = content.ContactName;
                            elementSet.ParentId = content.ParentId;
                            elementSet.Modality = content.Modality != null && content.Modality.Any() ? string.Join(",", content.Modality) : null;
                            elementSet.BiologicalSex = content.BiologicalSex != null && content.BiologicalSex.Any() ? string.Join(",", content.BiologicalSex) : null;
                            elementSet.AgeLowerBound = content.AgeLowerBound;
                            elementSet.AgeUpperBound = content.AgeUpperBound;
                            elementSet.Version = content.Version;

                            radElementDbContext.SaveChanges();

                            RemovePersonReferences(id);
                            RemoveOrganizationReferences(id);

                            AddPersonReferences(id, content.Persons);
                            AddOrganizationReferences(id, content.Organizations);

                            transaction.Commit();

                            return await Task.FromResult(new JsonResult(string.Format("Set with id '{0}' is updated.", setId), HttpStatusCode.OK));
                        }
                    }

                    return await Task.FromResult(new JsonResult(string.Format("No such set with id '{0}'.", setId), HttpStatusCode.NotFound));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'UpdateSet(string setId, CreateUpdateSet content)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Deletes the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> DeleteSet(string setId)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (IsValidSetId(setId))
                    {
                        int id = Convert.ToInt32(setId.Remove(0, 4));
                        var elementSet = radElementDbContext.ElementSet.Where(x => x.Id == id).FirstOrDefault();

                        if (elementSet != null)
                        {
                            RemoveSetElementsReferences(elementSet);
                            RemovePersonReferences(elementSet.Id);
                            RemoveOrganizationReferences(elementSet.Id);

                            radElementDbContext.ElementSet.Remove(elementSet);
                            radElementDbContext.SaveChanges();
                            transaction.Commit();

                            return await Task.FromResult(new JsonResult(string.Format("Set with id '{0}' is deleted.", setId), HttpStatusCode.OK));
                        }
                    }
                    return await Task.FromResult(new JsonResult(string.Format("No such set with id '{0}'.", setId), HttpStatusCode.NotFound));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'DeleteSet(string setId)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
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
                return result;
            }

            return false;
        }

        /// <summary>
        /// Removes the set elements.
        /// </summary>
        /// <param name="elementSet">The element set.</param>
        private void RemoveSetElementsReferences(ElementSet elementSet)
        {
            var elementSetRefs = radElementDbContext.ElementSetRef.Where(x => x.ElementSetId == elementSet.Id).ToList();
            if (elementSetRefs != null && elementSetRefs.Any())
            {
                radElementDbContext.ElementSetRef.RemoveRange(elementSetRefs);
            }
        }

        /// <summary>
        /// Adds the person references.
        /// </summary>
        /// <param name="personIds">The person ids.</param>
        private void AddPersonReferences(int setId, List<PersonDetails> personRefs)
        {
            if (personRefs != null && personRefs.Any())
            {
                foreach (var personRef in personRefs)
                {
                    var person = radElementDbContext.Person.Where(x => x.Id == personRef.PersonId).FirstOrDefault();

                    if (person != null)
                    {
                        if (personRef.Roles != null && personRef.Roles.Any())
                        {
                            foreach (var role in personRef.Roles.Distinct())
                            {
                                var setRef = new PersonRoleElementSetRef()
                                {
                                    ElementSetID = setId,
                                    PersonID = personRef.PersonId,
                                    Role = role.ToString()
                                };

                                radElementDbContext.PersonRoleElementSetRef.Add(setRef);
                            }
                        }
                        else
                        {
                            var setRef = new PersonRoleElementSetRef()
                            {
                                ElementSetID = setId,
                                PersonID = personRef.PersonId
                            };

                            radElementDbContext.PersonRoleElementSetRef.Add(setRef);
                        }
                    }
                }
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Adds the organization references.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="orgRefs">The org refs.</param>
        private void AddOrganizationReferences(int setId, List<OrganizationDetails> orgRefs)
        {
            if (orgRefs != null && orgRefs.Any())
            {
                foreach (var orgRef in orgRefs)
                {
                    var organization = radElementDbContext.Organization.Where(x => x.Id == orgRef.OrganizationId).FirstOrDefault();

                    if (organization != null)
                    {
                        if (orgRef.Roles != null && orgRef.Roles.Any())
                        {
                            foreach (var role in orgRef.Roles.Distinct())
                            {
                                var setRef = new OrganizationRoleElementSetRef()
                                {
                                    ElementSetID = setId,
                                    OrganizationID = orgRef.OrganizationId,
                                    Role = role.ToString()
                                };

                                radElementDbContext.OrganizationRoleElementSetRef.Add(setRef);
                            }
                        }
                        else
                        {
                            var setRef = new OrganizationRoleElementSetRef()
                            {
                                ElementSetID = setId,
                                OrganizationID = orgRef.OrganizationId
                            };

                            radElementDbContext.OrganizationRoleElementSetRef.Add(setRef);
                        }
                    }
                }
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Removes the person references.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        private void RemovePersonReferences(int setId)
        {
            var personElementSetRefs = radElementDbContext.PersonRoleElementSetRef.Where(x => x.ElementSetID == setId).ToList();
            if (personElementSetRefs != null && personElementSetRefs.Any())
            {
                radElementDbContext.PersonRoleElementSetRef.RemoveRange(personElementSetRefs);
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Removes the organization references.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        private void RemoveOrganizationReferences(int setId)
        {
            var organizationElementSetRefs = radElementDbContext.OrganizationRoleElementSetRef.Where(x => x.ElementSetID == setId).ToList();
            if (organizationElementSetRefs != null && organizationElementSetRefs.Any())
            {
                radElementDbContext.OrganizationRoleElementSetRef.RemoveRange(organizationElementSetRefs);
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the element set details dto.
        /// </summary>
        /// <param name="set">The value.</param>
        /// <returns></returns>
        private object GetElementSetDetailsDto(object set)
        {
            if (set.GetType() == typeof(List<ElementSet>))
            {
                var sets = mapper.Map<List<ElementSet>, List<ElementSetDetails>>(set as List<ElementSet>);
                sets.ForEach(_set =>
                {
                    _set.OrganizationInformation = GetOrganizationDetails((_set as ElementSet).Id);
                    _set.PersonInformation = GetPersonDetails((_set as ElementSet).Id);
                });

                return sets;
            }
            else if (set.GetType() == typeof(ElementSet))
            {
                var setDetails = mapper.Map<ElementSetDetails>(set as ElementSet);
                setDetails.OrganizationInformation = GetOrganizationDetails((set as ElementSet).Id);
                setDetails.PersonInformation = GetPersonDetails((set as ElementSet).Id);

                return setDetails;
            }

            return null;
        }

        /// <summary>
        /// Gets the organization details.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        private List<OrganizationAttributes> GetOrganizationDetails(int setId)
        {
            List<OrganizationAttributes> organizationInfo = new List<OrganizationAttributes>();
            var organizationElementSetRefs = radElementDbContext.OrganizationRoleElementSetRef.Where(x => x.ElementSetID == setId).ToList();

            if (organizationElementSetRefs != null && organizationElementSetRefs.Any())
            {
                foreach (var organizationElementSetRef in organizationElementSetRefs)
                {
                    var organization = radElementDbContext.Organization.Where(x => x.Id == organizationElementSetRef.OrganizationID).FirstOrDefault();

                    if (organization != null)
                    {
                        if (!organizationInfo.Exists(x => x.Id == organization.Id))
                        {
                            var organizationDetails = mapper.Map<OrganizationAttributes>(organization);
                            if (!string.IsNullOrEmpty(organizationElementSetRef.Role))
                            {
                                organizationDetails.Roles.Add(organizationElementSetRef.Role);
                            }
                            organizationInfo.Add(organizationDetails);
                        }
                        else
                        {
                            var existingOrganization = organizationInfo.Find(x => x.Id == organization.Id);
                            if (!string.IsNullOrEmpty(organizationElementSetRef.Role))
                            {
                                existingOrganization.Roles.Add(organizationElementSetRef.Role);
                            }
                        }
                    }
                }
            }

            return organizationInfo;
        }

        /// <summary>
        /// Gets the person details.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        private List<PersonAttributes> GetPersonDetails(int setId)
        {
            List<PersonAttributes> personInfo = new List<PersonAttributes>();
            var personElementSetRefs = radElementDbContext.PersonRoleElementSetRef.Where(x => x.ElementSetID == setId).ToList();

            if (personElementSetRefs != null && personElementSetRefs.Any())
            {
                foreach (var personElementSetRef in personElementSetRefs)
                {
                    var person = radElementDbContext.Person.Where(x => x.Id == personElementSetRef.PersonID).FirstOrDefault();

                    if (person != null)
                    {
                        if (!personInfo.Exists(x => x.Id == person.Id))
                        {
                            var personDetails = mapper.Map<PersonAttributes>(person);
                            if (!string.IsNullOrEmpty(personElementSetRef.Role))
                            {
                                personDetails.Roles.Add(personElementSetRef.Role);
                            }
                            personInfo.Add(personDetails);
                        }
                        else
                        {
                            var existingPerson = personInfo.Find(x => x.Id == person.Id);
                            if (!string.IsNullOrEmpty(personElementSetRef.Role))
                            {
                                existingPerson.Roles.Add(personElementSetRef.Role);
                            }
                        }
                    }
                }
            }

            return personInfo;
        }
    }
}
