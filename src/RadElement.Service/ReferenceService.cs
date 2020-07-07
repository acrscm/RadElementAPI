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
    /// Business service for handling the reference related operations
    /// </summary>
    /// <seealso cref="RadElement.Core.Services.IReferenceService" />
    public class ReferenceService : IReferenceService
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
        /// Initializes a new instance of the <see cref="ReferenceService" /> class.
        /// </summary>
        /// <param name="radElementDbContext">The RAD element database context.</param>
        /// <param name="logger">The logger.</param>
        public ReferenceService(
            RadElementDbContext radElementDbContext,
            ILogger logger)
        {
            this.radElementDbContext = radElementDbContext;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetReferences()
        {
            try
            {
                var references = radElementDbContext.Reference.ToList();
                return await Task.FromResult(new JsonResult(references, HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetReferences()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Gets the reference.
        /// </summary>
        /// <param name="referenceId">The reference identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> GetReference(int referenceId)
        {
            try
            {
                if (referenceId != 0)
                {
                    var reference = radElementDbContext.Reference.Where(x => x.Id == referenceId).FirstOrDefault();
                    if (reference != null)
                    {
                        return await Task.FromResult(new JsonResult(reference, HttpStatusCode.OK));
                    }
                }
                return await Task.FromResult(new JsonResult(string.Format("No such reference with id '{0}'.", referenceId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetReference(int referenceId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Searches the references.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<JsonResult> SearchReferences(string searchKeyword)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    var filteredReference = radElementDbContext.Reference.Where(x => x.Citation.Contains(searchKeyword, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    if (filteredReference != null && filteredReference.Any())
                    {
                        return await Task.FromResult(new JsonResult(filteredReference, HttpStatusCode.OK));
                    }
                    else
                    {
                        return await Task.FromResult(new JsonResult(string.Format("No such reference with keyword '{0}'.", searchKeyword), HttpStatusCode.NotFound));
                    }
                }

                return await Task.FromResult(new JsonResult("Keyword given is invalid.", HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'SearchReferences(string searchKeyword)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Creates the reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <returns></returns>
        public async Task<JsonResult> CreateReference(CreateUpdateReference reference)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (reference == null)
                    {
                        return await Task.FromResult(new JsonResult("Reference fields are invalid.", HttpStatusCode.BadRequest));
                    }

                    var matchedReference = radElementDbContext.Reference.ToList().Where(
                        x => string.Equals(x.Citation?.Trim(), reference.Citation?.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                             string.Equals(x.Doi_Uri?.Trim(), reference.DoiUri?.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                             string.Equals(x.Url?.Trim(), reference.Url?.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                             x.Pubmed_Id == reference.PubmedId).FirstOrDefault();

                    if (matchedReference != null)
                    {

                        var referenceDetails = new ReferenceIdDetails()
                        {
                            ReferenceId = matchedReference.Id.ToString()
                        };

                        return await Task.FromResult(new JsonResult(referenceDetails, HttpStatusCode.OK));
                    }

                    var referenceData = new Reference()
                    {
                        Citation = reference.Citation,
                        Doi_Uri = reference.DoiUri,
                        Pubmed_Id = reference.PubmedId,
                        Url = reference.Url,
                    };

                    radElementDbContext.Reference.Add(referenceData);
                    radElementDbContext.SaveChanges();
                    transaction.Commit();

                    return await Task.FromResult(new JsonResult(new ReferenceIdDetails() { ReferenceId = referenceData.Id.ToString() }, HttpStatusCode.Created));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'CreateReference(CreateUpdateReference reference)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Updates the reference.
        /// </summary>
        /// <param name="referenceId">The reference identifier.</param>
        /// <param name="reference">The reference.</param>
        /// <returns></returns>
        public async Task<JsonResult> UpdateReference(int referenceId, CreateUpdateReference reference)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (reference == null)
                    {
                        return await Task.FromResult(new JsonResult("Reference fields are invalid.", HttpStatusCode.BadRequest));
                    }

                    if (referenceId != 0)
                    {
                        var referenceDetails = radElementDbContext.Reference.Where(x => x.Id == referenceId).FirstOrDefault();

                        if (referenceDetails != null)
                        {
                            referenceDetails.Citation = reference.Citation;
                            referenceDetails.Doi_Uri = reference.DoiUri;
                            referenceDetails.Pubmed_Id = reference.PubmedId;
                            referenceDetails.Url = reference.Url;

                            radElementDbContext.SaveChanges();
                            transaction.Commit();

                            return await Task.FromResult(new JsonResult(string.Format("Reference with id '{0}' is updated.", referenceId), HttpStatusCode.OK));
                        }
                    }

                    return await Task.FromResult(new JsonResult(string.Format("No such reference with id '{0}'.", referenceId), HttpStatusCode.NotFound));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'UpdateReference(int referenceId, CreateUpdateReference reference)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Deletes the reference.
        /// </summary>
        /// <param name="referenceId">The reference identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> DeleteReference(int referenceId)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (referenceId != 0)
                    {
                        var reference = radElementDbContext.Reference.Where(x => x.Id == referenceId).FirstOrDefault();

                        if (reference != null)
                        {
                            RemoveReferences(reference);

                            radElementDbContext.Reference.Remove(reference);
                            radElementDbContext.SaveChanges();
                            transaction.Commit();

                            return await Task.FromResult(new JsonResult(string.Format("Reference with id '{0}' is deleted.", referenceId), HttpStatusCode.OK));
                        }
                    }
                    return await Task.FromResult(new JsonResult(string.Format("No such reference with id '{0}'.", referenceId), HttpStatusCode.NotFound));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'DeleteReference(int referenceId)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Removes the references.
        /// </summary>
        /// <param name="reference">The reference.</param>
        private void RemoveReferences(Reference reference)
        {
            var refs = radElementDbContext.ReferenceRef.Where(x => x.Reference_Id == reference.Id);
            if (refs != null && refs.Any())
            {
                radElementDbContext.ReferenceRef.RemoveRange(refs);
                radElementDbContext.SaveChanges();
            }
        }
    }
}
