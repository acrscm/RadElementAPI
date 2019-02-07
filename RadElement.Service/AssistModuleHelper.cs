using RadElement.Core.Domain;
using RadElement.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RadElement.Service
{
    public class AssistModuleHelper
    {
        private Dictionary<string, string> dataElementKeyValues = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        private readonly ModuleDetails moduleDetails;
        private readonly ReportingModule reportingModule;

        public AssistModuleHelper(ModuleDetails moduleDetails)
        {
            this.moduleDetails = moduleDetails;
            reportingModule = moduleDetails.Module;
        }

        public AssistModule ConvertToAssistModule()
        {
            var dataElements = ReturnDataElements();
            var assistModule = new AssistModule()
            {
                ModuleName = moduleDetails.Module.Metadata.ID,
                Version = moduleDetails.Module.Metadata.ModuleVersion,
                MetaData = reportingModule.Metadata,
            };

            List<DataElement> allElements = new List<DataElement>();
            allElements.AddRange(ReturnGlobalValues());
            allElements.AddRange(dataElements);
            assistModule.DataElements = allElements;
            return assistModule;
        }

        private void PopulateBasicData(string id, string label, baseElementContentsIsRequired isRequired,
            string hintText, Diagram[] diagrams, string displaySequence, DataElement element)
        {
            element.Id = id;
            element.Label = label;
            element.Required = isRequired == baseElementContentsIsRequired.@true ? true : false;
            element.HintText = hintText;
            element.Diagrams = new List<Diagram>();
            if (diagrams != null)
            {
                foreach (Diagram diag in diagrams)
                {
                    var diagram = new Diagram
                    {
                        Label = diag.Label,
                        Location = diag.Location,
                        DisplaySequence = diag.DisplaySequence,
                        IsKeyDiagram = diag.IsKeyDiagram,
                        IsKeyDiagramSpecified = diag.IsKeyDiagramSpecified
                    };
                    element.Diagrams.Add(diagram);
                }
            }
            element.DisplaySequence = Convert.ToInt32(displaySequence);
        }

        private DataElement CreateNumericElement(NumericDataElement dataElement)
        {
            var element = new NumericElement();
            PopulateBasicData(dataElement.Id, dataElement.Label, dataElement.IsRequired,
              dataElement.Hint, dataElement.Diagrams, dataElement.DisplaySequence, element);

            if (dataElement.MaximumSpecified)
            {
                element.MaximumValue = dataElement.Maximum;
            }
            if (dataElement.MinimumSpecified)
            {
                element.MinimumValue = dataElement.Minimum;
            }

            element.Units = dataElement.Units;
            return element;
        }

        private DataElement CreateIntegerElement(IntegerDataElement dataElement)
        {
            var element = new IntegerElement();
            PopulateBasicData(dataElement.Id, dataElement.Label, dataElement.IsRequired,
              dataElement.Hint, dataElement.Diagrams, dataElement.DisplaySequence, element);
            if (!string.IsNullOrEmpty(dataElement.Maximum))
            {
                element.MaximumValue = Convert.ToInt32(dataElement.Maximum);
            }
            if (!string.IsNullOrEmpty(dataElement.Minimum))
            {
                element.MinimumValue = Convert.ToInt32(dataElement.Minimum);
            }
            return element;
        }

        private ChoiceElement CreateChoiceElement(ChoiceDataElement dataElement)
        {
            ChoiceElement choiceElement = new ChoiceElement();
            bool isMultiChoice = false;

            return CreateChoiceElement(dataElement.Id, dataElement.Label, dataElement.IsRequired, dataElement.Hint,
                dataElement.Diagrams, dataElement.ChoiceInfo, dataElement.ImageMap, dataElement.AllowFreetextSpecified, isMultiChoice, dataElement.DisplaySequence, choiceElement);
        }

        private ChoiceElement CreateChoiceElement(string id, string label, baseElementContentsIsRequired isRequired,
            string hintText, Diagram[] diagrams, ChoiceInfo choiceInfo, ImageMap imageMap, bool allowFreeText, bool isMultiChoice, string displaySequence,
            ChoiceElement element)
        {
            PopulateBasicData(id, label, isRequired,
             hintText, diagrams, displaySequence, element);
            dataElementKeyValues[id] = label;
            element.Options = new List<Core.DTO.Option>();
            if (choiceInfo.Choice != null)
            {
                var choiceDTO = new Core.DTO.Option();
                dataElementKeyValues[choiceInfo.Choice.Value] = choiceInfo.Choice.Label;
                choiceDTO.Label = choiceInfo.Choice.Label;
                choiceDTO.Value = choiceInfo.Choice.Value;
                choiceDTO.ReportText = choiceInfo.Choice.ReportText;
                element.Options.Add(choiceDTO);
            }

            if (choiceInfo.Choices != null)
            {
                foreach (var choiceData in choiceInfo.Choices)
                {
                    dataElementKeyValues[choiceData.Value] = choiceData.Label;
                    var choiceDTO = new Core.DTO.Option
                    {
                        Label = choiceData.Label,
                        Value = choiceData.Value,
                        ReportText = choiceData.ReportText
                    };
                    element.Options.Add(choiceDTO);
                }
            }
            if (imageMap != null)
            {
                var diagram = new Diagram
                {
                    Label = imageMap.Label,
                    Location = imageMap.Location
                };
                element.ImageMapLocation = diagram;
            }
            if (!isMultiChoice)
            {
                element.AllowFreeText = allowFreeText;
            }
            return element;
        }

        private MultipleChoiceElement CreateMutipleChoiceElement(MultiChoiceDataElement dataElement)
        {
            MultipleChoiceElement element = new MultipleChoiceElement();
            bool isMultiChoice = true;

            CreateChoiceElement(dataElement.Id, dataElement.Label, dataElement.IsRequired, dataElement.Hint,
                dataElement.Diagrams, dataElement.ChoiceInfo, dataElement.ImageMap, false, isMultiChoice, dataElement.DisplaySequence,
                element);
            return element;
        }

        private List<DataElement> ReturnDataElements()
        {
            List<DataElement> elements = new List<DataElement>();
            var numericDataElements = reportingModule.DataElements.OfType<NumericDataElement>().ToList();
            var integerDataElements = reportingModule.DataElements.OfType<IntegerDataElement>().ToList();
            var choiceDataElements = reportingModule.DataElements.OfType<ChoiceDataElement>().ToList();
            var multiChoiceDataElements = reportingModule.DataElements.OfType<MultiChoiceDataElement>().ToList();
            numericDataElements.ForEach(numericElement =>
            {
                dataElementKeyValues[numericElement.Id] = numericElement.Label;
                elements.Add(CreateNumericElement(numericElement));

            });

            integerDataElements.ForEach(integerDataElement =>
            {
                dataElementKeyValues[integerDataElement.Id] = integerDataElement.Label;
                elements.Add(CreateIntegerElement(integerDataElement));
            });
            choiceDataElements.ForEach(choiceDataElement =>
            {
                elements.Add(CreateChoiceElement(choiceDataElement));
            });

            multiChoiceDataElements.ForEach(multiChoiceDataElement =>
            {
                elements.Add(CreateMutipleChoiceElement(multiChoiceDataElement));
            });
            elements = elements.OrderBy(o => o.DisplaySequence).ToList();
            return elements;
        }       
     

        private List<Core.DTO.GlobalValue> ReturnGlobalValues()
        {
            List<Core.DTO.GlobalValue> globals = new List<Core.DTO.GlobalValue>();
            var globalValues = reportingModule.DataElements.OfType<Core.Domain.GlobalValue>().ToList();
            globalValues.ForEach(globalValue =>
            {
                var globalsDTO = new Core.DTO.GlobalValue
                {
                    Id = globalValue.Id,
                    Label = globalValue.Id,
                    Value = globalValue.Text[0]
                };
                globals.Add(globalsDTO);
            });
            return globals;
        }
        
    }
}
