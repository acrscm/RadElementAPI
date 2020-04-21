using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Core.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using System.Net;
using RadElement.Core.Data;

namespace RadElement.Service
{
    /// <summary>
    /// Business service for handling the organization related operations
    /// </summary>
    /// <seealso cref="RadElement.Core.Services.IOrganizationService" />
    public class OrganizationService : IOrganizationService
    {
        /// <summary>
        /// The RAD element database context
        /// </summary>
        private RadElementDbContext radElementDbContext;

        /// <summary>
        /// The element set service
        /// </summary>
        private IElementSetService elementSetService;

        /// <summary>
        /// The element service
        /// </summary>
        private IElementService elementService;

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
        /// <param name="elementSetService">The element set service.</param>
        /// <param name="elementService">The element service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public OrganizationService(
            RadElementDbContext radElementDbContext,
            IElementSetService elementSetService,
            IElementService elementService,
            IMapper mapper,
            ILogger logger)
        {
            this.radElementDbContext = radElementDbContext;
            this.elementSetService = elementSetService;
            this.elementService = elementService;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the organization.
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetOrganizations()
        {
            try
            {
                var organizations = radElementDbContext.Organization.ToList();
                return await Task.FromResult(new JsonResult(GetOrganizationDetailsDto(organizations), HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetOrganizations()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Gets the organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> GetOrganization(int organizationId)
        {
            try
            {
                if (organizationId != 0)
                {
                    var organizations = radElementDbContext.Organization.ToList();
                    var organization = organizations.Find(x => x.Id == organizationId);

                    if (organization != null)
                    {
                        return await Task.FromResult(new JsonResult(GetOrganizationDetailsDto(organization), HttpStatusCode.OK));
                    }
                }
                return await Task.FromResult(new JsonResult(string.Format("No such organization with id '{0}'.", organizationId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetOrganization(int organizationId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Gets the organization by set identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> GetOrganizationBySetId(string setId)
        {
            try
            {
                if (await IsValidSetId(setId))
                {
                    int setInternalId = Convert.ToInt32(setId.Remove(0, 4));
                    var organizationSetRefs = radElementDbContext.OrganizationRoleElementSetRef.ToList();
                    var organizationIds = organizationSetRefs.Where(x => x.ElementSetID == setInternalId);
                    var organizations = radElementDbContext.Organization.ToList();

                    var selectedOrganizations = from organizationId in organizationIds
                                                join organization in organizations on organizationId.OrganizationID equals (int)organization.Id
                                                select organization;

                    if (selectedOrganizations != null && selectedOrganizations.Any())
                    {
                        return await Task.FromResult(new JsonResult(GetOrganizationDetailsDto(selectedOrganizations.ToList()), HttpStatusCode.OK));
                    }
                }
                return await Task.FromResult(new JsonResult(string.Format("No such organization with set id '{0}'.", setId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetOrganizationsBySetId(string setId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Gets the organization by element identifier.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> GetOrganizationByElementId(string elementId)
        {
            try
            {
                if (await IsValidElementId(elementId))
                {
                    int elementInternalId = Convert.ToInt32(elementId.Remove(0, 3));
                    var organizationELementRefs = radElementDbContext.OrganizationRoleElementRef.ToList();
                    var organizationIds = organizationELementRefs.Where(x => x.ElementID == elementInternalId);
                    var organizations = radElementDbContext.Organization.ToList();

                    var selectedOrganizations = from organizationId in organizationIds
                                                join organization in organizations on organizationId.OrganizationID equals (int)organization.Id
                                                select organization;

                    if (selectedOrganizations != null && selectedOrganizations.Any())
                    {
                        return await Task.FromResult(new JsonResult(GetOrganizationDetailsDto(selectedOrganizations.ToList()), HttpStatusCode.OK));
                    }
                }
                return await Task.FromResult(new JsonResult(string.Format("No such organization with element id '{0}'.", elementId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetOrganizationsByElementId(string elementId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Searches the organization.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<JsonResult> SearchOrganization(SearchKeyword searchKeyword)
        {

            try
            {
                if (!string.IsNullOrEmpty(searchKeyword.Keyword))
                {
                    var organizations = radElementDbContext.Organization.ToList();
                    var filteredorganizations = organizations.Where(x => x.Name.ToLower().Contains(searchKeyword.Keyword.ToLower())).ToList();
                    if (filteredorganizations != null && filteredorganizations.Any())
                    {
                        return await Task.FromResult(new JsonResult(GetOrganizationDetailsDto(filteredorganizations), HttpStatusCode.OK));
                    }
                    else
                    {
                        return await Task.FromResult(new JsonResult(string.Format("No such organization with keyword '{0}'.", searchKeyword.Keyword), HttpStatusCode.NotFound));
                    }
                }

                return await Task.FromResult(new JsonResult(string.Format("Keyword '{0}' given is invalid.", searchKeyword.Keyword), HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'SearchOrganizations(SearchKeyword searchKeyword)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Creates the organization.
        /// </summary>
        /// <param name="organization">The organization.</param>
        /// <returns></returns>
        public async Task<JsonResult> CreateOrganization(CreateUpdateOrganization organization)
        {
            try
            {
                if (organization == null)
                {
                    return await Task.FromResult(new JsonResult("Organization fields are invalid.", HttpStatusCode.BadRequest));
                }

                if (!string.IsNullOrEmpty(organization.SetId))
                {
                    if (!await IsValidSetId(organization.SetId))
                    {
                        return await Task.FromResult(new JsonResult(string.Format("No such set with set id '{0}'.", organization.SetId), HttpStatusCode.NotFound));
                    }
                }

                if (!string.IsNullOrEmpty(organization.ElementId))
                {
                    if (!await IsValidElementId(organization.ElementId))
                    {
                        return await Task.FromResult(new JsonResult(string.Format("No such element with element id '{0}'.", organization.ElementId), HttpStatusCode.NotFound));
                    }
                }

                var isMatchingOrganization = radElementDbContext.Organization.ToList().Exists(x => string.Equals(x.Name, organization.Name, StringComparison.OrdinalIgnoreCase) &&
                                                                                                   string.Equals(x.Abbreviation, organization.Abbreviation, StringComparison.OrdinalIgnoreCase) &&
                                                                                                   string.Equals(x.Url, organization.Url, StringComparison.OrdinalIgnoreCase) &&
                                                                                                   string.Equals(x.Comment, organization.Comment, StringComparison.OrdinalIgnoreCase) &&
                                                                                                   string.Equals(x.Email, organization.Email, StringComparison.OrdinalIgnoreCase) &&
                                                                                                   string.Equals(x.TwitterHandle, organization.TwitterHandle, StringComparison.OrdinalIgnoreCase));
                if (isMatchingOrganization)
                {
                    return await Task.FromResult(new JsonResult("Organization with same details already exists.", HttpStatusCode.BadRequest));
                }

                int setId = !string.IsNullOrEmpty(organization.SetId) ? Convert.ToInt32(organization.SetId.Remove(0, 4)) : 0;
                int elementId = !string.IsNullOrEmpty(organization.ElementId) ? Convert.ToInt32(organization.ElementId.Remove(0, 4)) : 0;

                var organizationData = new Organization()
                {
                    Name = organization.Name,
                    Abbreviation = organization.Abbreviation,
                    Url = organization.Url,
                    Comment = organization.Comment,
                    Email = organization.Email,
                    TwitterHandle = organization.TwitterHandle,
                };

                radElementDbContext.Organization.Add(organizationData);
                radElementDbContext.SaveChanges();

                if (elementId != 0)
                {
                    AddOrganizationElementReferences(elementId, organizationData.Id, organization.Roles);
                }
                if (setId != 0)
                {
                    AddOrganizationElementSetReferences(setId, organizationData.Id, organization.Roles);
                }

                radElementDbContext.SaveChanges();

                return await Task.FromResult(new JsonResult(new OrganizationIdDetails() { OrganizationId = organizationData.Id.ToString() }, HttpStatusCode.Created));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'CreateOrganization(CreateUpdateOrganization organization)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Updates the organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="organization">The organization.</param>
        /// <returns></returns>
        public async Task<JsonResult> UpdateOrganization(int organizationId, CreateUpdateOrganization organization)
        {
            try
            {
                if (organization == null)
                {
                    return await Task.FromResult(new JsonResult("Organization fields are invalid.", HttpStatusCode.BadRequest));
                }

                if (!string.IsNullOrEmpty(organization.SetId))
                {
                    if (!await IsValidSetId(organization.SetId))
                    {
                        return await Task.FromResult(new JsonResult(string.Format("No such set with set id '{0}'.", organization.SetId), HttpStatusCode.NotFound));
                    }
                }

                if (!string.IsNullOrEmpty(organization.ElementId))
                {
                    if (!await IsValidElementId(organization.ElementId))
                    {
                        return await Task.FromResult(new JsonResult(string.Format("No such element with element id '{0}'.", organization.ElementId), HttpStatusCode.NotFound));
                    }
                }
                if (organizationId != 0)
                {
                    var organizations = radElementDbContext.Organization.ToList();
                    var isMatchingOrganization = organizations.Exists(x => string.Equals(x.Name, organization.Name, StringComparison.OrdinalIgnoreCase) &&
                                                                                                   string.Equals(x.Abbreviation, organization.Abbreviation, StringComparison.OrdinalIgnoreCase) &&
                                                                                                   string.Equals(x.Url, organization.Url, StringComparison.OrdinalIgnoreCase) &&
                                                                                                   string.Equals(x.Comment, organization.Comment, StringComparison.OrdinalIgnoreCase) &&
                                                                                                   string.Equals(x.Email, organization.Email, StringComparison.OrdinalIgnoreCase) &&
                                                                                                   string.Equals(x.TwitterHandle, organization.TwitterHandle, StringComparison.OrdinalIgnoreCase));
                    if (isMatchingOrganization)
                    {
                        return await Task.FromResult(new JsonResult("Organization with same details already exists.", HttpStatusCode.BadRequest));
                    }

                    var organizationDetails = organizations.Find(x => x.Id == organizationId);

                    if (organizationDetails != null)
                    {
                        int setId = !string.IsNullOrEmpty(organization.SetId) ? Convert.ToInt32(organization.SetId.Remove(0, 4)) : 0;
                        int elementId = !string.IsNullOrEmpty(organization.ElementId) ? Convert.ToInt32(organization.ElementId.Remove(0, 4)) : 0;

                        organizationDetails.Name = organization.Name;
                        organizationDetails.Abbreviation = organization.Abbreviation;
                        organizationDetails.Url = organization.Url;
                        organizationDetails.Comment = organization.Comment;
                        organizationDetails.Email = organization.Email;
                        organizationDetails.TwitterHandle = organization.TwitterHandle;

                        radElementDbContext.SaveChanges();

                        RemoveOrganizationElementReferences(organizationDetails);
                        RemoveOrganizationElementReferences(organizationDetails);

                        if (elementId != 0)
                        {
                            AddOrganizationElementReferences(elementId, organizationDetails.Id, organization.Roles);
                        }
                        if (setId != 0)
                        {
                            AddOrganizationElementSetReferences(setId, organizationDetails.Id, organization.Roles);
                        }

                        radElementDbContext.SaveChanges();

                        return await Task.FromResult(new JsonResult(string.Format("Organization with id '{0}' is updated.", organizationId), HttpStatusCode.OK));
                    }
                }

                return await Task.FromResult(new JsonResult(string.Format("No such organization with id '{0}'.", organizationId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'UpdateOrganization(int organizationId, CreateUpdateOrganization organization)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Deletes the organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> DeleteOrganization(int organizationId)
        {
            try
            {
                if (organizationId != 0)
                {
                    var organizations = radElementDbContext.Organization.ToList();
                    var organization = organizations.Find(x => x.Id == organizationId);

                    if (organization != null)
                    {
                        RemoveOrganizationElementReferences(organization);
                        RemoveOrganizationElementSetReferences(organization);

                        radElementDbContext.Organization.Remove(organization);
                        radElementDbContext.SaveChanges();

                        return await Task.FromResult(new JsonResult(string.Format("Organization with id '{0}' is deleted.", organizationId), HttpStatusCode.OK));
                    }
                }
                return await Task.FromResult(new JsonResult(string.Format("No such organization with id '{0}'.", organizationId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'DeleteOrganization(int organizationId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Determines whether [is valid element identifier] [the specified element identifier].
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        private async Task<bool> IsValidElementId(string elementId)
        {
            var element = await elementService.GetElement(elementId);
            return element != null && element.Code == HttpStatusCode.OK ? true : false;
        }

        /// <summary>
        /// Determines whether [is valid set identifier] [the specified set identifier].
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        private async Task<bool> IsValidSetId(string setId)
        {
            var set = await elementSetService.GetSet(setId);
            return set != null && set.Code == HttpStatusCode.OK ? true : false;
        }

        /// <summary>
        /// Adds the organization element set references.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="roles">The roles.</param>
        private void AddOrganizationElementSetReferences(int setId, int organizationId, List<OrganizationRole> roles)
        {
            if (roles != null && roles.Any())
            {
                foreach (var role in roles)
                {
                    var setRef = new OrganizationRoleElementSetRef()
                    {
                        ElementSetID = setId,
                        OrganizationID = organizationId,
                        Role = role
                    };

                    radElementDbContext.OrganizationRoleElementSetRef.Add(setRef);
                }
            }
            else
            {
                var setRef = new OrganizationRoleElementSetRef()
                {
                    ElementSetID = setId,
                    OrganizationID = organizationId
                };

                radElementDbContext.OrganizationRoleElementSetRef.Add(setRef);
            }
        }

        /// <summary>
        /// Adds the organization element references.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="roles">The roles.</param>
        private void AddOrganizationElementReferences(int elementId, int organizationId, List<OrganizationRole> roles)
        {
            if (roles != null && roles.Any())
            {
                foreach (var role in roles)
                {
                    var elementRef = new OrganizationRoleElementRef()
                    {
                        ElementID = elementId,
                        OrganizationID = organizationId,
                        Role = role
                    };

                    radElementDbContext.OrganizationRoleElementRef.Add(elementRef);
                }
            }
            else
            {
                var elementRef = new OrganizationRoleElementRef()
                {
                    ElementID = elementId,
                    OrganizationID = organizationId
                };

                radElementDbContext.OrganizationRoleElementRef.Add(elementRef);
            }
        }

        /// <summary>
        /// Removes the organization element set references.
        /// </summary>
        /// <param name="organization">The organization.</param>
        private void RemoveOrganizationElementSetReferences(Organization organization)
        {
            var organizationElementSetRefs = radElementDbContext.OrganizationRoleElementSetRef.ToList().Where(x => x.OrganizationID == organization.Id);
            if (organizationElementSetRefs != null && organizationElementSetRefs.Any())
            {
                radElementDbContext.OrganizationRoleElementSetRef.RemoveRange(organizationElementSetRefs);
            }
        }

        /// <summary>
        /// Removes the organization element references.
        /// </summary>
        /// <param name="organization">The organization.</param>
        private void RemoveOrganizationElementReferences(Organization organization)
        {
            var organizationElementsRefs = radElementDbContext.OrganizationRoleElementRef.ToList().Where(x => x.OrganizationID == organization.Id);
            if (organizationElementsRefs != null && organizationElementsRefs.Any())
            {
                radElementDbContext.OrganizationRoleElementRef.RemoveRange(organizationElementsRefs);
            }
        }

        /// <summary>
        /// Gets the organization details dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private object GetOrganizationDetailsDto(object value)
        {
            if (value.GetType() == typeof(List<Organization>))
            {
                return mapper.Map<List<Organization>, List<OrganizationDetails>>(value as List<Organization>);
            }
            else if (value.GetType() == typeof(Organization))
            {
                return mapper.Map<OrganizationDetails>(value as Organization);
            }

            return null;
        }
    }
}
