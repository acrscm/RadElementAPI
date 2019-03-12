using RadElement.Core.Domain;
using RadElement.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace RadElement.Service.Tests.Mocks.Data
{
    public class MockElementDataContext
    {
        #region Data

        static List<Element> elementsDB = new List<Element>()
            {
                new Element { Id = 340, Name = "Tumuor bed size change", ShortName = "", Definition = "Status of the tumor bed", ValueType = "valueSet",
                              ValueSize = 0, MinCardinality = 1, MaxCardinality = 1, Unit = "", Question = "Tumuor bed size change", Instructions = "",
                              References = "", Version = "1", VersionDate = DateTime.Now, Synonyms = "", Source = "DSI TOUCH-AI", Status = "Proposed",
                              StatusDate = DateTime.Now, Editor = "" },
                new Element { Id = 338, Name = "Approximate deployment angle", ShortName = "", Definition = "Angle to optimize visualization of the 3 cusps of the aortic valve (degrees)", ValueType = "float",
                              ValueSize = 0, StepValue = 1, MinCardinality = 1, MaxCardinality = 1, Unit = "", Question = "Approximate deployment angle", Instructions = "",
                              References = "", Version = "1", VersionDate = DateTime.Now, Synonyms = "", Source = "DSI TOUCH-AI", Status = "Proposed",
                              StatusDate = DateTime.Now, Editor = "" },
                new Element { Id = 307, Name = "Myocardial wall thickening", ShortName = "", Definition = "The  percent  myocardial  thickness  increase during  end  systole  relative  to end diastole (%)", ValueType = "integer",
                              ValueSize = 0, StepValue = 1, MinCardinality = 1, MaxCardinality = 1, Unit = "", Question = "Myocardial wall thickening", Instructions = "",
                              References = "", Version = "1", VersionDate = DateTime.Now, Synonyms = "", Source = "DSI TOUCH-AI", Status = "Proposed",
                              StatusDate = DateTime.Now, Editor = "" },
                  new Element { Id = 283, Name = "Presence of Valvular Calcifications", ShortName = "", Definition = "Determine the presence of valvular calcifications", ValueType = "valueSet",
                              ValueSize = 0, MinCardinality = 1, MaxCardinality = 1, Unit = "", Question = "Presence of Valvular Calcifications", Instructions = "",
                              References = "", Version = "1", VersionDate = DateTime.Now, Synonyms = "", Source = "DSI TOUCH-AI", Status = "Proposed",
                              StatusDate = DateTime.Now, Editor = "" },
            };

        static List<ElementSetRef> elementSetRefDb = new List<ElementSetRef>()
            {
                new ElementSetRef { Id = 351, ElementId = 283, ElementSetId = 53 },
                new ElementSetRef { Id = 375, ElementId = 307, ElementSetId = 66 },
                new ElementSetRef { Id = 406, ElementId = 338, ElementSetId = 72 },
                new ElementSetRef { Id = 408, ElementId = 340, ElementSetId = 74 }
            };

        static List<ElementSet> elementSetDb = new List<ElementSet>()
            {
                new ElementSet { Id = 53, Name = "Pulmonary Veins Mapping Preablation", Description = "This module describes the common data elements for pulmonary veins mapping preablation use case.",
                                 ContactName = "David Wymer, MD", Status = "Proposed" },
                new ElementSet { Id = 66, Name = "Left Ventricle Wall Thickening", Description = "This module describes the common elements for left ventricle wall thickening",
                                 ContactName = "Carlo De Cecco, MD, PhD", Status = "Proposed" },
                new ElementSet { Id = 72, Name = "TAVR Aortic Root Measurements", Description = "This module describes the common data elements for TAVR Aortic Root Measurements use case.",
                                 ContactName = "Kimberly Brockenbrough, MD", Status = "Proposed" },
                new ElementSet { Id = 74, Name = "Soft Tissue Tumor Bed Size Change", Description = "This module describes the common data elements for soft tissue tumor bed size change use case.",
                                 ContactName = "Jay Patti, MD", Status = "Proposed" },
            };

        static List<ElementValue> elementValueDb = new List<ElementValue>()
            {
                new ElementValue { Id = 1001, ElementId = 283, Value = "0", Name = "0 Unknown", Definition = "0 Unknown" },
                new ElementValue { Id = 1002, ElementId = 283, Value = "1", Name = "1 Present", Definition = "1 Present" },
                new ElementValue { Id = 1003, ElementId = 283, Value = "2", Name = "2 Absent", Definition = "2 Absent" },
                new ElementValue { Id = 1007, ElementId = 304, Value = "0", Name = "0 No change in size", Definition = "0 No change in size" },
                new ElementValue { Id = 1008, ElementId = 340, Value = "1", Name = "1 Increase in size", Definition = "1 Increase in size" },
                new ElementValue { Id = 1009, ElementId = 340, Value = "2", Name = "2 Decrease in size", Definition = "2 Decrease in size" },
        };

        #endregion

        #region Elements

        public static JsonResult GetElements()
        {
            return new JsonResult(elementsDB.ToList(), HttpStatusCode.OK);
        }

        public static JsonResult GetElement(int elementId)
        {
            var element = elementsDB.ToList().Find(x => x.Id == elementId);
            if (element != null)
            {
                return new JsonResult(element, HttpStatusCode.OK);
            }
            else
            {
                return new JsonResult(string.Format("No such element with id '{0}'", elementId), HttpStatusCode.NotFound);
            }
        }

        public static JsonResult GetElementsBySetId(int setId)
        {
            var elementIds = elementSetRefDb.ToList().FindAll(x => x.ElementSetId == setId);

            var selectedElements = from elemetId in elementIds
                                   join element in elementsDB.ToList() on elemetId.ElementId equals (int)element.Id
                                   select element;

            if (selectedElements != null && selectedElements.Any())
            {
                return new JsonResult(selectedElements.ToList(), HttpStatusCode.OK);
            }
            else
            {
                return new JsonResult(string.Format("No such elements with set id '{0}'.", setId), HttpStatusCode.NotFound);
            }
        }

        public static JsonResult SearchElement(string searchKeyword)
        {
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                var filteredElements = elementsDB.ToList().FindAll(x => x.Definition.ToLower().Contains(searchKeyword.ToLower()) || x.Editor.ToLower().Contains(searchKeyword.ToLower()) ||
                                             x.Instructions.ToLower().Contains(searchKeyword.ToLower()) || x.Name.ToLower().Contains(searchKeyword.ToLower()) ||
                                             x.Question.ToLower().Contains(searchKeyword.ToLower()) || x.References.ToLower().Contains(searchKeyword.ToLower()) ||
                                             x.ShortName.ToLower().Contains(searchKeyword.ToLower()) || x.Source.ToLower().Contains(searchKeyword.ToLower()) ||
                                             x.Synonyms.ToLower().Contains(searchKeyword.ToLower()));
                if (filteredElements != null && filteredElements.Any())
                {
                    return new JsonResult(filteredElements.ToList(), HttpStatusCode.OK);
                }
                else
                {
                    return new JsonResult(string.Format("No such element with keyword '{0}'.", searchKeyword), HttpStatusCode.NotFound);
                }
            }
            else
            {
                return new JsonResult(string.Format("Keyword '{0}' given is invalid", searchKeyword), HttpStatusCode.BadRequest);
            }
        }

        public static JsonResult CreateElement(int setId, DataElementType elementType, CreateUpdateElement dataElement)
        {
            if (dataElement == null)
            {
                return new JsonResult("Dataelement fields are invalid in request", HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrEmpty(dataElement.Label))
            {
                return new JsonResult("'Label' field is missing in request", HttpStatusCode.BadRequest);
            }

            if (elementType == DataElementType.Choice || elementType == DataElementType.MultiChoice)
            {
                if (dataElement.Options == null)
                {
                    return new JsonResult("'Options' field is missing for Choice type elements in request", HttpStatusCode.BadRequest);
                }
            }

            int elementId = 0;
            var elementSet = elementSetDb.ToList().Find(x => x.Id == setId);

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
                    element.Id = 1000;

                    elementsDB.Add(element);
                    elementId = (int)element.Id;

                    if (elementType == DataElementType.MultiChoice || elementType == DataElementType.Choice)
                    {
                        AddElementValues(dataElement.Options, element.Id);
                    }
                }

                ElementSetRef setRef = new ElementSetRef()
                {
                    ElementSetId = setId,
                    ElementId = (short)elementId
                };

                elementSetRefDb.Add(setRef);
            }

            if (elementId != 0)
            {
                return new JsonResult(new ElementIdDetails() { ElementId = elementId.ToString() }, HttpStatusCode.Created);
            }
            else
            {
                return new JsonResult(new ElementIdDetails() { ElementId = elementId.ToString() }, HttpStatusCode.BadRequest);
            }
        }

        public static JsonResult UpdateElement(int setId, int elementId, DataElementType elementType, CreateUpdateElement dataElement)
        {
            if (dataElement == null)
            {
                return new JsonResult("Dataelement fields are invalid in request", HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrEmpty(dataElement.Label))
            {
                return new JsonResult("'Label' field is missing in request", HttpStatusCode.BadRequest);
            }

            if (elementType == DataElementType.Choice || elementType == DataElementType.MultiChoice)
            {
                if (dataElement.Options == null)
                {
                    return new JsonResult("'Options' field is missing for Choice type elements in request", HttpStatusCode.BadRequest);
                }
            }

            var elementSet = elementSetDb.ToList().Find(x => x.Id == setId);

            if (elementSet != null)
            {
                var element = elementsDB.ToList().Find(x => x.Id == elementId);
                if (element != null)
                {
                    var elementValues = elementValueDb.ToList().FindAll(x => x.ElementId == element.Id);
                    if (elementValues != null && elementValues.Any())
                    {
                        foreach (var value in elementValues)
                        {
                            elementValueDb.Remove(value);
                        }
                        if (elementType == DataElementType.Choice || elementType == DataElementType.MultiChoice)
                        {
                            AddElementValues(dataElement.Options, element.Id);
                        }
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

                    return new JsonResult(string.Format("Element with set id {0} and element id {1} is updated.", setId, elementId), HttpStatusCode.OK);
                }
            }

            return new JsonResult(string.Format("No such element with set id {0} and element id {1}.", setId, elementId), HttpStatusCode.NotFound);
        }

        public static JsonResult DeleteElement(int setId, int elementId)
        {
            var elementSetRef = elementSetRefDb.ToList().Find(x => x.ElementSetId == setId && x.ElementId == elementId);

            if (elementSetRef != null)
            {
                var elementValues = elementValueDb.ToList().FindAll(x => x.ElementId == elementSetRef.ElementId);
                var element = elementsDB.ToList().Find(x => x.Id == elementSetRef.ElementId);

                if (elementValues != null && elementValues.Any())
                {
                    foreach (var value in elementValues)
                    {
                        elementValueDb.Remove(value);
                    }
                }

                if (element != null)
                {
                    elementsDB.Remove(element);
                }

                elementSetRefDb.Remove(elementSetRef);
                return new JsonResult(string.Format("Element with set id {0} and element id {1} is deleted.", setId, elementId), HttpStatusCode.OK);

            }

            return new JsonResult(string.Format("No such element with set id {0} and element id {1}.", setId, elementId), HttpStatusCode.NotFound);
        }

        #endregion
        
        #region Private methods

        private static void AddElementValues(List<Core.DTO.Option> options, uint elementId)
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

                elementValueDb.Add(elementvalue);
            }
        }

        #endregion
    }
}
