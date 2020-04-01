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

namespace RadElement.Service
{
    /// <summary>
    /// Business service for handling the element related operations
    /// </summary>
    /// <seealso cref="RadElement.Core.Services.IElementService" />
    public class ElementService : IElementService
    {
        private IRadElementDbContext radElementDbContext;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementService" /> class.
        /// </summary>
        /// <param name="radElementDbContext">The RAD element database context.</param>
        /// <param name="logger">The logger.</param>
        public ElementService(IRadElementDbContext radElementDbContext, ILogger logger)
        {
            this.radElementDbContext = radElementDbContext;
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
                return await Task.FromResult(new JsonResult(GetElementDetailsArrayDto(elements), HttpStatusCode.OK));
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
                    int id = Convert.ToInt32(elementId.Remove(0, 3));
                    var elements = radElementDbContext.Element.ToList();
                    var element = elements.Find(x => x.Id == id);

                    if (element != null)
                    {
                        return await Task.FromResult(new JsonResult(GetElementDetailsDto(element), HttpStatusCode.OK));
                    }
                }

                return await Task.FromResult(new JsonResult(string.Format("No such element with id '{0}'", elementId), HttpStatusCode.NotFound));
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
                    int id = Convert.ToInt32(setId.Remove(0, 4));
                    var setRefs = radElementDbContext.ElementSetRef.ToList();
                    var elementIds = setRefs.FindAll(x => x.ElementSetId == id);
                    var elements = radElementDbContext.Element.ToList();

                    var selectedElements = from elemetId in elementIds
                                           join element in elements on elemetId.ElementId equals (int)element.Id
                                           select element;

