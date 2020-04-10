using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RadElement.Core.Data;
using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Core.Services;
using System.Net;
using Serilog;
using AutoMapper;

namespace RadElement.Service
{
    /// <summary>
    /// Business service for handling the element related operations
    /// </summary>
    /// <seealso cref="RadElement.Core.Services.IElementService" />
    public class ElementService : IElementService
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
        /// Initializes a new instance of the <see cref="ElementService" /> class.
        /// </summary>
        /// <param name="radElementDbContext">The RAD element database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public ElementService(
            IRadElementDbContext radElementDbContext,
            IMapper mapper,
            ILogger logger)
        {
            this.radElementDbContext = radElementDbContext;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetElements()
        {
            try
            {
                var elements = radElementDbContext.Element.ToList();
                return await Task.FromResult(new JsonResult(GetElementDetailsDto(elements), HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetElements()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> GetElement(string elementId)
        {
            try
            {
                if (IsValidElementId(elementId))
                {
                    int setInternalId = Convert.ToInt32(elementId.Remove(0, 3));
                    var elements = radElementDbContext.Element.ToList();
                    var element = elements.Find(x => x.Id == setInternalId);

                    if (element != null)
                    {
                        return await Task.FromResult(new JsonResult(GetElementDetailsDto(element), HttpStatusCode.OK));
                    }
                }

                return await Task.FromResult(new JsonResult(string.Format("No such element with id '{0}'.", elementId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetElement(int elementId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Gets the elements by set identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> GetElementsBySetId(string setId)
        {
            try
            {
                if (IsValidSetId(setId))
                {
                    int setInternalId = Convert.ToInt32(setId.Remove(0, 4));
                    var setRefs = radElementDbContext.ElementSetRef.ToList();
                    var elementIds = setRefs.Where(x => x.ElementSetId == setInternalId);
                    var elements = radElementDbContext.Element.ToList();

                    var selectedElements = from elemetId in elementIds
                                           join element in elements on elemetId.ElementId equals (int)element.Id
                                           select element;

                    if (selectedElements != null && selectedElements.Any())
                    {
                        return await Task.FromResult(new JsonResult(GetElementDetailsDto(selectedElements.ToList()), HttpStatusCode.OK));
                    }
                }
                return await Task.FromResult(new JsonResult(string.Format("No such elements with set id '{0}'.", setId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetElementsBySetId(int setId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Searches the element.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<JsonResult> SearchElement(SearchKeyword searchKeyword)
        {
            try
            {
                var elements = radElementDbContext.Element.ToList();
                var filteredElements = elements.Where(x => string.Concat("RDE", x.Id).ToLower().Contains(searchKeyword.Keyword.ToLower()) ||
                                                           x.Name.ToLower().Contains(searchKeyword.Keyword.ToLower())).ToList();
                if (filteredElements != null && filteredElements.Any())
                {
                    return await Task.FromResult(new JsonResult(GetElementDetailsDto(filteredElements), HttpStatusCode.OK));
                }
                else
                {
                    return await Task.FromResult(new JsonResult(string.Format("No such element with keyword '{0}'.", searchKeyword.Keyword), HttpStatusCode.NotFound));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'SearchElement(string searchKeyword)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Creates the element.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="dataElement">The data element.</param>
        /// <returns></returns>
        public async Task<JsonResult> CreateElement(string setId, CreateUpdateElement dataElement)
        {
            try
            {
                if (IsValidSetId(setId))
                {
                    int setInternalId = Convert.ToInt32(setId.Remove(0, 4));
                    if (dataElement == null)
                    {
                        return await Task.FromResult(new JsonResult("Dataelement fields are invalid in request", HttpStatusCode.BadRequest));
                    }

                    if (dataElement.ValueType == DataElementType.Choice || dataElement.ValueType == DataElementType.MultiChoice)
                    {
                        if (dataElement.Options == null || dataElement.Options.Count == 0)
                        {
                            return await Task.FromResult(new JsonResult("'Options' field is missing for Choice type elements in request", HttpStatusCode.BadRequest));
                        }
                    }

                    var elementSets = radElementDbContext.ElementSet.ToList();
                    var elementSet = elementSets.Find(x => x.Id == setInternalId);

                    if (elementSet != null)
                    {
                        Element element = new Element()
                        {
                            Name = dataElement.Label,
                            ShortName = dataElement.ShortName ?? string.Empty,
                            Definition = dataElement.Definition ?? string.Empty,
                            ValueType = GetElementValueType(dataElement.ValueType),
                            MinCardinality = 1,
                            MaxCardinality = (dataElement.ValueType == DataElementType.MultiChoice) ? (uint)dataElement.Options.Count : (uint)1,
                            Unit = dataElement.Unit ?? string.Empty,
                            Question = dataElement.Question ?? dataElement.Label,
                            Instructions = dataElement.Instructions ?? string.Empty,
                            References = dataElement.References ?? string.Empty,
                            Version = dataElement.Version ?? "",
                            VersionDate = dataElement.VersionDate ?? DateTime.Now,
                            Synonyms = dataElement.Synonyms ?? string.Empty,
                            Source = dataElement.Source ?? string.Empty,
                            Status = "Proposed",
                            StatusDate = DateTime.Now,
                            Editor = dataElement.Editor ?? string.Empty,
                            Modality = dataElement.Modality != null && dataElement.Modality.Any() ? string.Join(",", dataElement.Modality) : null,
                            BiologicalSex = dataElement.BiologicalSex != null && dataElement.BiologicalSex.Any() ? string.Join(",", dataElement.BiologicalSex) : null,
                            AgeLowerBound = dataElement.AgeLowerBound,
                            AgeUpperBound = dataElement.AgeUpperBound,
                            ValueSize = 0
                        };

                        if (dataElement.ValueType == DataElementType.Integer)
                        {
                            element.ValueMin = dataElement.ValueMin;
                            element.ValueMax = dataElement.ValueMax;
                            element.StepValue = 1;
                        }

                        if (dataElement.ValueType == DataElementType.Numeric)
                        {
                            float? minValue = null;
                            float? maxValue = null;

                            if (dataElement.ValueMin.HasValue)
                            {
                                minValue = Convert.ToSingle(dataElement.ValueMin.Value);
                            }

                            if (dataElement.ValueMax.HasValue)
                            {
                                maxValue = Convert.ToSingle(dataElement.ValueMax.Value);
                            }

                            element.ValueMin = minValue;
                            element.ValueMax = maxValue;
                            element.StepValue = 0.1f;
                        }

                        radElementDbContext.Element.Add(element);
                        radElementDbContext.SaveChanges();

                        if (dataElement.ValueType == DataElementType.MultiChoice || dataElement.ValueType == DataElementType.Choice)
                        {
                            AddElementValues(dataElement.Options, element.Id);
                        }

                        ElementSetRef setRef = new ElementSetRef()
                        {
                            ElementSetId = setInternalId,
                            ElementId = (int)element.Id
                        };

                        radElementDbContext.ElementSetRef.Add(setRef);
                        radElementDbContext.SaveChanges();

                        return await Task.FromResult(new JsonResult(new ElementIdDetails() { ElementId = "RDE" + element.Id.ToString() }, HttpStatusCode.Created));
                    }
                }

                return await Task.FromResult(new JsonResult(string.Format("No such element with set id {0}.", setId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'CreateElement(int setId, DataElementType elementType, CreateUpdateElement dataElement)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Creates the element.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> CreateElement(string setId, string elementId)
        {
            try
            {
                if (IsValidSetId(setId))
                {
                    int setInternalId = Convert.ToInt32(setId.Remove(0, 4));
                    var elementSets = radElementDbContext.ElementSet.ToList();
                    var elementSet = elementSets.Find(x => x.Id == setInternalId);

                    if (elementSet != null)
                    {
                        if (IsValidElementId(elementId))
                        {
                            int elementInternalId = Convert.ToInt32(elementId.Remove(0, 3));
                            var elements = radElementDbContext.Element.ToList();
                            var element = elements.Find(x => x.Id == elementInternalId);

                            if (element != null)
                            {
                                ElementSetRef setRef = new ElementSetRef()
                                {
                                    ElementSetId = setInternalId,
                                    ElementId = elementInternalId
                                };

                                radElementDbContext.ElementSetRef.Add(setRef);
                                radElementDbContext.SaveChanges();

                                return await Task.FromResult(new JsonResult(string.Format("Element with set id {0} and element id {1} is mapped.", setId, elementId), HttpStatusCode.Created));
                            }
                        }

                        return await Task.FromResult(new JsonResult(string.Format("No such element with id '{0}'.", elementId), HttpStatusCode.NotFound));
                    }
                }

                return await Task.FromResult(new JsonResult(string.Format("No such set with id '{0}'.", setId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'CreateElement(int setId, DataElementType elementType, CreateUpdateElement dataElement)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="dataElement">The data element.</param>
        /// <returns></returns>
        public async Task<JsonResult> UpdateElement(string setId, string elementId, CreateUpdateElement dataElement)
        {
            try
            {
                if (IsValidSetId(setId) && IsValidElementId(elementId))
                {
                    int setInternalId = Convert.ToInt32(setId.Remove(0, 4));
                    int elementInternalId = Convert.ToInt32(elementId.Remove(0, 3));

                    if (dataElement == null)
                    {
                        return await Task.FromResult(new JsonResult("Dataelement fields are invalid in request", HttpStatusCode.BadRequest));
                    }

                    if (dataElement.ValueType == DataElementType.Choice || dataElement.ValueType == DataElementType.MultiChoice)
                    {
                        if (dataElement.Options == null || dataElement.Options.Count == 0)
                        {
                            return await Task.FromResult(new JsonResult("'Options' field is missing for Choice type elements in request", HttpStatusCode.BadRequest));
                        }
                    }

                    var elementSets = radElementDbContext.ElementSet.ToList();
                    var elementSet = elementSets.Find(x => x.Id == setInternalId);

                    if (elementSet != null)
                    {
                        var element = radElementDbContext.Element.ToList().Find(x => x.Id == elementInternalId);
                        if (element != null)
                        {
                            var elementValues = radElementDbContext.ElementValue.ToList().Where(x => x.ElementId == element.Id);
                            if (elementValues != null && elementValues.Any())
                            {
                                radElementDbContext.ElementValue.RemoveRange(elementValues);
                            }

                            element.Name = dataElement.Label;
                            element.ShortName = dataElement.ShortName ?? string.Empty;
                            element.Definition = dataElement.Definition ?? string.Empty;
                            element.ValueType = GetElementValueType(dataElement.ValueType);
                            element.MinCardinality = 1;
                            element.MaxCardinality = (dataElement.ValueType == DataElementType.MultiChoice) ? (uint)dataElement.Options.Count : (uint)1;
                            element.Unit = dataElement.Unit ?? string.Empty;
                            element.Question = dataElement.Question ?? dataElement.Label;
                            element.Instructions = dataElement.Instructions ?? string.Empty;
                            element.References = dataElement.References ?? string.Empty;
                            element.Version = dataElement.Version ?? "";
                            element.VersionDate = DateTime.Now;
                            element.Synonyms = dataElement.Synonyms ?? string.Empty;
                            element.Source = dataElement.Source ?? string.Empty;
                            element.Status = "Proposed";
                            element.StatusDate = DateTime.Now;
                            element.Editor = dataElement.Editor ?? string.Empty;
                            element.Modality = dataElement.Modality != null && dataElement.Modality.Any() ? string.Join(",", dataElement.Modality) : null;
                            element.BiologicalSex = dataElement.BiologicalSex != null && dataElement.BiologicalSex.Any() ? string.Join(",", dataElement.BiologicalSex) : null;
                            element.AgeLowerBound = dataElement.AgeLowerBound;
                            element.AgeUpperBound = dataElement.AgeUpperBound;
                            element.ValueSize = 0;

                            if (dataElement.ValueType == DataElementType.Integer)
                            {
                                element.ValueMin = dataElement.ValueMin;
                                element.ValueMax = dataElement.ValueMax;
                                element.StepValue = 1;
                            }

                            if (dataElement.ValueType == DataElementType.Numeric)
                            {
                                float? minValue = null;
                                float? maxValue = null;

                                if (dataElement.ValueMin.HasValue)
                                {
                                    minValue = Convert.ToSingle(dataElement.ValueMin.Value);
                                }

                                if (dataElement.ValueMax.HasValue)
                                {
                                    maxValue = Convert.ToSingle(dataElement.ValueMax.Value);
                                }

                                element.ValueMin = minValue;
                                element.ValueMax = maxValue;
                                element.StepValue = 0.1f;
                            }

                            if (dataElement.ValueType == DataElementType.MultiChoice || dataElement.ValueType == DataElementType.Choice)
                            {
                                AddElementValues(dataElement.Options, element.Id);
                            }

                            radElementDbContext.SaveChanges();

                            return await Task.FromResult(new JsonResult(string.Format("Element with set id {0} and element id {1} is updated.", setId, elementId), HttpStatusCode.OK));
                        }
                    }
                }

                return await Task.FromResult(new JsonResult(string.Format("No such element with set id {0} and element id {1}.", setId, elementId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'UpdateElement(int setId, int elementId, DataElementType elementType, CreateUpdateElement dataElement)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Deletes the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> DeleteElement(string setId, string elementId)
        {
            try
            {
                if (IsValidSetId(setId) && IsValidElementId(elementId))
                {
                    int setInternalId = Convert.ToInt32(setId.Remove(0, 4));
                    int elementInternalId = Convert.ToInt32(elementId.Remove(0, 3));
                    var elementSetRefs = radElementDbContext.ElementSetRef.ToList();
                    var elementSetRef = elementSetRefs.Find(x => x.ElementSetId == setInternalId && x.ElementId == elementInternalId);

                    if (elementSetRef != null)
                    {
                        var elementValues = radElementDbContext.ElementValue.ToList().Where(x => x.ElementId == elementSetRef.ElementId);
                        var element = radElementDbContext.Element.ToList().Find(x => x.Id == elementSetRef.ElementId);

                        if (elementValues != null && elementValues.Any())
                        {
                            radElementDbContext.ElementValue.RemoveRange(elementValues);
                        }

                        if (element != null)
                        {
                            radElementDbContext.Element.Remove(element);
                        }

                        radElementDbContext.ElementSetRef.Remove(elementSetRef);
                        radElementDbContext.SaveChanges();

                        return await Task.FromResult(new JsonResult(string.Format("Element with set id {0} and element id {1} is deleted.", setId, elementId), HttpStatusCode.OK));
                    }
                }
                return await Task.FromResult(new JsonResult(string.Format("No such element with set id {0} and element id {1}.", setId, elementId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'DeleteElement(int setId, int elementId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Adds the element values.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="elementId">The element identifier.</param>
        private void AddElementValues(List<Option> options, uint elementId)
        {
            foreach (Option option in options)
            {
                ElementValue elementvalue = new ElementValue()
                {
                    ElementId = elementId,
                    Value = option.Value ?? "",
                    Name = option.Name ?? "",
                    Definition = option.Definition ?? "",
                    Images = option.Images ?? ""
                };

                radElementDbContext.ElementValue.Add(elementvalue);
            }
        }

        /// <summary>
        /// Determines whether [is valid element identifier] [the specified element identifier].
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is valid element identifier] [the specified element identifier]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidElementId(string elementId)
        {
            if (elementId.Length > 3 && string.Equals(elementId.Substring(0, 3), "RDE", StringComparison.OrdinalIgnoreCase))
            {
                bool result = int.TryParse(elementId.Remove(0, 3), out _);
                return result;
            }

            return false;
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
        /// Gets the element details dto.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private object GetElementDetailsDto(object element)
        {
            if (element.GetType() == typeof(List<Element>))
            {
                var elements = mapper.Map<List<Element>, List<ElementDetails>>(element as List<Element>);
                elements.ForEach(element =>
                {
                    element.ElementValues = element.ValueType == "valueSet" ? radElementDbContext.ElementValue.ToList().Where(x => x.ElementId == element.Id).ToList() : null;
                });

                return elements;
            }
            else if (element.GetType() == typeof(Element))
            {
                var elementDetails = mapper.Map<ElementDetails>(element as Element);
                elementDetails.ElementValues = elementDetails.ValueType == "valueSet" ? radElementDbContext.ElementValue.ToList().Where(x => x.ElementId == (element as Element).Id).ToList() : null;

                return elementDetails;
            }

            return null;
        }

        /// <summary>
        /// Gets the type of the element value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private string GetElementValueType(DataElementType? type)
        {
            var valueType = string.Empty;

            switch (type)
            {
                case DataElementType.Choice:
                case DataElementType.MultiChoice:
                    valueType = "valueSet";
                    break;

                case DataElementType.Integer:
                    valueType = "integer";
                    break;

                case DataElementType.Numeric:
                    valueType = "float";
                    break;

                case DataElementType.DateTime:
                    valueType = "date";
                    break;

                case DataElementType.String:
                    valueType = "string";
                    break;
            }

            return valueType;
        }
    }
}
