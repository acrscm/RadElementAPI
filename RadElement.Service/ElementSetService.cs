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
        /// The logger
        /// </summary>
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
                var sets = radElementDbContext.ElementSet.ToList();
                return await Task.FromResult(new JsonResult(GetElementSetDetailsArrayDto(sets), HttpStatusCode.OK));
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
                    var sets = radElementDbContext.ElementSet.ToList();
                    var elementSets = GetElementSetDetailsArrayDto(sets);
                    var filteredSets = elementSets.FindAll(x => x.SetId.ToString().ToLower().Contains(searchKeyword.ToLower()) ||
                                                    x.Name.ToLower().Contains(searchKeyword.ToLower()) ||
                                                    x.Description.ToLower().Contains(searchKeyword.ToLower()) ||
                                                    x.ContactName.ToLower().Contains(searchKeyword.ToLower())); ;
                    if (filteredSets != null && filteredSets.Any())
                    {
                        return await Task.FromResult(new JsonResult(filteredSets, HttpStatusCode.OK));
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
                ElementSet set = new ElementSet()
                {
                    Name = content.ModuleName.Trim(),
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
                    var elementSets = radElementDbContext.ElementSet.ToList();
                    var elementSet = elementSets.Find(x => x.Id == id);

                    if (elementSet != null)
                    {
                        elementSet.Name = content.ModuleName.Trim();
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
            if (setId.Length > 4 && setId.Substring(0, 4) == "RDES")
            {
                int id;
                bool result = Int32.TryParse(setId.Remove(0, 4), out id);
                return result;
            }

            return false;
        }

        /// <summary>
        /// Gets the element set details array dto.
        /// </summary>
        /// <param name="sets">The sets.</param>
        /// <returns></returns>
        private List<ElementSetDetails> GetElementSetDetailsArrayDto(List<ElementSet> sets)
        {
            List<ElementSetDetails> setDetails = new List<ElementSetDetails>();
            foreach (var set in sets)
            {
                setDetails.Add(GetElementSetDetailsDto(set));
            }

            return setDetails;
        }

        /// <summary>
        /// Gets the element set details dto.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns></returns>
        private ElementSetDetails GetElementSetDetailsDto(ElementSet set)
        {
            return new ElementSetDetails()
            {
                Id = set.Id,
                SetId = "RDES" + set.Id,
                ContactName = set.ContactName,
                Description = set.Description,
                Name = set.Name,
                ParentId = set.ParentId,
                Status = set.Status,
                StatusDate = set.StatusDate,
                Modality = set.Modality,
                BiologicalSex = set.BiologicalSex,
                AgeLowerBound = set.AgeLowerBound,
                AgeUpperBound = set.AgeUpperBound,
                Version = set.Version
            };
        }
    }
}
