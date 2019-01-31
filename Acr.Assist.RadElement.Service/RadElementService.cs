using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Microsoft.EntityFrameworkCore;

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

        #region SET

        /// <summary>
        /// Gets the set.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ElementSet>> GetSets()
        {
            var cdeSets = await radElementDbContext.ElementSet.ToListAsync();
            return cdeSets;
        }

        /// <summary>
        /// Gets the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        public async Task<ElementSet> GetSet(int setId)
        {
            var cdeSets = await radElementDbContext.ElementSet.ToListAsync();
            return cdeSets.Find(x => x.Id == setId);
        }

        /// <summary>
        /// Searches the cde set.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<List<ElementSet>> SearchSet(string searchKeyword)
        {
            var cdeSets = await radElementDbContext.ElementSet.ToListAsync();
            return cdeSets.FindAll(x => x.Name.ToLower().Contains(searchKeyword.ToLower()) || x.Description.ToLower().Contains(searchKeyword.ToLower()) ||
                                        x.ContactName.ToLower().Contains(searchKeyword.ToLower()));
        }

        /// <summary>
        /// Creates the cde set.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<string> CreateSet(XmlElement content)
        {
            string setId = string.Empty;
            var assistModule = await GetDeserializedDataFromXml(content.OuterXml);
            if (assistModule != null)
            {
                setId = AddElementSet(assistModule);
            }

            return setId;
        }

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<bool> UpdateSet(int setId, XmlElement content)
        {
            var assistModule = await GetDeserializedDataFromXml(content.OuterXml);
            if (assistModule != null)
            {
                return UpdateElementSet(setId, assistModule);
            }

            return false;
        }

        /// <summary>
        /// Deletes the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        public async Task<bool> DeleteSet(int setId)
        {
            var elementSets = await radElementDbContext.ElementSet.ToListAsync();
            var elementSet = elementSets.Find(x => x.Id == setId);

            if (elementSet != null)
            {
                return DeleteElementSet(elementSet);
            }

            return false;
        }

        #endregion

        #region Element

        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Element>> GetElements()
        {
            var elements = await radElementDbContext.Element.ToListAsync();
            return elements;
        }

        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        public async Task<Element> GetElement(int elementId)
        {
            var elements = await radElementDbContext.Element.ToListAsync();
            return elements.Find(x => x.Id == elementId);
        }

        /// <summary>
        /// Gets the elements by set identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        public async Task<List<Element>> GetElementsBySetId(int setId)
        {
            var setRefs = await radElementDbContext.ElementSetRef.ToListAsync();
            var elementIds = setRefs.FindAll(x => x.ElementSetId == setId);
            var elements = await radElementDbContext.Element.ToListAsync();

            var selectedElements = from elemetId in elementIds
                                   join element in elements on elemetId.Id equals (int)element.Id
                                   select element;

            return selectedElements.ToList();
        }

        /// <summary>
        /// Searches the element.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<List<Element>> SearchElement(string searchKeyword)
        {
            var elements = await radElementDbContext.Element.ToListAsync();
            return elements.FindAll(x => x.Definition.ToLower().Contains(searchKeyword.ToLower()) || x.Editor.ToLower().Contains(searchKeyword.ToLower()) ||
                                         x.Instructions.ToLower().Contains(searchKeyword.ToLower()) || x.Name.ToLower().Contains(searchKeyword.ToLower()) ||
                                         x.Question.ToLower().Contains(searchKeyword.ToLower()) || x.References.ToLower().Contains(searchKeyword.ToLower()) ||
                                         x.ShortName.ToLower().Contains(searchKeyword.ToLower()) || x.Source.ToLower().Contains(searchKeyword.ToLower()) ||
                                         x.Synonyms.ToLower().Contains(searchKeyword.ToLower()));
        }

        /// <summary>
        /// Creates the element.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<List<int>> CreateElement(int setId, XmlElement content)
        {
            List<int> elementIds = new List<int>();
            var assistModule = await GetDeserializedDataFromXml(content.OuterXml);
            if (assistModule != null)
            {
                elementIds = AddElements(assistModule.DataElements);
                AddSetRef(setId, elementIds);
                radElementDbContext.SaveChanges();
            }

            return elementIds;
        }

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<bool> UpdateElement(int setId, int elementId, XmlElement content)
        {
            var assistModule = await GetDeserializedDataFromXml(content.OuterXml);
            if (assistModule != null)
            {
                return UpdateElement(setId, elementId, assistModule.DataElements[0]);
            }

            return false;
        }

        /// <summary>
        /// Deletes the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        public async Task<bool> DeleteElement(int setId, int elementId)
        {
            var elementSetRefs = await radElementDbContext.ElementSetRef.ToListAsync();
            var elementSetRef = elementSetRefs.Find(x => x.ElementSetId == setId && x.ElementId == elementId);

            if (elementSetRef != null)
            {
                return DeleteElement(elementSetRef);
            }

            return false;
        }

        #endregion

        #region Element SET

        /// <summary>
        /// Creates the element set.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public async Task<string> CreateElementSet(XmlElement data)
        {
            return await InsertElements(data.OuterXml);
        }

        /// <summary>
        /// Creates the element set.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public async Task<string> CreateElementSet(UserModule data)
        {
            var id = string.Empty;
            var xml = await marvalMicroService.GetModule(data);
            if (!string.IsNullOrEmpty(xml))
            {
                id = await InsertElements(xml);
            }

            return id;
        }

        #endregion

        #region Private Methods
        
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
        
        private bool UpdateElementSet(int setId, AssistModule assistModule)
        {
            var elementSet = radElementDbContext.ElementSet.ToList().Find(x => x.Id == setId);

            if (elementSet != null)
            {
                elementSet.Name = assistModule.ModuleName.Replace("_", " ");
                elementSet.Description = assistModule.MetaData.Info != null && !string.IsNullOrEmpty(assistModule.MetaData.Info.Description)
                                            ? assistModule.MetaData.Info.Description : string.Empty;
                elementSet.ContactName = assistModule.MetaData.Info.Contact != null && !string.IsNullOrEmpty(assistModule.MetaData.Info.Contact.Name)
                                            ? assistModule.MetaData.Info.Contact.Name : string.Empty;
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

        private bool UpdateElement(int setId, int elementId, DataElement data)
        {
            var elementSet = radElementDbContext.ElementSet.ToList().Find(x => x.Id == setId);

            if (elementSet != null)
            {
                var element = radElementDbContext.Element.ToList().Find(x => x.Id == elementId);
                if (element != null)
                {
                    var elementValues = radElementDbContext.ElementValue.ToList().FindAll(x => x.ElementId == element.Id);
                    if (elementValues != null && elementValues.Any())
                    {
                        radElementDbContext.ElementValue.RemoveRange(elementValues);
                        AddElementValues(data, element.Id);
                    }

                    element.Name = data.Label;
                    element.ShortName = "";
                    element.Definition = data.HintText ?? "";
                    element.MaxCardinality = 1;
                    element.MinCardinality = 1;
                    element.Source = "DSI TOUCH-AI";
                    element.Status = "Proposed";
                    element.StatusDate = DateTime.Now;
                    element.Editor = "";
                    element.Instructions = "";
                    element.Question = data.Label ?? "";
                    element.References = "";
                    element.Synonyms = "";
                    element.VersionDate = DateTime.Now;
                    element.Version = "1";
                    element.ValueSize = 0;
                    element.Unit = "";

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

                    return radElementDbContext.SaveChanges() > 0;
                }
            }

            return false;
        }

        private bool DeleteElement(ElementSetRef elementSetRef)
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

            return radElementDbContext.SaveChanges() > 0;
        }

        private async Task<string> InsertElements(string content)
        {
            string id = string.Empty;
            var assistModule = await GetDeserializedDataFromXml(content);
            if (assistModule != null)
            {
                List<int> elementIds = AddElements(assistModule.DataElements);
                id = AddElementSet(assistModule);
                AddSetRef(Int16.Parse(id), elementIds);
                radElementDbContext.SaveChanges();
            }

            return id;
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
            }
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
                    Value = option.Value,
                    Definition = option.Label,
                    ElementId = elementId,
                };

                radElementDbContext.ElementValue.Add(elementvalue);
            }
        }

        private Task<AssistModule> GetDeserializedDataFromXml(string content)
        {
            return Task.Run(() =>
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
            });
        }

        #endregion
    }
}
