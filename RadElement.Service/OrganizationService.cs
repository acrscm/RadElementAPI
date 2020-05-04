using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Core.Services;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
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
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementSetService" /> class.
        /// </summary>
        /// <param name="radElementDbContext">The RAD element database context.</param>
        /// <param name="logger">The logger.</param>
        public OrganizationService(
            RadElementDbContext radElementDbContext,
            ILogger logger)
        {
            this.radElementDbContext = radElementDbContext;
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
                return await Task.FromResult(new JsonResult(organizations, HttpStatusCode.OK));
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
                    var organization = radElementDbContext.Organization.Where(x => x.Id == organizationId).FirstOrDefault();

                    if (organization != null)
                    {
                        return await Task.FromResult(new JsonResult(organization, HttpStatusCode.OK));
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
        /// Searches the organization.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<JsonResult> SearchOrganizations(string searchKeyword)
        {

            try
            {
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    var filteredorganizations = radElementDbContext.Organization.Where(x => x.Name.ToLower().Contains(searchKeyword.ToLower())).ToList();
                    if (filteredorganizations != null && filteredorganizations.Any())
                    {
                        return await Task.FromResult(new JsonResult(filteredorganizations, HttpStatusCode.OK));
                    }
                    else
                    {
                        return await Task.FromResult(new JsonResult(string.Format("No such organization with keyword '{0}'.", searchKeyword), HttpStatusCode.NotFound));
                    }
                }

                return await Task.FromResult(new JsonResult("Keyword given is invalid.", HttpStatusCode.BadRequest));
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
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (organization == null)
                    {
                        return await Task.FromResult(new JsonResult("Organization fields are invalid.", HttpStatusCode.BadRequest));
                    }

                    var matchedOrganization = radElementDbContext.Organization.ToList().Where(
                        x => string.Equals(x.Name?.Trim(), organization.Name?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                             string.Equals(x.Abbreviation?.Trim(), organization.Abbreviation?.Trim(), StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                    if (matchedOrganization != null)
                    {
                        matchedOrganization.Url = organization.Url;
                        matchedOrganization.Comment = organization.Comment;
                        matchedOrganization.Email = organization.Email;
                        matchedOrganization.TwitterHandle = organization.TwitterHandle;

                        radElementDbContext.SaveChanges();
                        transaction.Commit();

                        var details = new OrganizationIdDetails()
                        {
                            OrganizationId = matchedOrganization.Id.ToString(),
                            Message = string.Format("Organization with name '{0}' is updated.", organization.Name)
                        };

                        return await Task.FromResult(new JsonResult(details, HttpStatusCode.OK));
                    }

                    var organizationData = new Organization()
                    {
                        Name = organization.Name,
                        Abbreviation = organization.Abbreviation,
                        Url = organization.Url,
                        Comment = organization.Comment,
                        Email = organization.Email,
                        TwitterHandle = organization.TwitterHandle
                    };

                    radElementDbContext.Organization.Add(organizationData);
                    radElementDbContext.SaveChanges();
                    transaction.Commit();

                    return await Task.FromResult(new JsonResult(new OrganizationIdDetails() { OrganizationId = organizationData.Id.ToString() }, HttpStatusCode.Created));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'CreateOrganization(CreateUpdateOrganization organization)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
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
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (organization == null)
                    {
                        return await Task.FromResult(new JsonResult("Organization fields are invalid.", HttpStatusCode.BadRequest));
                    }

                    if (organizationId != 0)
                    {
                        var organizationDetails = radElementDbContext.Organization.Where(x => x.Id == organizationId).FirstOrDefault();

                        if (organizationDetails != null)
                        {
                            organizationDetails.Name = organization.Name;
                            organizationDetails.Abbreviation = organization.Abbreviation;
                            organizationDetails.Url = organization.Url;
                            organizationDetails.Comment = organization.Comment;
                            organizationDetails.Email = organization.Email;
                            organizationDetails.TwitterHandle = organization.TwitterHandle;

                            radElementDbContext.SaveChanges();
                            transaction.Commit();

                            return await Task.FromResult(new JsonResult(string.Format("Organization with id '{0}' is updated.", organizationId), HttpStatusCode.OK));
                        }
                    }

                    return await Task.FromResult(new JsonResult(string.Format("No such organization with id '{0}'.", organizationId), HttpStatusCode.NotFound));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'UpdateOrganization(int organizationId, CreateUpdateOrganization organization)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Deletes the organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> DeleteOrganization(int organizationId)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (organizationId != 0)
                    {
                        var organization = radElementDbContext.Organization.Where(x => x.Id == organizationId).FirstOrDefault();

                        if (organization != null)
                        {
                            RemoveOrganizationElementReferences(organization);
                            RemoveOrganizationElementSetReferences(organization);

                            radElementDbContext.Organization.Remove(organization);
                            radElementDbContext.SaveChanges();
                            transaction.Commit();

                            return await Task.FromResult(new JsonResult(string.Format("Organization with id '{0}' is deleted.", organizationId), HttpStatusCode.OK));
                        }
                    }
                    return await Task.FromResult(new JsonResult(string.Format("No such organization with id '{0}'.", organizationId), HttpStatusCode.NotFound));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'DeleteOrganization(int organizationId)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Removes the organization element set references.
        /// </summary>
        /// <param name="organization">The organization.</param>
        private void RemoveOrganizationElementSetReferences(Organization organization)
        {
            var organizationElementSetRefs = radElementDbContext.OrganizationRoleElementSetRef.Where(x => x.OrganizationID == organization.Id);
            if (organizationElementSetRefs != null && organizationElementSetRefs.Any())
            {
                radElementDbContext.OrganizationRoleElementSetRef.RemoveRange(organizationElementSetRefs);
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Removes the organization element references.
        /// </summary>
        /// <param name="organization">The organization.</param>
        private void RemoveOrganizationElementReferences(Organization organization)
        {
            var organizationElementsRefs = radElementDbContext.OrganizationRoleElementRef.Where(x => x.OrganizationID == organization.Id);
            if (organizationElementsRefs != null && organizationElementsRefs.Any())
            {
                radElementDbContext.OrganizationRoleElementRef.RemoveRange(organizationElementsRefs);
                radElementDbContext.SaveChanges();
            }
        }
    }
}
