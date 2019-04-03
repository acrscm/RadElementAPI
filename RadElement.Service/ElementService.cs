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
                    var filteredElements = elements.FindAll(x => x.Definition.ToLower().Contains(searchKeyword.ToLower()) || x.Editor.ToLower().Contains(searchKeyword.ToLower()) ||
                                                 x.Instructions.ToLower().Contains(searchKeyword.ToLower()) || x.Name.ToLower().Contains(searchKeyword.ToLower()) ||
                                                 x.Question.ToLower().Contains(searchKeyword.ToLower()) || x.References.ToLower().Contains(searchKeyword.ToLower()) ||
                                                 x.ShortName.ToLower().Contains(searchKeyword.ToLower()) || x.Source.ToLower().Contains(searchKeyword.ToLower()) ||
                                                 x.Synonyms.ToLower().Contains(searchKeyword.ToLower()));
                    if (filteredElements != null && filteredElements.Any())
                    {
                        return await Task.FromResult(new JsonResult(GetElementDetailsArrayDto(filteredElements.ToList()), HttpStatusCode.OK));
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
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<JsonResult> CreateElement(string setId, DataElementType elementType, CreateUpdateElement dataElement)
        {
            try
            {
                if (IsValidSetId(setId))
                {
                    int id = Convert.ToInt32(setId.Remove(0, 4));
                    if (dataElement == null)
                    {
                        return await Task.FromResult(new JsonResult("Dataelement fields are invalid in request", HttpStatusCode.BadRequest));
                    }

                    if (string.IsNullOrEmpty(dataElement.Label))
                    {
                        return await Task.FromResult(new JsonResult("'Label' field is missing in request", HttpStatusCode.BadRequest));
                    }

                    if (elementType == DataElementType.Choice || elementType == DataElementType.MultiChoice)
                    {
                        if (dataElement.Options == null)
                        {
                            return await Task.FromResult(new JsonResult("'Options' field is missing for Choice type elements in request", HttpStatusCode.BadRequest));
                        }
                    }

                    int elementId = 0;
                    var elementSets = radElementDbContext.ElementSet.ToList();
                    var elementSet = elementSets.Find(x => x.Id == id);

                    if (elementSet != null)
                    {
                        if (elementType != DataElementType.Global)
                        {
                            Element element = new Element()
                            {
                                Name = dataElement.Label,
                                ShortName = "",
                                Definition = dataElement.Definition ?? "",
                                MaxCardinality = 1,
                                MinCardinality = 1,
                                Source = "DSI TOUCH-AI",
                                Status = "Proposed",
                                StatusDate = DateTime.Now,
                                Editor = "",
                                Instructions = "",
                                Question = dataElement.Label ?? "",
                                References = "",
                                Synonyms = "",
                                VersionDate = DateTime.Now,
                                Version = "1",
                                ValueSize = 0,
                                Unit = ""
                            };

                            if (elementType == DataElementType.Integer)
                            {
                                element.ValueType = "integer";
                                element.ValueMin = dataElement.ValueMin;
                                element.ValueMax = dataElement.ValueMax;
                                element.StepValue = 1;
                            }

                            if (elementType == DataElementType.Choice)
                            {
                                element.ValueType = "valueSet";
                            }

                            if (elementType == DataElementType.MultiChoice)
                            {
                                element.ValueType = "valueSet";
                                element.MaxCardinality = (short)dataElement.Options.Count;
                            }

                            if (elementType == DataElementType.Numeric)
                            {
                                float? minValue = null;
                                if (dataElement.ValueMin.HasValue)
                                {
                                    minValue = Convert.ToSingle(dataElement.ValueMin.Value);
                                }
                                float? maxValue = null;
                                if (dataElement.ValueMax.HasValue)
                                {
                                    maxValue = Convert.ToSingle(dataElement.ValueMax.Value);
                                }
                                element.ValueType = "float";
                                element.ValueMin = minValue;
                                element.ValueMax = maxValue;
                                element.StepValue = 0.1f;
                            }

                            radElementDbContext.Element.Add(element);
                            radElementDbContext.SaveChanges();

                            elementId = (int)element.Id;

                            if (elementType == DataElementType.MultiChoice || elementType == DataElementType.Choice)
                            {
                                AddElementValues(dataElement.Options, element.Id);
                            }
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
        public async Task<JsonResult> UpdateElement(string setId, string elementId, DataElementType elementType, CreateUpdateElement dataElement)
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

                    if (string.IsNullOrEmpty(dataElement.Label))
                    {
                        return await Task.FromResult(new JsonResult("'Label' field is missing in request", HttpStatusCode.BadRequest));
                    }

                    if (elementType == DataElementType.Choice || elementType == DataElementType.MultiChoice)
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

                            element.Name = dataElement.Label;
                            element.ShortName = "";
                            element.Definition = dataElement.Definition ?? "";
                            element.MaxCardinality = 1;
                            element.MinCardinality = 1;
                            element.Source = "DSI TOUCH-AI";
                            element.Status = "Proposed";
                            element.StatusDate = DateTime.Now;
                            element.Editor = "";
                            element.Instructions = "";
                            element.Question = dataElement.Label ?? "";
                            element.References = "";
                            element.Synonyms = "";
                            element.VersionDate = DateTime.Now;
                            element.Version = "1";
                            element.ValueSize = 0;
                            element.Unit = "";

                            if (elementType == DataElementType.Integer)
                            {
                                element.ValueType = "integer";
                                element.ValueMin = dataElement.ValueMin;
                                element.ValueMax = dataElement.ValueMax;
                                element.StepValue = 1;
                            }

                            if (elementType == DataElementType.Choice)
                            {
                                element.ValueType = "valueSet";
                            }

                            if (elementType == DataElementType.MultiChoice)
                            {
                                element.ValueType = "valueSet";
                                element.MaxCardinality = (short)dataElement.Options.Count;
                            }

                            if (elementType == DataElementType.Numeric)
                            {
                                float? minValue = null;
                                if (dataElement.ValueMin.HasValue)
                                {
                                    minValue = Convert.ToSingle(dataElement.ValueMin.Value);
                                }
                                float? maxValue = null;
                                if (dataElement.ValueMax.HasValue)
                                {
                                    maxValue = Convert.ToSingle(dataElement.ValueMax.Value);
                                }
                                element.ValueType = "float";
                                element.ValueMin = minValue;
                                element.ValueMax = maxValue;
                                element.StepValue = 0.1f;
                            }

                            if (elementType == DataElementType.MultiChoice || elementType == DataElementType.Choice)
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

        private void AddElementValues(List<Core.DTO.Option> options, uint elementId)
        {
            foreach (Core.DTO.Option option in options)
            {
                ElementValue elementvalue = new ElementValue()
                {
                    Name = option.Label,
                    Value = option.Value.Length > 32 ? option.Value.Substring(0, 32) : option.Value,
                    Definition = option.Label,
                    ElementId = elementId,
                };

                radElementDbContext.ElementValue.Add(elementvalue);
                radElementDbContext.SaveChanges();
            }
        }

        private bool IsValidElementId(string elementId)
        {
            if (elementId.Length > 3 && elementId.Substring(0, 3) == "RDE")
            {
                int id;
                bool result = Int32.TryParse(elementId.Remove(0, 3), out id);
                return result;
            }

            return false;
        }

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

        private List<ElementDetails> GetElementDetailsArrayDto(List<Element> elements)
        {
            List<ElementDetails> elementDetails = new List<ElementDetails>();
            foreach (var element in elements)
            {
                elementDetails.Add(GetElementDetailsDto(element));
            }

            return elementDetails;
        }

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