                    if (selectedElements != null && selectedElements.Any())
                    {
                        return await Task.FromResult(new JsonResult(GetElementDetailsArrayDto(selectedElements.ToList()), HttpStatusCode.OK));
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
        public async Task<JsonResult> SearchElement(string searchKeyword)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    var elements = radElementDbContext.Element.ToList();
                    var elems = GetElementDetailsArrayDto(elements);
                    var filteredElements = elems.FindAll(x => x.ElementId.ToString().ToLower().Contains(searchKeyword.ToLower()) ||
                                                            x.Definition.ToLower().Contains(searchKeyword.ToLower()) ||
                                                            x.Editor.ToLower().Contains(searchKeyword.ToLower()) ||
                                                            x.Instructions.ToLower().Contains(searchKeyword.ToLower()) ||
                                                            x.Name.ToLower().Contains(searchKeyword.ToLower()) ||
                                                            x.Question.ToLower().Contains(searchKeyword.ToLower()) ||
                                                            x.References.ToLower().Contains(searchKeyword.ToLower()) ||
                                                            x.ShortName.ToLower().Contains(searchKeyword.ToLower()) ||
                                                            x.Source.ToLower().Contains(searchKeyword.ToLower()) ||
                                                            x.Synonyms.ToLower().Contains(searchKeyword.ToLower()));
                    if (filteredElements != null && filteredElements.Any())
                    {
                        return await Task.FromResult(new JsonResult(filteredElements, HttpStatusCode.OK));
                    }
                    else
                    {
                        return await Task.FromResult(new JsonResult(string.Format("No such element with keyword '{0}'.", searchKeyword), HttpStatusCode.NotFound));
                    }
                }
                else
                {
                    return await Task.FromResult(new JsonResult(string.Format("Keyword '{0}' given is invalid", searchKeyword), HttpStatusCode.BadRequest));
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
            int elementId = 0;
            try
            {
                if (IsValidSetId(setId))
                {
                    int id = Convert.ToInt32(setId.Remove(0, 4));
                    if (dataElement == null)
                    {
                        return await Task.FromResult(new JsonResult("Dataelement fields are invalid in request", HttpStatusCode.BadRequest));
                    }

                    if (dataElement.ValueType == DataElementType.Choice || dataElement.ValueType == DataElementType.MultiChoice)
                    {
                        if (dataElement.Options == null)
                        {
                            return await Task.FromResult(new JsonResult("'Options' field is missing for Choice type elements in request", HttpStatusCode.BadRequest));
                        }
                    }

                    var elementSets = radElementDbContext.ElementSet.ToList();
                    var elementSet = elementSets.Find(x => x.Id == id);

                    if (elementSet != null)
                    {
                        Element element = new Element()
                        {
                            Name = dataElement.Name,
                            ShortName = dataElement.ShortName ?? string.Empty,
                            Definition = dataElement.Definition ?? string.Empty,
                            ValueType = GetElementValueType(dataElement.ValueType),
                            MinCardinality = 1,
                            MaxCardinality = (dataElement.ValueType == DataElementType.MultiChoice) ? (short)dataElement.Options.Count : (short)1,
                            Unit = dataElement.Unit ?? string.Empty,
                            Question = dataElement.Question ?? dataElement.Name,
                            Instructions = dataElement.Instructions ?? string.Empty,
                            References = dataElement.References ?? string.Empty,
                            Version = dataElement.Version ?? "",
                            VersionDate = DateTime.Now,
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

                        elementId = (int)element.Id;

                        if (dataElement.ValueType == DataElementType.MultiChoice || dataElement.ValueType == DataElementType.Choice)
                        {
                            AddElementValues(dataElement.Options, element.Id);
                        }

                        ElementSetRef setRef = new ElementSetRef()
                        {
                            ElementSetId = id,
                            ElementId = (short)elementId
                        };

                        radElementDbContext.ElementSetRef.Add(setRef);
                        radElementDbContext.SaveChanges();
                        return await Task.FromResult(new JsonResult(new ElementIdDetails() { ElementId = "RDE" + elementId.ToString() }, HttpStatusCode.Created));
                    }
                }

                return await Task.FromResult(new JsonResult(string.Format("No such element with set id {0}.", setId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                if (elementId != 0)
                {
                    var response = DeleteElement(setId, "RDE" + elementId.ToString());
                }

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
                    int id = Convert.ToInt32(setId.Remove(0, 4));
                    int elemId = Convert.ToInt32(elementId.Remove(0, 3));

                    if (dataElement == null)
                    {
                        return await Task.FromResult(new JsonResult("Dataelement fields are invalid in request", HttpStatusCode.BadRequest));
                    }

                    if (dataElement.ValueType == DataElementType.Choice || dataElement.ValueType == DataElementType.MultiChoice)
                    {
                        if (dataElement.Options == null)
                        {
                            return await Task.FromResult(new JsonResult("'Options' field is missing for Choice type elements in request", HttpStatusCode.BadRequest));
                        }
                    }

                    var elementSets = radElementDbContext.ElementSet.ToList();
                    var elementSet = elementSets.Find(x => x.Id == id);

                    if (elementSet != null)
                    {
                        var element = radElementDbContext.Element.ToList().Find(x => x.Id == elemId);
                        if (element != null)
                        {
                            var elementValues = radElementDbContext.ElementValue.ToList().FindAll(x => x.ElementId == element.Id);
                            if (elementValues != null && elementValues.Any())
                            {
                                radElementDbContext.ElementValue.RemoveRange(elementValues);
                            }

                            element.Name = dataElement.Name;
                            element.ShortName = dataElement.ShortName ?? string.Empty;
                            element.Definition = dataElement.Definition ?? string.Empty;
                            element.ValueType = GetElementValueType(dataElement.ValueType);
                            element.MinCardinality = 1;
                            element.MaxCardinality = (dataElement.ValueType == DataElementType.MultiChoice) ? (short)dataElement.Options.Count : (short)1;
                            element.Unit = dataElement.Unit ?? string.Empty;
                            element.Question = dataElement.Question ?? dataElement.Name;
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
                    int id = Convert.ToInt32(setId.Remove(0, 4));
                    int elemId = Convert.ToInt32(elementId.Remove(0, 3));
                    var elementSetRefs = radElementDbContext.ElementSetRef.ToList();
                    var elementSetRef = elementSetRefs.Find(x => x.ElementSetId == id && x.ElementId == elemId);

                    if (elementSetRef != null)
                    {
                        var elementValues = radElementDbContext.ElementValue.ToList().FindAll(x => x.ElementId == elementSetRef.ElementId);
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
                int id;
                bool result = int.TryParse(elementId.Remove(0, 3), out id);
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
            if (setId.Length > 4 && string.Equals(setId.Substring(0, 4), "RDES", StringComparison.InvariantCulture))
            {
                int id;
                bool result = int.TryParse(setId.Remove(0, 4), out id);
                return result;
            }

            return false;
        }

        /// <summary>
        /// Gets the element details array dto.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        private List<ElementDetails> GetElementDetailsArrayDto(List<Element> elements)
        {
            List<ElementDetails> elementDetails = new List<ElementDetails>();
            foreach (var element in elements)
            {
                elementDetails.Add(GetElementDetailsDto(element));
            }

            return elementDetails;
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

        /// <summary>
        /// Gets the element details dto.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private ElementDetails GetElementDetailsDto(Element element)
        {
            return new ElementDetails()
            {
                Id = element.Id,
                ElementId = "RDE" + element.Id,
                Definition = element.Definition,
                Editor = element.Editor,
                Instructions = element.Instructions,
                MaxCardinality = element.MaxCardinality,
                MinCardinality = element.MinCardinality,
                Name = element.Name,
                Question = element.Question,
                References = element.References,
                ShortName = element.ShortName,
                Source = element.Source,
                Status = element.Status,
                StatusDate = element.StatusDate,
                StepValue = element.StepValue,
                Synonyms = element.Synonyms,
                Unit = element.Unit,
                ValueMax = element.ValueMax,
                ValueMin = element.ValueMin,
                ValueSize = element.ValueSize,
                ValueType = element.ValueType,
                Version = element.Version,
                VersionDate = element.VersionDate,
                ElementValues = element.ValueType == "valueSet" ? radElementDbContext.ElementValue.ToList().FindAll(x => x.ElementId == element.Id) : null
            };
        }
    }
}
