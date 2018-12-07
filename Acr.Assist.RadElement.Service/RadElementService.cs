using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Acr.Assist.RadElement.Core.Data;
using Acr.Assist.RadElement.Core.Domain;
using Acr.Assist.RadElement.Core.DTO;
using Acr.Assist.RadElement.Core.Infrastructure;
using Acr.Assist.RadElement.Core.Integrations;
using Acr.Assist.RadElement.Core.Services;

namespace Acr.Assist.RadElement.Service
{
    public class RadElementService : IRadElementService
    {
        private readonly IConfigurationManager configurationManager;
        private readonly IMarvalMicroService marvalMicroService;
        private IRadElementDbContext radElementDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadElementService" /> class.
        /// </summary>
        /// <param name="marvalMicroService">The marval micro service.</param>
        public RadElementService(IMarvalMicroService marvalMicroService, IRadElementDbContext radElementDbContext, IConfigurationManager configurationManager)
        {
            this.marvalMicroService = marvalMicroService;
            this.radElementDbContext = radElementDbContext;
            this.configurationManager = configurationManager;
        }

        public async Task InsertData(Object data)
        {
            if (data is UserModule)
            {
                var userModule = data as UserModule;
                var xml = await marvalMicroService.GetModule(userModule);
                if (!string.IsNullOrEmpty(xml))
                {
                    XmlContent xmlContent = new XmlContent() { Content = xml };
                    await InsertElements(xmlContent);
                }
            }
            else if(data is XmlContent)
            {
                var xmlContent = data as XmlContent;
                await InsertElements(xmlContent);
            }
        }

        private async Task InsertElements(XmlContent xmlContent)
        {
            var assistModule = await GetDeserializedDataFromXml(xmlContent);
            if (assistModule != null)
            {
                List<uint> elementIds = AddElementsToDB(assistModule.DataElements);
                AddElementSet(assistModule, elementIds);
            }
        }

        private List<uint> AddElementsToDB(List<DataElement> dataElements)
        {
            List<uint> elementIds = new List<uint>();
            foreach (DataElement dataElement in dataElements)
            {
                if (dataElement.DataElementType == DataElementType.Global)
                {
                    continue;
                }

                uint elementId = AddElementToDB(dataElement);
                elementIds.Add(elementId);
            }

            return elementIds;
        }

        private void AddElementSet(AssistModule cdemodule, List<uint> elementIds)
        {
            ElementSet set = new ElementSet()
            {
                Name = cdemodule.ModuleName.Replace("_", " "),
                Description = cdemodule.MetaData.Info.Description,
                ContactName = cdemodule.MetaData.Info.Contact.Name
            };

            radElementDbContext.ElementSet.Add(set);
            radElementDbContext.SaveChanges();

            foreach (short elementid in elementIds)
            {
                ElementSetRef setRef = new ElementSetRef()
                {
                    ElementSetId = set.Id,
                    ElementId = elementid
                };

                radElementDbContext.ElementSetRef.Add(setRef);
                radElementDbContext.SaveChanges();
            }
        }

        private uint AddElementToDB(DataElement data)
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

            if (data.DataElementType == DataElementType.MultiChoice)
            {
                MultipleChoiceElement choiceElement = data as MultipleChoiceElement;
                AddElementValues(choiceElement.Options, element.Id);
            }

            if (data.DataElementType == DataElementType.Choice)
            {
                ChoiceElement choiceElement = data as ChoiceElement;
                AddElementValues(choiceElement.Options, element.Id);
            }

            return element.Id;
        }

        private void AddElementValues(List<Core.DTO.Option> options, uint elementId)
        {
            foreach (Core.DTO.Option option in options)
            {
                ElementValue elementvalue = new ElementValue()
                {
                    Code = "",
                    Definition = option.Label,
                    ElementId = elementId,
                    Name = option.Label
                };

                radElementDbContext.ElementValue.Add(elementvalue);
                radElementDbContext.SaveChanges();
            }
        }

        private Task<AssistModule> GetDeserializedDataFromXml(XmlContent xmlContent)
        {
            return Task.Run(() =>
            {
                ModuleDetails details = null;
                XmlSerializer serializer = null;
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(xmlContent.Content);
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
            });
        }
    }
}
