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

        private List<DataElement> ReturnDataElements()
        {
            List<DataElement> elements = new List<DataElement>();
            var numericDataElements = reportingModule.DataElements.OfType<NumericDataElement>().ToList();
            var integerDataElements = reportingModule.DataElements.OfType<IntegerDataElement>().ToList();
            var choiceDataElements = reportingModule.DataElements.OfType<ChoiceDataElement>().ToList();
            var multiChoiceDataElements = reportingModule.DataElements.OfType<MultiChoiceDataElement>().ToList();
            var dateTimeDataElements = reportingModule.DataElements.OfType<DateTimeDataElement>().ToList();
            var durationDataElements = reportingModule.DataElements.OfType<TimeSpanDataElement>().ToList();

            dateTimeDataElements.ForEach(dateTimeElement =>
            {
                elements.Add(CreateDateTimeElement(dateTimeElement));
            });

            durationDataElements.ForEach(durationElement =>
            {
                elements.Add(CreateDurationElement(durationElement));
            });

            numericDataElements.ForEach(numericElement =>
            {
                elements.Add(CreateNumericElement(numericElement));
            });

            integerDataElements.ForEach(integerDataElement =>
            {
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

        private void PopulateBasicData(string id, string label, BaseElementContentsIsRequired isRequired,
            string hintText, Diagram[] diagrams, string displaySequence, DataElement element, BaseElementContents dataelement)
        {
            element.Id = id;
            element.Label = label;
            element.Required = isRequired == BaseElementContentsIsRequired.@true ? true : false;
            element.HintText = hintText;
            element.Source = dataelement.Source;
            element.Output = dataelement.Output == BaseElementContentsIsOutput.@true ? true : false;
            element.Hasprefilled = dataelement.Hasprefilled == BaseElementContentsHasPrefilled.@true ? true : false;
            element.Editable = dataelement.Editable == BaseElementContentsIsEditable.@true ? true : false;
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

            element.CodableConcept = dataelement?.CodableConcept;
            element.DisplaySequence = Convert.ToInt32(displaySequence);
        }

        private DataElement CreateDateTimeElement(DateTimeDataElement dataElement)
        {
            var element = new DateTimeElement();
            PopulateBasicData(dataElement.Id, dataElement.Label, dataElement.IsRequired,
              dataElement.Hint, dataElement.Diagrams, dataElement.DisplaySequence, element, dataElement);

            return element;
        }

        private DataElement CreateDurationElement(TimeSpanDataElement dataElement)
        {
            var element = new DurationElement();
            PopulateBasicData(dataElement.Id, dataElement.Label, dataElement.IsRequired,
             dataElement.Hint, dataElement.Diagrams, dataElement.DisplaySequence, element, dataElement);

            element.ShowDays = dataElement.ShowDays == BaseShowDurationAttributes.@true ? true : false;
            element.ShowHours = dataElement.ShowHours == BaseShowDurationAttributes.@true ? true : false;
            element.ShowMinutes = dataElement.ShowMinutes == BaseShowDurationAttributes.@true ? true : false;
            element.ShowSeconds = dataElement.ShowSeconds == BaseShowDurationAttributes.@true ? true : false;
            element.MinimumHours = dataElement.MinimumHours != null && dataElement.MinimumHours == "0" ? null : dataElement.MinimumHours;
            element.MaximumHours = dataElement.MaximumHours != null && dataElement.MaximumHours == "0" ? null : dataElement.MaximumHours;
            element.MinimumMinutes = dataElement.MinimumMinutes != null && dataElement.MinimumMinutes == "0" ? null : dataElement.MinimumMinutes;
            element.MaximumMinutes = dataElement.MaximumMinutes != null && dataElement.MaximumMinutes == "0" ? null : dataElement.MaximumMinutes;
            element.MinimumDay = dataElement.MinimumDay != null && dataElement.MinimumDay == "0" ? null : dataElement.MinimumDay;
            element.MaximumDay = dataElement.MaximumDay != null && dataElement.MaximumDay == "0" ? null : dataElement.MaximumDay;
            element.MinimumSeconds = dataElement.MinimumSeconds != null && dataElement.MinimumSeconds == "0" ? null : dataElement.MinimumSeconds;
            element.MaximumSeconds = dataElement.MaximumSeconds != null && dataElement.MaximumSeconds == "0" ? null : dataElement.MaximumSeconds;

            return element;
        }

        private DataElement CreateNumericElement(NumericDataElement dataElement)
        {
            var element = new NumericElement();
            PopulateBasicData(dataElement.Id, dataElement.Label, dataElement.IsRequired,
              dataElement.Hint, dataElement.Diagrams, dataElement.DisplaySequence, element, dataElement);

            if (dataElement.MaximumSpecified)
            {
                element.MaximumValue = dataElement.Maximum;
            }
            if (dataElement.MinimumSpecified)
            {
                element.MinimumValue = dataElement.Minimum;
            }

            element.Unit = dataElement.Unit;
            return element;
        }

        private DataElement CreateIntegerElement(IntegerDataElement dataElement)
        {
            var element = new IntegerElement();
            PopulateBasicData(dataElement.Id, dataElement.Label, dataElement.IsRequired,
              dataElement.Hint, dataElement.Diagrams, dataElement.DisplaySequence, element, dataElement);
            if (!string.IsNullOrEmpty(dataElement.Maximum))
            {
                element.MaximumValue = Convert.ToInt32(dataElement.Maximum);
            }
            if (!string.IsNullOrEmpty(dataElement.Minimum))
            {
                element.MinimumValue = Convert.ToInt32(dataElement.Minimum);
            }

            element.Unit = dataElement.Unit;
            return element;
        }

        private ChoiceElement CreateChoiceElement(ChoiceDataElement dataElement)
        {
            ChoiceElement choiceElement = new ChoiceElement();
            bool isMultiChoice = false;
            return CreateChoiceElement(dataElement.Id, dataElement.Label, dataElement.IsRequired, dataElement.Hint,
                dataElement.Diagrams, dataElement.ChoiceInfo, dataElement.ImageMap, dataElement.AllowFreetextSpecified, isMultiChoice, dataElement.DisplaySequence, choiceElement,
                dataElement);
        }


        private ChoiceElement CreateChoiceElement(string id, string label, BaseElementContentsIsRequired isRequired,
            string hintText, Diagram[] diagrams, ChoiceInfo choiceInfo, ImageMap imageMap, bool allowFreeText, bool isMultiChoice, string displaySequence,
            ChoiceElement element, BaseElementContents dataelement)
        {
            PopulateBasicData(id, label, isRequired,
             hintText, diagrams, displaySequence, element, dataelement);
            element.Options = new List<Core.DTO.Option>();
            if (choiceInfo.Choice != null)
            {
                var choiceDTO = new Core.DTO.Option();
                choiceDTO.Label = choiceInfo.Choice.Label;
                choiceDTO.Value = choiceInfo.Choice.Value;
                choiceDTO.ReportText = choiceInfo.Choice.ReportText;
                element.Options.Add(choiceDTO);
            }

            if (choiceInfo.Choices != null)
            {
                foreach (var choiceData in choiceInfo.Choices)
                {
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
                element, dataElement);
            return element;
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
                    Value = globalValue.Text
                };
                globals.Add(globalsDTO);
            });
            return globals;
        }        
    }
}
