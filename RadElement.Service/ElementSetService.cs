using RadElement.Core.Data;
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
        private IRadElementDbContext radElementDbContext;

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
            IRadElementDbContext radElementDbContext,
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
                    var sets = radElementDbContext.ElementSet.ToList();
                    var set = sets.Find(x => x.Id == id);

                    if (set != null)
                    {
                        return await Task.FromResult(new JsonResult(GetElementSetDetailsDto(set), HttpStatusCode.OK));
                    }
                }
                return await Task.FromResult(new JsonResult(string.Format("No such set with id '{0}'.", setId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetSet(int setId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
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
                    var sets = radElementDbContext.ElementSet.ToList().Where(x => string.Concat("RDES", x.Id).ToLower().Contains(searchKeyword.ToLower()) ||
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
                else
                {
                    return await Task.FromResult(new JsonResult(string.Format("Keyword '{0}' given is invalid", searchKeyword), HttpStatusCode.BadRequest));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'SearchSet(string searchKeyword)'");
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
            try
            {
                if (content == null)
                {
                    return await Task.FromResult(new JsonResult("Element set is invalid", HttpStatusCode.BadRequest));
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

                return await Task.FromResult(new JsonResult(new SetIdDetails() { SetId = "RDES" + set.Id.ToString() }, HttpStatusCode.Created));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'CreateSet(CreateUpdateSet content)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
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
            try
            {
                if (IsValidSetId(setId))
                {
                    int id = Convert.ToInt32(setId.Remove(0, 4));

                    if (content == null)
                    {
                        return await Task.FromResult(new JsonResult("Element set is invalid", HttpStatusCode.BadRequest));
                    }

                    var elementSets = radElementDbContext.ElementSet.ToList();
                    var elementSet = elementSets.Find(x => x.Id == id);

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

                        return await Task.FromResult(new JsonResult(string.Format("Set with id {0} is updated.", setId), HttpStatusCode.OK));
                    }
                }

                return await Task.FromResult(new JsonResult(string.Format("No such set with id '{0}'.", setId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'CreateSet(CreateUpdaUpdateSet(int setId, CreateUpdateSet content)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Deletes the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> DeleteSet(string setId)
        {
            try
            {
                if (IsValidSetId(setId))
                {
                    int id = Convert.ToInt32(setId.Remove(0, 4));
                    var elementSets = radElementDbContext.ElementSet.ToList();
                    var elementSet = elementSets.Find(x => x.Id == id);

                    if (elementSet != null)
                    {
                        RemoveSetElements(elementSet);

                        radElementDbContext.ElementSet.Remove(elementSet);
                        radElementDbContext.SaveChanges();

                        return await Task.FromResult(new JsonResult(string.Format("Set with id {0} is deleted.", setId), HttpStatusCode.OK));
                    }
                }
                return await Task.FromResult(new JsonResult(string.Format("No such set with id '{0}'.", setId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'DeleteSet(int setId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
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
        private void RemoveSetElements(ElementSet elementSet)
        {
            var elementSetRefs = radElementDbContext.ElementSetRef.ToList().Where(x => x.ElementSetId == elementSet.Id);
            if (elementSetRefs != null && elementSetRefs.Any())
            {
                foreach (var setref in elementSetRefs)
                {
                    var elementValues = radElementDbContext.ElementValue.ToList().Where(x => x.ElementId == setref.ElementId);
                    var elements = radElementDbContext.Element.ToList().Where(x => x.Id == setref.ElementId);

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
        }

        /// <summary>
        /// Gets the element set details dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private object GetElementSetDetailsDto(object value)
        {
            if (value.GetType() == typeof(List<ElementSet>))
            {
                return mapper.Map<List<ElementSet>, List<ElementSetDetails>>(value as List<ElementSet>);
            }
            else if (value.GetType() == typeof(ElementSet))
            {
                return mapper.Map<ElementSetDetails>(value as ElementSet);
            }

            return null;
        }
    }
}
