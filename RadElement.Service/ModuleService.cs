using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using RadElement.Core.Data;
using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Core.Services;
using Serilog;
using System.Net;

namespace RadElement.Service
{
    public class ModuleService : IModuleService
    {
        private IRadElementDbContext radElementDbContext;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleService"/> class.
        /// </summary>
        /// <param name="radElementDbContext">The RAD element database context.</param>
        public ModuleService(IRadElementDbContext radElementDbContext, ILogger logger)
        {
            this.radElementDbContext = radElementDbContext;
            this.logger = logger;
        }

        public async Task<JsonResult> CreateModule(XmlElement xmlContent)
        {
            try
            {
                SetIdDetails idDetails = new SetIdDetails();
                var assistModule = await GetDeserializedDataFromXml(xmlContent.OuterXml);
                if (assistModule != null)
                {
                    List<int> elementIds = AddElements(assistModule.DataElements);
                    idDetails.SetId = AddElementSet(assistModule);
                    AddSetRef(Int16.Parse(idDetails.SetId), elementIds);
                    return await Task.FromResult(new JsonResult(new SetIdDetails() { SetId = idDetails.SetId }, HttpStatusCode.Created));
                }
                else
                {
                    return await Task.FromResult(new JsonResult("Xml content provided is invalid", HttpStatusCode.BadRequest));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'CreateModule(XmlElement xmlContent)'");
                return await Task.FromResult(new JsonResult(ex, HttpStatusCode.InternalServerError));
            }
        }

        public async Task<JsonResult> UpdateModule(XmlElement xmlContent, int setId)
        {
            try
            {
                var module = await GetDeserializedDataFromXml(xmlContent.OuterXml);
                if (module != null)
                {
                    var elementSets = radElementDbContext.ElementSet.ToList();
                    var elementSet = elementSets.Find(x => x.Id == setId);

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
                        radElementDbContext.SaveChanges();

                        var elementSetRefs = radElementDbContext.ElementSetRef.ToList().FindAll(x => x.ElementSetId == setId);
                        if (elementSetRefs != null && elementSetRefs.Any())
                        {
                            foreach (var eleref in elementSetRefs.ToList())
                            {
                                var elementValues = radElementDbContext.ElementValue.ToList().FindAll(x => x.ElementId == eleref.ElementId);
                                var element = radElementDbContext.Element.ToList().Find(x => x.Id == eleref.ElementId);

                                if (elementValues != null && elementValues.Any())
                                {
                                    radElementDbContext.ElementValue.RemoveRange(elementValues);
                                }

                                if (element != null)
                                {
                                    radElementDbContext.Element.Remove(element);
                                }

                                radElementDbContext.ElementSetRef.Remove(eleref);
                            }
                            radElementDbContext.SaveChanges();
                        }

                        List<int> elementIds = AddElements(module.DataElements);
                        AddSetRef(setId, elementIds);
                        return await Task.FromResult(new JsonResult(string.Format("Set with id {0} is updated.", setId), HttpStatusCode.OK));
                    }

                    return await Task.FromResult(new JsonResult(string.Format("No such set with id '{0}'", setId), HttpStatusCode.NotFound));
                }
                else
                {
                    return await Task.FromResult(new JsonResult("Xml content provided is invalid", HttpStatusCode.BadRequest));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'UpdateModule(XmlElement xmlContent, int setId)'");
                return await Task.FromResult(new JsonResult(ex, HttpStatusCode.InternalServerError));
            }
        }

        private List<int> AddElements(List<DataElement> dataElements)
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

        private uint AddElement(DataElement data)
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

            radElementDbContext.Element.Add(element);
            radElementDbContext.SaveChanges();

            AddElementValues(data, element.Id);

            return element.Id;
        }

        private void AddElementValues(DataElement data, uint elementId)
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

                radElementDbContext.ElementValue.Add(elementvalue);
            }
            radElementDbContext.SaveChanges();
        }

        private string AddElementSet(AssistModule module)
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

            radElementDbContext.ElementSet.Add(set);
            radElementDbContext.SaveChanges();
            return set.Id.ToString();
        }

        private bool UpdateElementSet(int setId, CreateUpdateSet content)
        {
            var elementSet = radElementDbContext.ElementSet.ToList().Find(x => x.Id == setId);

            if (elementSet != null)
            {
                elementSet.Name = content.ModuleName.Replace("_", " ");
                elementSet.Description = content.Description;
                elementSet.ContactName = content.ContactName;
                return radElementDbContext.SaveChanges() > 0;
            }

            return false;
        }

        private bool DeleteElementSet(ElementSet elementSet)
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
            return radElementDbContext.SaveChanges() > 0;
        }

        private void AddSetRef(int setId, List<int> elementIds)
        {
            foreach (short elementid in elementIds)
            {
                ElementSetRef setRef = new ElementSetRef()
                {
                    ElementSetId = setId,
                    ElementId = elementid
                };

                radElementDbContext.ElementSetRef.Add(setRef);
                radElementDbContext.SaveChanges();
            }
        }

        private Task<AssistModule> GetDeserializedDataFromXml(string content)
        {
            return Task.Run(() =>
            {
                try
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
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception in method 'GetDeserializedDataFromXml(string content))'");
                    return null;
                }
            });
        }
    }
}
