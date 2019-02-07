using RadElement.Core.Data;
using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using System.Net;
using System;

namespace RadElement.Service
{
    public class ElementSetService : IElementSetService
    {
        private IRadElementDbContext radElementDbContext;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementSetService" /> class.
        /// </summary>
        /// <param name="radElementDbContext">The RAD element database context.</param>
        /// <param name="logger">The logger.</param>
        public ElementSetService(IRadElementDbContext radElementDbContext, ILogger logger)
        {
            this.radElementDbContext = radElementDbContext;
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
                var sets = await radElementDbContext.ElementSet.ToListAsync();
                return new JsonResult(sets, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetSets()'");
                return new JsonResult(ex, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Gets the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> GetSet(int setId)
        {
            try
            {
                var sets = await radElementDbContext.ElementSet.ToListAsync();
                var set = sets.Find(x => x.Id == setId);

                if (set != null)
                {
                    return new JsonResult(set, HttpStatusCode.OK);
                }
                else
                {
                    return new JsonResult(string.Format("No such set with id '{0}'", setId), HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetSet(int setId)'");
                return new JsonResult(ex, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Searches the cde set.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<JsonResult> SearchSet(string searchKeyword)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    var sets = await radElementDbContext.ElementSet.ToListAsync();
                    var filteredSets = sets.FindAll(x => x.Name.ToLower().Contains(searchKeyword.ToLower()) || x.Description.ToLower().Contains(searchKeyword.ToLower()) ||
                                                x.ContactName.ToLower().Contains(searchKeyword.ToLower())); ;
                    if (filteredSets != null && filteredSets.Any())
                    {
                        return new JsonResult(filteredSets, HttpStatusCode.OK);
                    }
                    else
                    {
                        return new JsonResult(string.Format("No such set with keyword '{0}'.", searchKeyword), HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    return new JsonResult(string.Format("Keyword '{0}' given is invalid", searchKeyword), HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'SearchSet(string searchKeyword)'");
                return new JsonResult(ex, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Creates the cde set.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<JsonResult> CreateSet(CreateUpdateSet content)
        {
            try
            {
                if (content == null || string.IsNullOrEmpty(content.ModuleName) || string.IsNullOrEmpty(content.ContactName) || string.IsNullOrEmpty(content.Description))
                {
                    return new JsonResult("Element set is invalid", HttpStatusCode.BadRequest);
                }

                ElementSet set = new ElementSet()
                {
                    Name = content.ModuleName.Replace("_", " "),
                    Description = content.Description,
                    ContactName = content.ContactName,
                };

                await radElementDbContext.ElementSet.AddAsync(set);
                radElementDbContext.SaveChanges();

                if (set.Id != 0)
                {
                    return new JsonResult(new SetIdDetails() { SetId = set.Id.ToString() }, HttpStatusCode.Created);
                }
                else
                {
                    return new JsonResult(new SetIdDetails() { SetId = set.Id.ToString() }, HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'CreateSet(CreateUpdateSet content)'");
                return new JsonResult(ex, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<JsonResult> UpdateSet(int setId, CreateUpdateSet content)
        {
            try
            {
                if (content == null || string.IsNullOrEmpty(content.ModuleName) || string.IsNullOrEmpty(content.ContactName) || string.IsNullOrEmpty(content.Description))
                {
                    return new JsonResult("Element set is invalid", HttpStatusCode.BadRequest);
                }

                var elementSets = await radElementDbContext.ElementSet.ToListAsync();
                var elementSet = elementSets.Find(x => x.Id == setId);

                if (elementSet != null)
                {
                    elementSet.Name = content.ModuleName.Replace("_", " ");
                    elementSet.Description = content.Description;
                    elementSet.ContactName = content.ContactName;
                    radElementDbContext.SaveChanges();
                    return new JsonResult(string.Format("Set with id {0} is updated.", setId), HttpStatusCode.OK);
                }

                return new JsonResult(string.Format("No such set with id {0}.", setId), HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'CreateSet(CreateUpdaUpdateSet(int setId, CreateUpdateSet content)'");
                return new JsonResult(ex, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Deletes the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> DeleteSet(int setId)
        {
            try
            {
                var elementSets = await radElementDbContext.ElementSet.ToListAsync();
                var elementSet = elementSets.Find(x => x.Id == setId);

                if (elementSet != null)
                {
                    var elementSetRefs = radElementDbContext.ElementSetRef.ToList().FindAll(x => x.ElementSetId == elementSet.Id);
                    if (elementSetRefs != null && elementSetRefs.Any())
                    {
                        foreach (var setref in elementSetRefs)
                        {
                            var elementValues = radElementDbContext.ElementValue.ToList().FindAll(x => x.ElementId == setref.ElementId);
                            var elements = radElementDbContext.Element.ToList().FindAll(x => x.Id == setref.ElementId);

                            if (elementValues != null && elementValues.Any())
                            {
                                radElementDbContext.ElementValue.RemoveRange(elementValues);
                            }

                            if (elements != null && elements.Any())
                            {
                                radElementDbContext.Element.RemoveRange(elements);
                            }
                        }

                        radElementDbContext.ElementSetRef.RemoveRange(elementSetRefs);
                    }

                    radElementDbContext.ElementSet.Remove(elementSet);
                    radElementDbContext.SaveChanges();
                    return new JsonResult(string.Format("Set with id {0} is deleted.", setId), HttpStatusCode.OK);
                }

                return new JsonResult(string.Format("No such set with id {0}.", setId), HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'DeleteSet(int setId)'");
                return new JsonResult(ex, HttpStatusCode.InternalServerError);
            }
        }
    }
}
