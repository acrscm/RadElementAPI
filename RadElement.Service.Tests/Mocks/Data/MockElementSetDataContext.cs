using RadElement.Core.Domain;
using RadElement.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace RadElement.Service.Tests.Mocks.Data
{
    public class MockElementSetDataContext
    {
        #region Data

        static List<Element> elementsDB = new List<Element>()
            {
                new Element { Id = 340, Name = "Tumor bed size change", ShortName = "", Definition = "Status of the tumor bed", ValueType = "valueSet",
                              ValueSize = 0, MinCardinality = 1, MaxCardinality = 1, Unit = "", Question = "Tumor bed size change", Instructions = "",
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
        
        #region Element Set

        public static JsonResult GetSets()
        {
            return new JsonResult(elementSetDb.ToList(), HttpStatusCode.OK);
        }

        public static JsonResult GetSet(int setId)
        {
            var set = elementSetDb.ToList().Find(x => x.Id == setId);

            if (set != null)
            {
                return new JsonResult(set, HttpStatusCode.OK);
            }
            else
            {
                return new JsonResult(string.Format("No such set with id '{0}'", setId), HttpStatusCode.NotFound);
            }
        }

        public static JsonResult SearchSet(string searchKeyword)
        {
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                var filteredSets = elementSetDb.ToList().FindAll(x => x.Name.ToLower().Contains(searchKeyword.ToLower()) || x.Description.ToLower().Contains(searchKeyword.ToLower()) ||
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

        public static JsonResult CreateSet(CreateUpdateSet content)
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

            elementSetDb.Add(set);

            if (set.Id != 0)
            {
                return new JsonResult(new SetIdDetails() { SetId = set.Id.ToString() }, HttpStatusCode.Created);
            }
            else
            {
                return new JsonResult(new SetIdDetails() { SetId = set.Id.ToString() }, HttpStatusCode.BadRequest);
            }
        }

        public static JsonResult UpdateSet(int setId, CreateUpdateSet content)
        {
            if (content == null || string.IsNullOrEmpty(content.ModuleName) || string.IsNullOrEmpty(content.ContactName) || string.IsNullOrEmpty(content.Description))
            {
                return new JsonResult("Element set is invalid", HttpStatusCode.BadRequest);
            }

            var elementSet = elementSetDb.ToList().Find(x => x.Id == setId);

            if (elementSet != null)
            {
                elementSet.Name = content.ModuleName.Replace("_", " ");
                elementSet.Description = content.Description;
                elementSet.ContactName = content.ContactName;
                return new JsonResult(string.Format("Set with id {0} is updated.", setId), HttpStatusCode.OK);
            }

            return new JsonResult(string.Format("No such set with id {0}.", setId), HttpStatusCode.NotFound);
        }

        public static JsonResult DeleteSet(int setId)
        {
            var elementSet = elementSetDb.ToList().Find(x => x.Id == setId);

            if (elementSet != null)
            {
                var elementSetRefs = elementSetRefDb.ToList().FindAll(x => x.ElementSetId == elementSet.Id);
                if (elementSetRefs != null && elementSetRefs.Any())
                {
                    foreach (var setref in elementSetRefs.ToList())
                    {
                        var elementValues = elementValueDb.ToList().FindAll(x => x.ElementId == setref.ElementId);
                        var elements = elementsDB.ToList().FindAll(x => x.Id == setref.ElementId);

                        if (elementValues != null && elementValues.Any())
                        {
                            foreach (var value in elementValues)
                            {
                                elementValueDb.Remove(value);
                            }
                        }

                        if (elements != null && elements.Any())
                        {
                            foreach (var elem in elements)
                            {
                                elementsDB.Remove(elem);
                            }
                        }
                        elementSetRefDb.Remove(setref);
                    }
                }

                elementSetDb.Remove(elementSet);
                return new JsonResult(string.Format("Set with id {0} is deleted.", setId), HttpStatusCode.OK);
            }

            return new JsonResult(string.Format("No such set with id {0}.", setId), HttpStatusCode.NotFound);
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
