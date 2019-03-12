using RadElement.Core.Domain;
using RadElement.Core.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace RadElement.Service.Tests.Mocks.Data
{
    public class MockModuleDataContext
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

        #region Module

        public static JsonResult CreateModule(XmlElement xmlContent)
        {
            SetIdDetails idDetails = new SetIdDetails(); ;
            var assistModule = GetDeserializedDataFromXml(xmlContent.OuterXml);
            if (assistModule != null)
            {
                List<int> elementIds = AddElements(assistModule.DataElements);
                idDetails.SetId = AddElementSet(assistModule);
                AddSetRef(Int16.Parse(idDetails.SetId), elementIds);

                if (string.IsNullOrEmpty(idDetails.SetId))
                {
                    return new JsonResult(new SetIdDetails() { SetId = idDetails.SetId }, HttpStatusCode.Created);
                }
                else
                {
                    return new JsonResult(new SetIdDetails() { SetId = idDetails.SetId }, HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return new JsonResult("Xml content provided is invalid", HttpStatusCode.BadRequest);
            }
        }

        public static JsonResult UpdateModule(XmlElement xmlContent, int setId)
        {
            var module = GetDeserializedDataFromXml(xmlContent.OuterXml);
            if (module != null)
            {
                var elementSet = elementSetDb.ToList().Find(x => x.Id == setId);

                if (elementSet != null)
                {
                    string desc = string.Empty;
                    string contact = string.Empty;

                    if (module.MetaData.Info != null && !string.IsNullOrEmpty(module.MetaData.Info.Description))
                    {
                        desc = module.MetaData.Info.Description;
                    }
                    if (module.MetaData.Info.Contact != null && !string.IsNullOrEmpty(module.MetaData.Info.Contact.Name))
                    {
                        contact = module.MetaData.Info.Contact.Name;
                    }

                    elementSet.Name = module.ModuleName.Replace("_", " ");
                    elementSet.Description = desc;
                    elementSet.ContactName = contact;

                    var elementSetRefs = elementSetRefDb.ToList().FindAll(x => x.ElementSetId == setId);
                    if (elementSetRefs != null && elementSetRefs.Any())
                    {
                        foreach (var eleref in elementSetRefs.ToList())
                        {
                            var elementValues = elementValueDb.ToList().FindAll(x => x.ElementId == eleref.ElementId);
                            var element = elementsDB.ToList().Find(x => x.Id == eleref.ElementId);

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

                            elementSetRefDb.Remove(eleref);
                        }
                    }

                    List<int> elementIds = AddElements(module.DataElements);
                    AddSetRef(setId, elementIds);
                    return new JsonResult(string.Format("Set with id {0} is updated.", setId), HttpStatusCode.OK);
                }

                return new JsonResult(string.Format("No such set with id '{0}'", setId), HttpStatusCode.NotFound);
            }
            else
            {
                return new JsonResult("Xml content provided is invalid", HttpStatusCode.BadRequest);
            }
        }

        #endregion

        #region Private methods

        private static List<int> AddElements(List<DataElement> dataElements)
        {
            List<int> elementIds = new List<int>();
            foreach (DataElement dataElement in dataElements)
            {
                if (dataElement.DataElementType == DataElementType.Global)
                {
                    continue;
                }

                int elementId = (int)AddElement(dataElement);
                elementIds.Add(elementId);
            }

            return elementIds;
        }

        private static uint AddElement(DataElement data)
        {
            Element element = new Element()
            {
                Name = data.Label,
                ShortName = "",
                Definition = data.HintText ?? "",
                MaxCardinality = 1,
                MinCardinality = 1,
                Source = "DSI TOUCH-AI",
                Status = "Proposed",
                StatusDate = DateTime.Now,
                Editor = "",
                Instructions = "",
                Question = data.Label ?? "",
                References = "",
                Synonyms = "",
                VersionDate = DateTime.Now,
                Version = "1",
                ValueSize = 0,
                Unit = ""
            };

            if (data is IntegerElement)
            {
                IntegerElement intElement = data as IntegerElement;
                element.ValueType = "integer";
                element.ValueMin = intElement.MinimumValue;
                element.ValueMax = intElement.MaximumValue;
                element.StepValue = 1;
            }

            if (data.DataElementType == DataElementType.Choice)
            {
                ChoiceElement choiceElement = data as ChoiceElement;
                element.ValueType = "valueSet";
            }

            if (data.DataElementType == DataElementType.MultiChoice)
            {
                MultipleChoiceElement choiceElement = data as MultipleChoiceElement;
                element.ValueType = "valueSet";
                element.MaxCardinality = (short)choiceElement.Options.Count;
            }

            if (data is NumericElement)
            {
                NumericElement numericElement = data as NumericElement;
                float? minValue = null;
                if (numericElement.MinimumValue.HasValue)
                {
                    minValue = Convert.ToSingle(numericElement.MinimumValue.Value);
                }
                float? maxValue = null;
                if (numericElement.MaximumValue.HasValue)
                {
                    maxValue = Convert.ToSingle(numericElement.MaximumValue.Value);
                }
                element.ValueType = "float";
                element.ValueMin = minValue;
                element.ValueMax = maxValue;
                element.StepValue = 0.1f;
            }

            elementsDB.Add(element);
            AddElementValues(data, element.Id);

            return element.Id;
        }

        private static void AddElementValues(DataElement data, uint elementId)
        {
            var options = new List<Core.DTO.Option>();
            if (data.DataElementType == DataElementType.MultiChoice)
            {
                var elem = data as MultipleChoiceElement;
                options = elem.Options;
            }

            if (data.DataElementType == DataElementType.Choice)
            {
                var elem = data as ChoiceElement;
                options = elem.Options;
            }

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

        private static string AddElementSet(AssistModule module)
        {
            string desc = string.Empty;
            string contact = string.Empty;

            if (module.MetaData.Info != null && !string.IsNullOrEmpty(module.MetaData.Info.Description))
            {
                desc = module.MetaData.Info.Description;
            }
            if (module.MetaData.Info.Contact != null && !string.IsNullOrEmpty(module.MetaData.Info.Contact.Name))
            {
                contact = module.MetaData.Info.Contact.Name;
            }

            ElementSet set = new ElementSet()
            {
                Name = module.ModuleName.Replace("_", " "),
                Description = desc,
                ContactName = contact,
            };

            elementSetDb.Add(set);
            return set.Id.ToString();
        }

        private static bool UpdateElementSet(int setId, CreateUpdateSet content)
        {
            var elementSet = elementSetDb.ToList().Find(x => x.Id == setId);

            if (elementSet != null)
            {
                elementSet.Name = content.ModuleName.Replace("_", " ");
                elementSet.Description = content.Description;
                elementSet.ContactName = content.ContactName;
                return true;
            }

            return false;
        }

        private static bool DeleteElementSet(ElementSet elementSet)
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
            return true;
        }

        private static void AddSetRef(int setId, List<int> elementIds)
        {
            foreach (short elementid in elementIds)
            {
                ElementSetRef setRef = new ElementSetRef()
                {
                    ElementSetId = setId,
                    ElementId = elementid
                };

                elementSetRefDb.Add(setRef);
            }
        }

        private static AssistModule GetDeserializedDataFromXml(string content)
        {
            ModuleDetails details = null;
            XmlSerializer serializer = null;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(content);
            StringBuilder xmlStringBuilder = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(xmlStringBuilder))
            {
                xmldoc.Save(writer);
            }
            using (StringReader reader = new StringReader(xmlStringBuilder.ToString()))
            {
                details = new ModuleDetails();
                serializer = new XmlSerializer(typeof(ReportingModule));
                details.Module = serializer.Deserialize(reader) as ReportingModule;
            }

            var assistModule = new AssistModuleHelper(details).ConvertToAssistModule();
            return assistModule;
        }

        #endregion
    }
}
