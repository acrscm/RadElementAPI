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
    /// Business service for handling the index code related operations
    /// </summary>
    /// <seealso cref="RadElement.Core.Services.IndexCodeService" />
    public class IndexCodeService : IIndexCodeService
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
        /// Initializes a new instance of the <see cref="IndexCodeService" /> class.
        /// </summary>
        /// <param name="radElementDbContext">The RAD element database context.</param>
        /// <param name="logger">The logger.</param>
        public IndexCodeService(
            RadElementDbContext radElementDbContext,
            ILogger logger)
        {
            this.radElementDbContext = radElementDbContext;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the index codes.
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetIndexCodes()
        {
            try
            {
                var indexCodes = radElementDbContext.IndexCode.ToList();
                return await Task.FromResult(new JsonResult(indexCodes, HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetIndexCodes()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Gets the index code.
        /// </summary>
        /// <param name="indexCodeId">The index identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> GetIndexCode(int indexCodeId)
        {
            try
            {
                if (indexCodeId != 0)
                {
                    var indexCode = radElementDbContext.IndexCode.Where(x => x.Id == indexCodeId).FirstOrDefault();
                    if (indexCode != null)
                    {
                        return await Task.FromResult(new JsonResult(indexCode, HttpStatusCode.OK));
                    }
                }
                return await Task.FromResult(new JsonResult(string.Format("No such index code with id '{0}'.", indexCodeId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetIndexCode(int indexCodeId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Searches the index codes.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<JsonResult> SearchIndexCodes(string searchKeyword)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    var filteredIndexCodes = radElementDbContext.IndexCode.Where(x => x.System.Contains(searchKeyword, StringComparison.InvariantCultureIgnoreCase) ||
                                                                                      x.Code.Contains(searchKeyword, StringComparison.InvariantCultureIgnoreCase) ||
                                                                                      x.Display.Contains(searchKeyword, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    if (filteredIndexCodes != null && filteredIndexCodes.Any())
                    {
                        return await Task.FromResult(new JsonResult(filteredIndexCodes, HttpStatusCode.OK));
                    }
                    else
                    {
                        return await Task.FromResult(new JsonResult(string.Format("No such index code with keyword '{0}'.", searchKeyword), HttpStatusCode.NotFound));
                    }
                }

                return await Task.FromResult(new JsonResult("Keyword given is invalid.", HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'SearchIndexCodes(string searchKeyword)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Creates the index code.
        /// </summary>
        /// <param name="indexCode">The index code.</param>
        /// <returns></returns>
        public async Task<JsonResult> CreateIndexCode(CreateUpdateIndexCode indexCode)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (indexCode == null)
                    {
                        return await Task.FromResult(new JsonResult("Index code fields are invalid.", HttpStatusCode.BadRequest));
                    }

                    var indexCodeSystem = radElementDbContext.IndexCodeSystem.Where(x => string.Equals(x.Abbrev, indexCode.System, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (indexCodeSystem != null)
                    {
                        var indexCodeDetails = radElementDbContext.IndexCode.Where(x => string.Equals(x.System, indexCode.System, StringComparison.InvariantCultureIgnoreCase) &&
                                                                                        string.Equals(x.Code, indexCode.Code, StringComparison.InvariantCultureIgnoreCase) &&
                                                                                        string.Equals(x.Display, indexCode.Display, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        if (indexCodeDetails != null)
                        {
                            var indexCodeIdDetails = new IndexCodeIdDetails()
                            {
                                IndexCodeId = indexCodeDetails.Id.ToString(),
                                Message = string.Format("Index code with name '{0}' is updated.", indexCodeDetails.System)
                            };

                            return await Task.FromResult(new JsonResult(indexCodeIdDetails, HttpStatusCode.OK));
                        }
                        else
                        {
                            var indexCodeSystemData = new IndexCode
                            {
                                Code = indexCode.Code,
                                System = indexCode.System,
                                Display = indexCode.Display,
                                AccessionDate = DateTime.UtcNow,
                                Href = indexCode.Href
                            };

                            radElementDbContext.IndexCode.Add(indexCodeSystemData);
                            radElementDbContext.SaveChanges();
                            transaction.Commit();

                            return await Task.FromResult(new JsonResult(new IndexCodeIdDetails() { IndexCodeId = indexCodeSystemData.Id.ToString() }, HttpStatusCode.Created));
                        }
                    }

                    return await Task.FromResult(new JsonResult(string.Format("No such index code system with code '{0}'.", indexCode.System), HttpStatusCode.NotFound));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'CreateIndexCode(CreateUpdateIndexCode indexCode)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Updates the index code.
        /// </summary>
        /// <param name="indexCodeId">The index identifier.</param>
        /// <param name="indexCode">The index code.</param>
        /// <returns></returns>
        public async Task<JsonResult> UpdateIndexCode(int indexCodeId, CreateUpdateIndexCode indexCode)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (indexCode == null)
                    {
                        return await Task.FromResult(new JsonResult("Index code fields are invalid.", HttpStatusCode.BadRequest));
                    }

                    if (indexCodeId != 0)
                    {
                        var indexCodeDetails = radElementDbContext.IndexCode.Where(x => x.Id == indexCodeId).FirstOrDefault();

                        if (indexCodeDetails != null)
                        {
                            indexCodeDetails.Code = indexCode.Code;
                            indexCodeDetails.System = indexCode.System;
                            indexCodeDetails.Display = indexCode.Display;
                            indexCodeDetails.AccessionDate = DateTime.UtcNow;
                            indexCodeDetails.Href = indexCode.Href;

                            radElementDbContext.SaveChanges();
                            transaction.Commit();

                            return await Task.FromResult(new JsonResult(string.Format("Index code with id '{0}' is updated.", indexCodeId), HttpStatusCode.OK));
                        }
                    }

                    return await Task.FromResult(new JsonResult(string.Format("No such index code with id '{0}'.", indexCodeId), HttpStatusCode.NotFound));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'UpdateIndexCode(int indexCodeId, CreateUpdateIndexCode indexCode)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Deletes the index code.
        /// </summary>
        /// <param name="indexCodeId">The index identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> DeleteIndexCode(int indexCodeId)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (indexCodeId != 0)
                    {
                        var indexCode = radElementDbContext.IndexCode.Where(x => x.Id == indexCodeId).FirstOrDefault();

                        if (indexCode != null)
                        {
                            RemoveIndexCodeElementSetReferences(indexCode);
                            RemoveIndexCodeElementReferences(indexCode);
                            RemoveIndexCodeElementValuesReferences(indexCode);

                            radElementDbContext.IndexCode.Remove(indexCode);
                            radElementDbContext.SaveChanges();
                            transaction.Commit();

                            return await Task.FromResult(new JsonResult(string.Format("Index code with id '{0}' is deleted.", indexCodeId), HttpStatusCode.OK));
                        }
                    }
                    return await Task.FromResult(new JsonResult(string.Format("No such index code with id '{0}'.", indexCodeId), HttpStatusCode.NotFound));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'DeleteIndexCode(int indexCodeId)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Removes the index code element set references.
        /// </summary>
        /// <param name="indexCode">The index code.</param>
        private void RemoveIndexCodeElementSetReferences(IndexCode indexCode)
        {
            var indexCodeElementSetRefs = radElementDbContext.IndexCodeElementSetRef.Where(x => x.CodeId == indexCode.Id);
            if (indexCodeElementSetRefs != null && indexCodeElementSetRefs.Any())
            {
                radElementDbContext.IndexCodeElementSetRef.RemoveRange(indexCodeElementSetRefs);
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Removes the index code element references.
        /// </summary>
        /// <param name="indexCode">The index code.</param>
        private void RemoveIndexCodeElementReferences(IndexCode indexCode)
        {
            var indexCodeElementRefs = radElementDbContext.IndexCodeElementRef.Where(x => x.CodeId == indexCode.Id);
            if (indexCodeElementRefs != null && indexCodeElementRefs.Any())
            {
                radElementDbContext.IndexCodeElementRef.RemoveRange(indexCodeElementRefs);
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Removes the index code element valuesreferences.
        /// </summary>
        /// <param name="indexCode">The index code.</param>
        private void RemoveIndexCodeElementValuesReferences(IndexCode indexCode)
        {
            var indexCodeElementValueRefs = radElementDbContext.IndexCodeElementValueRef.Where(x => x.CodeId == indexCode.Id);
            if (indexCodeElementValueRefs != null && indexCodeElementValueRefs.Any())
            {
                radElementDbContext.IndexCodeElementValueRef.RemoveRange(indexCodeElementValueRefs);
                radElementDbContext.SaveChanges();
            }
        }
    }
}
