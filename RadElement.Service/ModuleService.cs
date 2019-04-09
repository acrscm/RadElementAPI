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
                    return await Task.FromResult(new JsonResult(new SetIdDetails() { SetId = "RDES" + idDetails.SetId }, HttpStatusCode.Created));
                }
                else
                {
                    return await Task.FromResult(new JsonResult("Xml content provided is invalid", HttpStatusCode.BadRequest));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'CreateModule(XmlElement xmlContent)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        public async Task<JsonResult> UpdateModule(XmlElement xmlContent, string setId)
        {
            try
            {
                var module = await GetDeserializedDataFromXml(xmlContent.OuterXml);
                if (module != null)
                {
                    if (IsValidSetId(setId))
                    {
                        int id = Convert.ToInt32(setId.Remove(0, 4));
                        var elementSets = radElementDbContext.ElementSet.ToList();
                        var elementSet = elementSets.Find(x => x.Id == id);

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

                            var elementSetRefs = radElementDbContext.ElementSetRef.ToList().FindAll(x => x.ElementSetId == id);
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
                            AddSetRef(id, elementIds);
                            return await Task.FromResult(new JsonResult(string.Format("Set with id {0} is updated.", setId), HttpStatusCode.OK));
                        }
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
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
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
                element.Unit = intElement.Unit ?? "";
            }

            if (data is NumericElement)
            {
                NumericElement numericElement = data as NumericElement;
                float? minValue = null;
                float? maxValue = null;

                if (numericElement.MinimumValue.HasValue)
                {
                    minValue = Convert.ToSingle(numericElement.MinimumValue.Value);
                }

                if (numericElement.MaximumValue.HasValue)
                {
                    maxValue = Convert.ToSingle(numericElement.MaximumValue.Value);
                }

                element.ValueType = "float";
                element.ValueMin = minValue;
                element.ValueMax = maxValue;
                element.StepValue = 0.1f;
                element.Unit = numericElement.Unit ?? "";
            }

            if (data is ChoiceElement)
            {
                ChoiceElement choiceElement = data as ChoiceElement;
                element.ValueType = "valueSet";
            }

            if (data is MultipleChoiceElement)
            {
                MultipleChoiceElement choiceElement = data as MultipleChoiceElement;
                element.ValueType = "valueSet";
                element.MaxCardinality = (short)choiceElement.Options.Count;
            }

            if (data is DateTimeElement)
            {
                DateTimeElement datetimeElement = data as DateTimeElement;
                element.ValueType = "date";
            }

            if (data is DurationElement)
            {
                DurationElement durationElement = data as DurationElement;
                element.ValueType = "string";
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
                Status = "Proposed"
            };

            radElementDbContext.ElementSet.Add(set);
            radElementDbContext.SaveChanges();
            return set.Id.ToString();
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
    }
}
