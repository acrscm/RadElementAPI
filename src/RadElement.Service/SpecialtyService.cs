using RadElement.Core.Domain;
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
    /// Business service for handling the specialty related operations
    /// </summary>
    /// <seealso cref="RadElement.Core.Services.SpecialtyService" />
    public class SpecialtyService : ISpecialtyService
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
        /// Initializes a new instance of the <see cref="SpecialtyService" /> class.
        /// </summary>
        /// <param name="radElementDbContext">The RAD element database context.</param>
        /// <param name="logger">The logger.</param>
        public SpecialtyService(
            RadElementDbContext radElementDbContext,
            ILogger logger)
        {
            this.radElementDbContext = radElementDbContext;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the specialties.
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetSpecialties()
        {
            try
            {
                var indexCodes = radElementDbContext.Specialty.ToList();
                return await Task.FromResult(new JsonResult(indexCodes, HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetSpecialties()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Gets the specialty.
        /// </summary>
        /// <param name="specialtyId">The specialty identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> GetSpecialty(int specialtyId)
        {
            try
            {
                if (specialtyId != 0)
                {
                    var specialty = radElementDbContext.Specialty.Where(x => x.Id == specialtyId).FirstOrDefault();
                    if (specialty != null)
                    {
                        return await Task.FromResult(new JsonResult(specialty, HttpStatusCode.OK));
                    }
                }
                return await Task.FromResult(new JsonResult(string.Format("No such specialty with id '{0}'.", specialtyId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetSpecialty(int specialtyId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Searches the specialties.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<JsonResult> SearchSpecialties(string searchKeyword)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    var filteredSpecialties = radElementDbContext.Specialty.Where(x => x.Short_Name.Contains(searchKeyword, StringComparison.InvariantCultureIgnoreCase) ||
                                                                                      x.Code.Contains(searchKeyword, StringComparison.InvariantCultureIgnoreCase) ||
                                                                                      x.Name.Contains(searchKeyword, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    if (filteredSpecialties != null && filteredSpecialties.Any())
                    {
                        return await Task.FromResult(new JsonResult(filteredSpecialties, HttpStatusCode.OK));
                    }
                    else
                    {
                        return await Task.FromResult(new JsonResult(string.Format("No such specialty with keyword '{0}'.", searchKeyword), HttpStatusCode.NotFound));
                    }
                }

                return await Task.FromResult(new JsonResult("Keyword given is invalid.", HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'SearchSpecialties(string searchKeyword)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }
    }
}
