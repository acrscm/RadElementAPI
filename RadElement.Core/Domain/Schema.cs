namespace RadElement.Core.Domain
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ReportingModule
    {
        private Metadata metadataField;

        private object[] dataElementsField;

        private Rules rulesField;

        private EndPoints endPointsField;

        /// <remarks/>
        public Metadata Metadata
        {
            get
            {
                return this.metadataField;
            }
            set
            {
                this.metadataField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("GlobalValue", typeof(GlobalValue))]
        [XmlArrayItem("NumericDataElement", typeof(NumericDataElement))]
        [XmlArrayItem("IntegerDataElement", typeof(IntegerDataElement))]
        [XmlArrayItem("ChoiceDataElement", typeof(ChoiceDataElement))]
        [XmlArrayItem("ComputedDataElement", typeof(ComputedDataElement))]
        [XmlArrayItem("MultiChoiceDataElement", typeof(MultiChoiceDataElement))]
        [XmlArrayItem("DateTimeDataElement", typeof(DateTimeDataElement))]
        [XmlArrayItem("TimeSpanDataElement", typeof(TimeSpanDataElement))]
        public object[] DataElements
        {
            get
            {
                return this.dataElementsField;
            }
            set
            {
                this.dataElementsField = value;
            }
        }

        /// <remarks/>
        public Rules Rules
        {
            get
            {
                return this.rulesField;
            }
            set
            {
                this.rulesField = value;
            }
        }

        /// <remarks/>
        public EndPoints EndPoints
        {
            get
            {
                return this.endPointsField;
            }
            set
            {
                this.endPointsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Metadata
    {
        private string labelField;

        private string idField;

        private string schemaVersionField;

        private string moduleVersionField;

        private string createdDateField { get; set; }

        private string lastModifiedDateField { get; set; }

        private string approvedByField { get; set; }

        private string reviewedByField { get; set; }

        private string developedByField { get; set; }

        private CodableConcept codableConcept;

        private Info infoField;

        private string reportCitationTextField;

        private Ontology ontologyField;

        private ApplicableExamCategory[] applicableExamsField;

        private ApplicableSexes applicableSexesField;

        private ApplicableAgeGroups applicableAgeGroupsField;

        private TextCues textCuesField;

        private string[] voiceActivationField;

        private KeyValue[] additionalInfoField;

        private List<ApplicableExamFormatter> applicableExamFormatterField { get; set; }

        /// <remarks/>
        public string Label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string SchemaVersion
        {
            get
            {
                return this.schemaVersionField;
            }
            set
            {
                this.schemaVersionField = value;
            }
        }

        /// <remarks/>
        public string ModuleVersion
        {
            get
            {
                return this.moduleVersionField;
            }
            set
            {
                this.moduleVersionField = value;
            }
        }

        /// <remarks/>
        public string CreatedDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
            }
        }

        /// <remarks/>
        public string LastModifiedDate
        {
            get
            {
                return this.lastModifiedDateField;
            }
            set
            {
                this.lastModifiedDateField = value;
            }
        }

        /// <remarks/>
        public string ApprovedBy
        {
            get
            {
                return this.approvedByField;
            }
            set
            {
                this.approvedByField = value;
            }
        }

        /// <remarks/>
        public string ReviewedBy
        {
            get
            {
                return this.reviewedByField;
            }
            set
            {
                this.reviewedByField = value;
            }
        }

        /// <remarks/>
        public string DevelopedBy
        {
            get
            {
                return this.developedByField;
            }
            set
            {
                this.developedByField = value;
            }
        }

        /// <remarks/>
        public CodableConcept CodableConcept
        {
            get
            {
                return this.codableConcept;
            }
            set
            {
                this.codableConcept = value;
            }
        }

        /// <remarks/>
        public Info Info
        {
            get
            {
                return this.infoField;
            }
            set
            {
                this.infoField = value;
            }
        }

        /// <remarks/>
        public string ReportCitationText
        {
            get
            {
                return this.reportCitationTextField;
            }
            set
            {
                this.reportCitationTextField = value;
            }
        }

        /// <remarks/>
        public Ontology Ontology
        {
            get
            {
                return this.ontologyField;
            }
            set
            {
                this.ontologyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ApplicableExamCategory", IsNullable = false)]
        public ApplicableExamCategory[] ApplicableExams
        {
            get
            {
                return this.applicableExamsField;
            }
            set
            {
                this.applicableExamsField = value;
            }
        }

        /// <remarks/>
        public ApplicableSexes ApplicableSexes
        {
            get
            {
                return this.applicableSexesField;
            }
            set
            {
                this.applicableSexesField = value;
            }
        }

        /// <remarks/>
        public ApplicableAgeGroups ApplicableAgeGroups
        {
            get
            {
                return this.applicableAgeGroupsField;
            }
            set
            {
                this.applicableAgeGroupsField = value;
            }
        }

        /// <remarks/>
        public TextCues TextCues
        {
            get
            {
                return this.textCuesField;
            }
            set
            {
                this.textCuesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("VoiceCommandPhrase", IsNullable = false)]
        public string[] VoiceActivation
        {
            get
            {
                return this.voiceActivationField;
            }
            set
            {
                this.voiceActivationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("KeyValue", IsNullable = false)]
        public KeyValue[] AdditionalInfo
        {
            get
            {
                return this.additionalInfoField;
            }
            set
            {
                this.additionalInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public List<ApplicableExamFormatter> ApplicableExamFormatter
        {
            get
            {
                return this.applicableExamFormatterField;
            }
            set
            {
                this.applicableExamFormatterField = value;
            }
        }
    }

    public class ApplicableExamFormatter
    {
        public string Modality { get; set; }
        public string Anatomy { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Info
    {
        private string descriptionField;

        private Citation[] referencesField;

        private Diagram[] diagramsField;

        private string helpTextField;

        private Contact contactField;

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Citation", IsNullable = false)]
        public Citation[] References
        {
            get
            {
                return this.referencesField;
            }
            set
            {
                this.referencesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Diagram", IsNullable = false)]
        public Diagram[] Diagrams
        {
            get
            {
                return this.diagramsField;
            }
            set
            {
                this.diagramsField = value;
            }
        }

        /// <remarks/>
        public string HelpText
        {
            get
            {
                return this.helpTextField;
            }
            set
            {
                this.helpTextField = value;
            }
        }

        /// <remarks/>
        public Contact Contact
        {
            get
            {
                return this.contactField;
            }
            set
            {
                this.contactField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Citation
    {
        private string pubmedIdField;

        private string urlField;

        private string textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        public string PubmedId
        {
            get
            {
                return this.pubmedIdField;
            }
            set
            {
                this.pubmedIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string Url
        {
            get
            {
                return this.urlField;
            }
            set
            {
                this.urlField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class valueBranches
    {
        private ComputedElementBranch[] branchField;

        private ComputedElementDefaultBranch defaultBranchField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Branch")]
        public ComputedElementBranch[] Branch
        {
            get
            {
                return this.branchField;
            }
            set
            {
                this.branchField = value;
            }
        }

        /// <remarks/>
        public ComputedElementDefaultBranch DefaultBranch
        {
            get
            {
                return this.defaultBranchField;
            }
            set
            {
                this.defaultBranchField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ComputedElementBranch
    {
        private object conditionTypeField;

        private object itemField;

        [XmlElement("AndCondition", typeof(AndCondition))]
        [XmlElement("OrCondition", typeof(OrCondition))]
        [XmlElement("NotCondition", typeof(NotCondition))]
        [XmlElement("SectionIf", typeof(SectionIf))]
        [XmlElement("SectionIfNot", typeof(SectionIfNot))]
        [XmlElement("EqualCondition", typeof(EqualCondition))]
        [XmlElement("GreaterThanCondition", typeof(GreaterThanCondition))]
        [XmlElement("LessThanCondition", typeof(LessThanCondition))]
        [XmlElement("NotEqualCondition", typeof(NotEqualCondition))]
        [XmlElement("GreaterThanOrEqualsCondition", typeof(GreaterThanOrEqualsCondition))]
        [XmlElement("LessThanOrEqualsCondition", typeof(LessThanOrEqualsCondition))]
        [XmlElement("ContainsCondition", typeof(ContainsCondition))]
        [XmlElement("HasAnyNChoicesCondition", typeof(HasAnyNChoicesCondition))]

        /// <remarks/>
        public object ConditionType
        {
            get
            {
                return this.conditionTypeField;
            }
            set
            {
                this.conditionTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DecisionPoint", typeof(valueBranches))]
        [System.Xml.Serialization.XmlElementAttribute("ArithmeticExpression", typeof(ArithmeticExpression))]
        [System.Xml.Serialization.XmlElementAttribute("TextExpression", typeof(TextExpression))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ComputedElementDefaultBranch
    {
        private object itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DecisionPoint", typeof(valueBranches))]
        [System.Xml.Serialization.XmlElementAttribute("ArithmeticExpression", typeof(ArithmeticExpression))]
        [System.Xml.Serialization.XmlElementAttribute("TextExpression", typeof(TextExpression))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class BaseElementContents
    {
        private string idField;

        private BaseElementContentsIsRepeatable isRepeatableField;

        private bool isRepeatableFieldSpecified;

        private string repeatGroupField;

        private string repeatCountField;

        private BaseElementContentsIsRequired isRequiredField;

        private bool isRequiredFieldSpecified;

        private string displaySequenceField;

        private CodableConcept codableConceptField;

        private string labelField;

        private string hintField;

        private Diagram[] diagramsField;

        private string voiceCommandField;

        private BaseElementContentsIsEditable editableField;

        private bool editableFieldSpecified;

        private BaseElementContentsHasPrefilled hasprefilledField;

        private bool hasprefilledFieldSpecified;

        private BaseElementContentsIsOutput outputField;

        private bool outputFieldSpecified;

        private string unitField;

        private Source sourceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public BaseElementContentsIsRepeatable IsRepeatable
        {
            get
            {
                return this.isRepeatableField;
            }
            set
            {
                this.isRepeatableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsRepeatableSpecified
        {
            get
            {
                return this.isRepeatableFieldSpecified;
            }
            set
            {
                this.isRepeatableFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RepeatGroup
        {
            get
            {
                return this.repeatGroupField;
            }
            set
            {
                this.repeatGroupField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RepeatCountField
        {
            get
            {
                return this.repeatCountField;
            }
            set
            {
                this.repeatCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public BaseElementContentsIsRequired IsRequired
        {
            get
            {
                return this.isRequiredField;
            }
            set
            {
                this.isRequiredField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsRequiredSpecified
        {
            get
            {
                return this.isRequiredFieldSpecified;
            }
            set
            {
                this.isRequiredFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public CodableConcept CodableConcept
        {
            get
            {
                return this.codableConceptField;
            }
            set
            {
                this.codableConceptField = value;
            }
        }

        /// <remarks/>
        public string Label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        /// <remarks/>
        public string Hint
        {
            get
            {
                return this.hintField;
            }
            set
            {
                this.hintField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Diagram", IsNullable = false)]
        public Diagram[] Diagrams
        {
            get
            {
                return this.diagramsField;
            }
            set
            {
                this.diagramsField = value;
            }
        }

        /// <remarks/>
        public string VoiceCommand
        {
            get
            {
                return this.voiceCommandField;
            }
            set
            {
                this.voiceCommandField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        /// <remarks/>
        public BaseElementContentsIsEditable Editable
        {
            get
            {
                return this.editableField;
            }
            set
            {
                this.editableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EditableSpecified
        {
            get
            {
                return this.editableFieldSpecified;
            }
            set
            {
                this.editableFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        /// <remarks/>
        public BaseElementContentsHasPrefilled Hasprefilled
        {
            get
            {
                return this.hasprefilledField;
            }
            set
            {
                this.hasprefilledField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HasprefilledSpecified
        {
            get
            {
                return this.hasprefilledFieldSpecified;
            }
            set
            {
                this.hasprefilledFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        /// <remarks/>
        public BaseElementContentsIsOutput Output
        {
            get
            {
                return this.outputField;
            }
            set
            {
                this.outputField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OutputSpecified
        {
            get
            {
                return this.outputFieldSpecified;
            }
            set
            {
                this.outputFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        /// <remarks/>
        public string Unit
        {
            get
            {
                return this.unitField;
            }
            set
            {
                this.unitField = value;
            }
        }

        /// <remarks/>
        public Source Source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class CodableConcept
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement("Coding")]
        public Coding[] Coding { get; set; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Coding
    {
        [XmlElement]
        public CodingAttributes System { get; set; }

        [XmlElement]
        public CodingAttributes Version { get; set; }

        [XmlElement]
        public CodingAttributes Code { get; set; }

        [XmlElement]
        public CodingAttributes Display { get; set; }

        [XmlElement]
        public CodingAttributes UserSelected { get; set; }

        [XmlElement]
        public CodingAttributes Url { get; set; }
    }

    public partial class CodingAttributes
    {
        [XmlAttribute]
        public string Value { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Diagram
    {
        private string idField;

        private string locationField;

        private string labelField;

        private string valueField;

        private string displaySequenceField;

        private DiagramIsKeyDiagram isKeyDiagramField;

        private bool isKeyDiagramFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        /// <remarks/>
        public string Label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DiagramIsKeyDiagram IsKeyDiagram
        {
            get
            {
                return this.isKeyDiagramField;
            }
            set
            {
                this.isKeyDiagramField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsKeyDiagramSpecified
        {
            get
            {
                return this.isKeyDiagramFieldSpecified;
            }
            set
            {
                this.isKeyDiagramFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum DiagramIsKeyDiagram
    {
        /// <remarks/>
        @true,

        /// <remarks/>
        @false,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum BaseElementContentsIsRequired
    {
        /// <remarks/>
        @true,

        /// <remarks/>
        @false,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum BaseElementContentsHasPrefilled
    {
        /// <remarks/>
        @true,

        /// <remarks/>
        @false,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum BaseElementContentsIsEditable
    {
        /// <remarks/>
        @true,

        /// <remarks/>
        @false,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum BaseElementContentsIsOutput
    {
        /// <remarks/>
        @true,

        /// <remarks/>
        @false,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum BaseShowDurationAttributes
    {
        /// <remarks/>
        @true,

        /// <remarks/>
        @false,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum BaseElementContentsIsRepeatable
    {
        /// <remarks/>
        @true,

        /// <remarks/>
        @false,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Contact
    {
        private string nameField;

        private string emailField;

        private string institutionField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <remarks/>
        public string Institution
        {
            get
            {
                return this.institutionField;
            }
            set
            {
                this.institutionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Ontology
    {
        private AnatomicRegions[] anatomicRegionsField;

        private PossibleDiagnoses[] possibleDiagnosesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AnatomicRegions")]
        public AnatomicRegions[] AnatomicRegions
        {
            get
            {
                return this.anatomicRegionsField;
            }
            set
            {
                this.anatomicRegionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PossibleDiagnoses")]
        public PossibleDiagnoses[] PossibleDiagnoses
        {
            get
            {
                return this.possibleDiagnosesField;
            }
            set
            {
                this.possibleDiagnosesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class AnatomicRegions
    {
        private string codingSystemField;

        private Region[] regionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CodingSystem
        {
            get
            {
                return this.codingSystemField;
            }
            set
            {
                this.codingSystemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Region")]
        public Region[] Region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Region
    {
        private string codeField;

        private string urlField;

        private string textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Url
        {
            get
            {
                return this.urlField;
            }
            set
            {
                this.urlField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class PossibleDiagnoses
    {
        private string codingSystemField;

        private Diagnosis[] diagnosisField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CodingSystem
        {
            get
            {
                return this.codingSystemField;
            }
            set
            {
                this.codingSystemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Diagnosis")]
        public Diagnosis[] Diagnosis
        {
            get
            {
                return this.diagnosisField;
            }
            set
            {
                this.diagnosisField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Diagnosis
    {
        private string codingSystemField;

        private string codeField;

        private string urlField;

        private string textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CodingSystem
        {
            get
            {
                return this.codingSystemField;
            }
            set
            {
                this.codingSystemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Url
        {
            get
            {
                return this.urlField;
            }
            set
            {
                this.urlField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ApplicableExamCategory
    {
        private ApplicableExamCategoryAxis axisField;

        private string textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ApplicableExamCategoryAxis Axis
        {
            get
            {
                return this.axisField;
            }
            set
            {
                this.axisField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum ApplicableExamCategoryAxis
    {
        /// <remarks/>
        Modality,

        /// <remarks/>
        Anatomy,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ApplicableSexes
    {
        private ApplicableSexesValue valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ApplicableSexesValue Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum ApplicableSexesValue
    {
        /// <remarks/>
        Male,

        /// <remarks/>
        Female,

        /// <remarks/>
        Both,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ApplicableAgeGroups
    {
        private string minimumAgeField;

        private string maximumAgeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string MinimumAge
        {
            get
            {
                return this.minimumAgeField;
            }
            set
            {
                this.minimumAgeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string MaximumAge
        {
            get
            {
                return this.maximumAgeField;
            }
            set
            {
                this.maximumAgeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class TextCues
    {
        private string[] contextPhrasesField;

        private string[] keyWordsField;

        private string[] negationPhrasesField;

        private string regexField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ContextPhrase", IsNullable = false)]
        public string[] ContextPhrases
        {
            get
            {
                return this.contextPhrasesField;
            }
            set
            {
                this.contextPhrasesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("KeyWord", IsNullable = false)]
        public string[] KeyWords
        {
            get
            {
                return this.keyWordsField;
            }
            set
            {
                this.keyWordsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("NegationPhrase", IsNullable = false)]
        public string[] NegationPhrases
        {
            get
            {
                return this.negationPhrasesField;
            }
            set
            {
                this.negationPhrasesField = value;
            }
        }

        /// <remarks/>
        public string Regex
        {
            get
            {
                return this.regexField;
            }
            set
            {
                this.regexField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class KeyValue
    {

        private string labelField;

        private KeyValueValue valueField;

        private string idField;

        /// <remarks/>
        public string Label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        /// <remarks/>
        public KeyValueValue Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class KeyValueValue
    {

        private ValueElement[] valueElementField;

        private string[] textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ValueElement")]
        public ValueElement[] ValueElement
        {
            get
            {
                return this.valueElementField;
            }
            set
            {
                this.valueElementField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ValueElement
    {

        private string labelField;

        private string valueField;

        /// <remarks/>
        public string Label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Rules
    {
        private RulesDecisionPoint decisionPointField;

        /// <remarks/>
        public RulesDecisionPoint DecisionPoint
        {
            get
            {
                return this.decisionPointField;
            }
            set
            {
                this.decisionPointField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RulesDecisionPoint
    {
        private string idField;

        private string labelField;

        private string descriptionField;

        private RulesDecisionPointBranch[] branchField;

        private RulesDecisionPointDefaultBranch defaultBranchField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Branch")]
        public RulesDecisionPointBranch[] Branch
        {
            get
            {
                return this.branchField;
            }
            set
            {
                this.branchField = value;
            }
        }

        /// <remarks/>
        public RulesDecisionPointDefaultBranch DefaultBranch
        {
            get
            {
                return this.defaultBranchField;
            }
            set
            {
                this.defaultBranchField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RulesDecisionPointBranch
    {
        private string labelField;

        private object conditionTypeField;

        private object itemField;

        private RulesDecisionPointBranch branchField;

        /// <remarks/>
        public string Label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        [XmlElement("AndCondition", typeof(AndCondition))]
        [XmlElement("OrCondition", typeof(OrCondition))]
        [XmlElement("NotCondition", typeof(NotCondition))]
        [XmlElement("SectionIf", typeof(SectionIf))]
        [XmlElement("SectionIfNot", typeof(SectionIfNot))]
        [XmlElement("EqualCondition", typeof(EqualCondition))]
        [XmlElement("GreaterThanCondition", typeof(GreaterThanCondition))]
        [XmlElement("LessThanCondition", typeof(LessThanCondition))]
        [XmlElement("NotEqualCondition", typeof(NotEqualCondition))]
        [XmlElement("GreaterThanOrEqualsCondition", typeof(GreaterThanOrEqualsCondition))]
        [XmlElement("LessThanOrEqualsCondition", typeof(LessThanOrEqualsCondition))]
        [XmlElement("ContainsCondition", typeof(ContainsCondition))]
        [XmlElement("HasAnyNChoicesCondition", typeof(HasAnyNChoicesCondition))]
        /// <remarks/>
        public object ConditionType
        {
            get
            {
                return this.conditionTypeField;
            }
            set
            {
                this.conditionTypeField = value;
            }
        }


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DecisionPoint", typeof(RulesDecisionPoint))]
        [System.Xml.Serialization.XmlElementAttribute("EndPointRef", typeof(EndPointRef))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        [System.Xml.Serialization.XmlElement("Branch")]
        public RulesDecisionPointBranch DecisionPointBranch
        {
            get
            {
                return this.branchField;
            }
            set
            {
                this.branchField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RulesDecisionPointDefaultBranch
    {
        private string labelField;

        private object itemField;

        /// <remarks/>
        public string Label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DecisionPoint", typeof(RulesDecisionPoint))]
        [System.Xml.Serialization.XmlElementAttribute("EndPointRef", typeof(EndPointRef))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class EndpointBranch
    {
        private string labelField;

        private object conditionTypeField;

        private EndpointBranch branchField;

        private ReportText[] reportTextField;

        /// <remarks/>
        public string Label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        [XmlElement("AndCondition", typeof(AndCondition))]
        [XmlElement("OrCondition", typeof(OrCondition))]
        [XmlElement("NotCondition", typeof(NotCondition))]
        [XmlElement("SectionIf", typeof(SectionIf))]
        [XmlElement("SectionIfNot", typeof(SectionIfNot))]
        [XmlElement("EqualCondition", typeof(EqualCondition))]
        [XmlElement("GreaterThanCondition", typeof(GreaterThanCondition))]
        [XmlElement("LessThanCondition", typeof(LessThanCondition))]
        [XmlElement("NotEqualCondition", typeof(NotEqualCondition))]
        [XmlElement("GreaterThanOrEqualsCondition", typeof(GreaterThanOrEqualsCondition))]
        [XmlElement("LessThanOrEqualsCondition", typeof(LessThanOrEqualsCondition))]
        [XmlElement("ContainsCondition", typeof(ContainsCondition))]
        [XmlElement("HasAnyNChoicesCondition", typeof(HasAnyNChoicesCondition))]
        /// <remarks/>
        public object ConditionType
        {
            get
            {
                return this.conditionTypeField;
            }
            set
            {
                this.conditionTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("ReportText")]
        public ReportText[] ReportText
        {
            get
            {
                return this.reportTextField;
            }
            set
            {
                this.reportTextField = value;
            }
        }

        [System.Xml.Serialization.XmlElement("Branch")]
        public EndpointBranch EndpointInnerBranch
        {
            get
            {
                return this.branchField;
            }
            set
            {
                this.branchField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class EndPointRef
    {
        private string endPointIdField;

        private string diagramIdIdField;

        private BaseElementContentsIsRepeatable isRepeatableField;

        private bool isRepeatableFieldSpecified;

        private string repeatGroupField;

        private string repeatCountField;

        private string labelField;

        private string descriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "IDREF")]
        public string EndPointId
        {
            get
            {
                return this.endPointIdField;
            }
            set
            {
                this.endPointIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "IDREF")]
        public string DiagramId
        {
            get
            {
                return this.diagramIdIdField;
            }
            set
            {
                this.diagramIdIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public BaseElementContentsIsRepeatable IsRepeatable
        {
            get
            {
                return this.isRepeatableField;
            }
            set
            {
                this.isRepeatableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsRepeatableSpecified
        {
            get
            {
                return this.isRepeatableFieldSpecified;
            }
            set
            {
                this.isRepeatableFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string RepeatGroup
        {
            get
            {
                return this.repeatGroupField;
            }
            set
            {
                this.repeatGroupField = value;
            }
        }

        /// <remarks/>
        public string RepeatCount
        {
            get
            {
                return this.repeatCountField;
            }
            set
            {
                this.repeatCountField = value;
            }
        }

        /// <remarks/>
        public string Label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class EndPoints
    {
        private TemplatePartial[] templatePartialField;

        private EndPoint[] endPointField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("TemplatePartial")]
        public TemplatePartial[] TemplatePartial
        {
            get
            {
                return this.templatePartialField;
            }
            set
            {
                this.templatePartialField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("EndPoint")]
        public EndPoint[] EndPoint
        {
            get
            {
                return this.endPointField;
            }
            set
            {
                this.endPointField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class TemplatePartial
    {
        private string idField;

        private EndpointBranch[] branchField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Branch")]
        public EndpointBranch[] EndPointBranch
        {
            get
            {
                return this.branchField;
            }
            set
            {
                this.branchField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class EndPoint
    {
        private string labelField;

        private Diagnosis diagnosisField;

        private ReportSections reportSectionsField;

        private ActionableFinding actionableFindingField;

        private ImagingFollowup imagingFollowupField;

        private string idField;

        /// <remarks/>
        public string Label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        /// <remarks/>
        public Diagnosis Diagnosis
        {
            get
            {
                return this.diagnosisField;
            }
            set
            {
                this.diagnosisField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ReportSections")]
        public ReportSections ReportSections
        {
            get
            {
                return this.reportSectionsField;
            }
            set
            {
                this.reportSectionsField = value;
            }
        }

        /// <remarks/>
        public ActionableFinding ActionableFinding
        {
            get
            {
                return this.actionableFindingField;
            }
            set
            {
                this.actionableFindingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ImagingFollowup")]
        public ImagingFollowup ImagingFollowup
        {
            get
            {
                return this.imagingFollowupField;
            }
            set
            {
                this.imagingFollowupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ReportSections
    {
        private ReportSection[] reportSectionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ReportSection")]
        public ReportSection[] ReportSection
        {
            get
            {
                return this.reportSectionField;
            }
            set
            {
                this.reportSectionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ReportSection
    {
        private string sectionIdField;

        private EndpointBranch[] branchField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SectionId
        {
            get
            {
                return this.sectionIdField;
            }
            set
            {
                this.sectionIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Branch")]
        public EndpointBranch[] Branch
        {
            get
            {
                return this.branchField;
            }
            set
            {
                this.branchField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ReportText
    {
        private string typeField;

        private string valueField;

        private string textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ActionableFinding
    {
        private string categoryField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ImagingFollowup
    {
        private RecommendationBase recommendationBaseField;

        private PreferredImagingExam preferredImagingExamField;

        private Exam[] acceptableImagingExamsField;

        private IndicationForFollowup indicationForFollowupField;

        private RecommendedTimeFrame recommendedTimeFrameField;

        /// <remarks/>
        public RecommendationBase RecommendationBase
        {
            get
            {
                return this.recommendationBaseField;
            }
            set
            {
                this.recommendationBaseField = value;
            }
        }

        /// <remarks/>
        public PreferredImagingExam PreferredImagingExam
        {
            get
            {
                return this.preferredImagingExamField;
            }
            set
            {
                this.preferredImagingExamField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Exam", IsNullable = false)]
        public Exam[] AcceptableImagingExams
        {
            get
            {
                return this.acceptableImagingExamsField;
            }
            set
            {
                this.acceptableImagingExamsField = value;
            }
        }

        /// <remarks/>
        public IndicationForFollowup IndicationForFollowup
        {
            get
            {
                return this.indicationForFollowupField;
            }
            set
            {
                this.indicationForFollowupField = value;
            }
        }

        /// <remarks/>
        public RecommendedTimeFrame RecommendedTimeFrame
        {
            get
            {
                return this.recommendedTimeFrameField;
            }
            set
            {
                this.recommendedTimeFrameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class EvidenceLevel
    {
        private string codingSystemField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CodingSystem
        {
            get
            {
                return this.codingSystemField;
            }
            set
            {
                this.codingSystemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute(DataType = "token")]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class RecommendationBase
    {
        private string clinicalConditionField;

        private Citation[] referencesField;

        private EvidenceLevel evidenceLevelField;

        /// <remarks/>
        public string ClinicalCondition
        {
            get
            {
                return this.clinicalConditionField;
            }
            set
            {
                this.clinicalConditionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Citation", IsNullable = false)]
        public Citation[] References
        {
            get
            {
                return this.referencesField;
            }
            set
            {
                this.referencesField = value;
            }
        }

        /// <remarks/>
        public EvidenceLevel EvidenceLevel
        {
            get
            {
                return this.evidenceLevelField;
            }
            set
            {
                this.evidenceLevelField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class PreferredImagingExam
    {
        private string clinicalConditionField;

        private Citation[] referencesField;

        private EvidenceLevel evidenceLevelField;

        /// <remarks/>
        public string ClinicalCondition
        {
            get
            {
                return this.clinicalConditionField;
            }
            set
            {
                this.clinicalConditionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Citation", IsNullable = false)]
        public Citation[] References
        {
            get
            {
                return this.referencesField;
            }
            set
            {
                this.referencesField = value;
            }
        }

        /// <remarks/>
        public EvidenceLevel EvidenceLevel
        {
            get
            {
                return this.evidenceLevelField;
            }
            set
            {
                this.evidenceLevelField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Exam
    {
        private string codeField;

        private string codeSystemField;

        private string modalityField;

        private string bodyRegionField;

        private string textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CodeSystem
        {
            get
            {
                return this.codeSystemField;
            }
            set
            {
                this.codeSystemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Modality
        {
            get
            {
                return this.modalityField;
            }
            set
            {
                this.modalityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string BodyRegion
        {
            get
            {
                return this.bodyRegionField;
            }
            set
            {
                this.bodyRegionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class IndicationForFollowup
    {
        private string codingSystemField;

        private string codeField;

        private string textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CodingSystem
        {
            get
            {
                return this.codingSystemField;
            }
            set
            {
                this.codingSystemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class RecommendedTimeFrame
    {
        private string earliestField;

        private string latestField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "duration")]
        public string Earliest
        {
            get
            {
                return this.earliestField;
            }
            set
            {
                this.earliestField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "duration")]
        public string Latest
        {
            get
            {
                return this.latestField;
            }
            set
            {
                this.latestField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class References
    {

        private Citation[] citationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Citation")]
        public Citation[] Citation
        {
            get
            {
                return this.citationField;
            }
            set
            {
                this.citationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ApplicableExams
    {

        private ApplicableExamCategory[] applicableExamCategoryField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ApplicableExamCategory")]
        public ApplicableExamCategory[] ApplicableExamCategory
        {
            get
            {
                return this.applicableExamCategoryField;
            }
            set
            {
                this.applicableExamCategoryField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ContextPhrases
    {

        private string[] contextPhraseField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ContextPhrase")]
        public string[] ContextPhrase
        {
            get
            {
                return this.contextPhraseField;
            }
            set
            {
                this.contextPhraseField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class KeyWords
    {

        private string[] keyWordField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("KeyWord")]
        public string[] KeyWord
        {
            get
            {
                return this.keyWordField;
            }
            set
            {
                this.keyWordField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class NegationPhrases
    {

        private string[] negationPhraseField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("NegationPhrase")]
        public string[] NegationPhrase
        {
            get
            {
                return this.negationPhraseField;
            }
            set
            {
                this.negationPhraseField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class VoiceActivation
    {

        private string[] voiceCommandPhraseField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("VoiceCommandPhrase")]
        public string[] VoiceCommandPhrase
        {
            get
            {
                return this.voiceCommandPhraseField;
            }
            set
            {
                this.voiceCommandPhraseField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class AdditionalInfo
    {

        private KeyValue[] keyValueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("KeyValue")]
        public KeyValue[] KeyValue
        {
            get
            {
                return this.keyValueField;
            }
            set
            {
                this.keyValueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Modality
    {

        private string codingSystemField;

        private string codeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CodingSystem
        {
            get
            {
                return this.codingSystemField;
            }
            set
            {
                this.codingSystemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class BodyRegion
    {

        private string codingSystemField;

        private string codeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CodingSystem
        {
            get
            {
                return this.codingSystemField;
            }
            set
            {
                this.codingSystemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class DataElements
    {

        private object[] dataElementTypesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DataElementTypes")]
        public object[] DataElementTypes
        {
            get
            {
                return this.dataElementTypesField;
            }
            set
            {
                this.dataElementTypesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class GlobalValue
    {
        private string idField;

        private string textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Diagrams
    {

        private Diagram[] diagramField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Diagram")]
        public Diagram[] Diagram
        {
            get
            {
                return this.diagramField;
            }
            set
            {
                this.diagramField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class TextExpression
    {
        private InsertValue[] insertValueField;

        private string textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("InsertValue")]
        public InsertValue[] InsertValue
        {
            get
            {
                return this.insertValueField;
            }
            set
            {
                this.insertValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ArithmeticExpression
    {
        private string textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class InsertValue
    {
        private string dataElementIdField;

        private string significantDigitsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "IDREF")]
        public string DataElementId
        {
            get
            {
                return this.dataElementIdField;
            }
            set
            {
                this.dataElementIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string SignificantDigits
        {
            get
            {
                return this.significantDigitsField;
            }
            set
            {
                this.significantDigitsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ComputedDataElement
    {
        private string idField;

        private string displaySequenceField;

        private ComputedDataElementShowValue showValueField;

        private bool showValueFieldSpecified;

        private string labelField;

        private string hintField;

        private Diagram[] diagramsField;

        private object itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ComputedDataElementShowValue ShowValue
        {
            get
            {
                return this.showValueField;
            }
            set
            {
                this.showValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowValueSpecified
        {
            get
            {
                return this.showValueFieldSpecified;
            }
            set
            {
                this.showValueFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string Label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        /// <remarks/>
        public string Hint
        {
            get
            {
                return this.hintField;
            }
            set
            {
                this.hintField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Diagram", IsNullable = false)]
        public Diagram[] Diagrams
        {
            get
            {
                return this.diagramsField;
            }
            set
            {
                this.diagramsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DecisionPoint", typeof(valueBranches))]
        [System.Xml.Serialization.XmlElementAttribute("ArithmeticExpression", typeof(ArithmeticExpression))]
        [System.Xml.Serialization.XmlElementAttribute("TextExpression", typeof(TextExpression))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum ComputedDataElementShowValue
    {

        /// <remarks/>
        @true,

        /// <remarks/>
        @false,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class NumericDataElement : BaseElementContents
    {
        private decimal minimumField;

        private bool minimumFieldSpecified;

        private decimal maximumField;

        private bool maximumFieldSpecified;

        private NumericConditionalProperties conditionalPropertiesField;

        /// <remarks/>
        public decimal Minimum
        {
            get
            {
                return this.minimumField;
            }
            set
            {
                this.minimumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MinimumSpecified
        {
            get
            {
                return this.minimumFieldSpecified;
            }
            set
            {
                this.minimumFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal Maximum
        {
            get
            {
                return this.maximumField;
            }
            set
            {
                this.maximumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MaximumSpecified
        {
            get
            {
                return this.maximumFieldSpecified;
            }
            set
            {
                this.maximumFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ConditionalProperties")]
        public NumericConditionalProperties ConditionalProperties
        {
            get
            {
                return this.conditionalPropertiesField;
            }
            set
            {
                this.conditionalPropertiesField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class DateTimeDataElement : BaseElementContents
    {
        private string minimumDateField;

        private string maximumDateField;

        private DateTimeConditionalProperties conditionalPropertiesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string MinimumDate
        {
            get
            {
                return this.minimumDateField;
            }
            set
            {
                this.minimumDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string MaximumDate
        {
            get
            {
                return this.maximumDateField;
            }
            set
            {
                this.maximumDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ConditionalProperties")]
        public DateTimeConditionalProperties ConditionalProperties
        {
            get
            {
                return this.conditionalPropertiesField;
            }
            set
            {
                this.conditionalPropertiesField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class TimeSpanDataElement : BaseElementContents
    {
        private BaseShowDurationAttributes showDaysField;

        private bool showDaysFieldSpecified;

        private BaseShowDurationAttributes showHoursField;

        private bool showHoursFieldSpecified;

        private BaseShowDurationAttributes showMinutesField;

        private bool showMinutesFieldSpecified;

        private BaseShowDurationAttributes showSecondsField;

        private bool showSecondsFieldSpecified;

        private string minimumDayField;

        private string minimumHoursField;

        private string minimumMinutesField;

        private string minimumSecondsField;

        private string maximumDayField;

        private string maximumHoursField;

        private string maximumMinutesField;

        private string maximumSecondsField;

        private DurationConditionalProperties conditionalPropertiesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public BaseShowDurationAttributes ShowDays
        {
            get
            {
                return this.showDaysField;
            }
            set
            {
                this.showDaysField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowDaysSpecified
        {
            get
            {
                return this.showDaysFieldSpecified;
            }
            set
            {
                this.showDaysFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public BaseShowDurationAttributes ShowHours
        {
            get
            {
                return this.showHoursField;
            }
            set
            {
                this.showHoursField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowHoursSpecified
        {
            get
            {
                return this.showHoursFieldSpecified;
            }
            set
            {
                this.showHoursFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public BaseShowDurationAttributes ShowMinutes
        {
            get
            {
                return this.showMinutesField;
            }
            set
            {
                this.showMinutesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowMinutesSpecified
        {
            get
            {
                return this.showMinutesFieldSpecified;
            }
            set
            {
                this.showMinutesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public BaseShowDurationAttributes ShowSeconds
        {
            get
            {
                return this.showSecondsField;
            }
            set
            {
                this.showSecondsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowSecondsSpecified
        {
            get
            {
                return this.showSecondsFieldSpecified;
            }
            set
            {
                this.showSecondsFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string MinimumDay
        {
            get
            {
                return this.minimumDayField;
            }
            set
            {
                this.minimumDayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string MaximumDay
        {
            get
            {
                return this.maximumDayField;
            }
            set
            {
                this.maximumDayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string MinimumHours
        {
            get
            {
                return this.minimumHoursField;
            }
            set
            {
                this.minimumHoursField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string MaximumHours
        {
            get
            {
                return this.maximumHoursField;
            }
            set
            {
                this.maximumHoursField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string MinimumMinutes
        {
            get
            {
                return this.minimumMinutesField;
            }
            set
            {
                this.minimumMinutesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string MaximumMinutes
        {
            get
            {
                return this.maximumMinutesField;
            }
            set
            {
                this.maximumMinutesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string MinimumSeconds
        {
            get
            {
                return this.minimumSecondsField;
            }
            set
            {
                this.minimumSecondsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string MaximumSeconds
        {
            get
            {
                return this.maximumSecondsField;
            }
            set
            {
                this.maximumSecondsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ConditionalProperties")]
        public DurationConditionalProperties ConditionalProperties
        {
            get
            {
                return this.conditionalPropertiesField;
            }
            set
            {
                this.conditionalPropertiesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class IntegerDataElement : BaseElementContents
    {
        private string minimumField;

        private string maximumField;

        private IntegerConditionalProperties conditionalPropertiesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string Minimum
        {
            get
            {
                return this.minimumField;
            }
            set
            {
                this.minimumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string Maximum
        {
            get
            {
                return this.maximumField;
            }
            set
            {
                this.maximumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ConditionalProperties")]
        public IntegerConditionalProperties ConditionalProperties
        {
            get
            {
                return this.conditionalPropertiesField;
            }
            set
            {
                this.conditionalPropertiesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Choice
    {
        private string valueField;

        private string labelField;

        private string hintField;

        private string voiceCommandField;

        private string reportTextField;

        private ChoiceIsDefault isDefaultField;

        private bool isDefaultFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        public string Label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        /// <remarks/>
        public string Hint
        {
            get
            {
                return this.hintField;
            }
            set
            {
                this.hintField = value;
            }
        }

        /// <remarks/>
        public string VoiceCommand
        {
            get
            {
                return this.voiceCommandField;
            }
            set
            {
                this.voiceCommandField = value;
            }
        }

        /// <remarks/>
        public string ReportText
        {
            get
            {
                return this.reportTextField;
            }
            set
            {
                this.reportTextField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ChoiceIsDefault IsDefault
        {
            get
            {
                return this.isDefaultField;
            }
            set
            {
                this.isDefaultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsDefaultSpecified
        {
            get
            {
                return this.isDefaultFieldSpecified;
            }
            set
            {
                this.isDefaultFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum ChoiceIsDefault
    {

        /// <remarks/>
        @true,

        /// <remarks/>
        @false,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ChoiceInfo
    {
        private Choice choiceField;

        private Choice[] choice1Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public Choice Choice
        {
            get
            {
                return this.choiceField;
            }
            set
            {
                this.choiceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Choice", Order = 1)]
        public Choice[] Choices
        {
            get
            {
                return choice1Field;
            }
            set
            {
                choice1Field = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ImageMap
    {
        private string locationField;

        private string labelField;

        private DrawStyle drawStyleField;

        private Area[] mapField;

        /// <remarks/>
        public string Location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        /// <remarks/>
        public string Label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        /// <remarks/>
        public DrawStyle DrawStyle
        {
            get
            {
                return this.drawStyleField;
            }
            set
            {
                this.drawStyleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Area", IsNullable = false)]
        public Area[] Map
        {
            get
            {
                return this.mapField;
            }
            set
            {
                this.mapField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class DrawStyle
    {
        private string outlineField;

        private string hoverFillField;

        private string selectedFillField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Outline
        {
            get
            {
                return this.outlineField;
            }
            set
            {
                this.outlineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string HoverFill
        {
            get
            {
                return this.hoverFillField;
            }
            set
            {
                this.hoverFillField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SelectedFill
        {
            get
            {
                return this.selectedFillField;
            }
            set
            {
                this.selectedFillField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Area
    {
        private AreaShape shapeField;

        private string coordsField;

        private string choiceValueField;

        private string outlineField;

        private string hoverFillField;

        private string selectedFillField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public AreaShape Shape
        {
            get
            {
                return this.shapeField;
            }
            set
            {
                this.shapeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Coords
        {
            get
            {
                return this.coordsField;
            }
            set
            {
                this.coordsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        public string ChoiceValue
        {
            get
            {
                return this.choiceValueField;
            }
            set
            {
                this.choiceValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Outline
        {
            get
            {
                return this.outlineField;
            }
            set
            {
                this.outlineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string HoverFill
        {
            get
            {
                return this.hoverFillField;
            }
            set
            {
                this.hoverFillField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SelectedFill
        {
            get
            {
                return this.selectedFillField;
            }
            set
            {
                this.selectedFillField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum AreaShape
    {
        /// <remarks/>
        rect,

        /// <remarks/>
        poly,

        /// <remarks/>
        circle,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Map
    {

        private Area[] areaField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Area")]
        public Area[] Area
        {
            get
            {
                return this.areaField;
            }
            set
            {
                this.areaField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ChoiceDataElement : BaseChoiceElementContents
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum ChoiceDataElementAllowFreetext
    {

        /// <remarks/>
        @true,

        /// <remarks/>
        @false,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class MultiChoiceDataElement : BaseChoiceElementContents
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class AndCondition : ConditionTypes
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class OrCondition : ConditionTypes
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class NotCondition : ConditionTypes
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class SectionIf : BaseSectionIfConditionContent
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class SectionIfNot : BaseSectionIfConditionContent
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class EqualCondition : ComparisonConditionContents
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class GreaterThanCondition : ComparisonConditionContents
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class LessThanCondition : ComparisonConditionContents
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class NotEqualCondition : ComparisonConditionContents
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class GreaterThanOrEqualsCondition : ComparisonConditionContents
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class LessThanOrEqualsCondition : ComparisonConditionContents
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ContainsCondition : ComparisonConditionContents
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class HasAnyNChoicesCondition
    {
        private string dataElementIdField;

        private string minimumChoicesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "IDREF")]
        public string DataElementId
        {
            get
            {
                return this.dataElementIdField;
            }
            set
            {
                this.dataElementIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "positiveInteger")]
        public string MinimumChoices
        {
            get
            {
                return this.minimumChoicesField;
            }
            set
            {
                this.minimumChoicesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ChoiceRef
    {

        private string dataElementIdField;

        private string choiceValueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "IDREF")]
        public string DataElementId
        {
            get
            {
                return this.dataElementIdField;
            }
            set
            {
                this.dataElementIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        public string ChoiceValue
        {
            get
            {
                return this.choiceValueField;
            }
            set
            {
                this.choiceValueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class AcceptableImagingExams
    {

        private Exam[] examField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Exam")]
        public Exam[] Exam
        {
            get
            {
                return this.examField;
            }
            set
            {
                this.examField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class InsertPartial
    {
        private string partialIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "IDREF")]
        public string PartialId
        {
            get
            {
                return this.partialIdField;
            }
            set
            {
                this.partialIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Source
    {
        private string vendorField;

        private string appNameField;

        private string versionField;

        public string Vendor
        {
            get
            {
                return this.vendorField;
            }
            set
            {
                this.vendorField = value;
            }
        }

        public string AppName
        {
            get
            {
                return this.appNameField;
            }
            set
            {
                this.appNameField = value;
            }
        }

        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TextTemplateContent
    {
        private string[] textField;

        private TemplateTags[] templateTagsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public TemplateTags[] TemplateTags
        {
            get
            {
                return this.templateTagsField;
            }
            set
            {
                this.templateTagsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TemplateTags
    {
        private object textItems;

        /// <remarks/>
        [XmlElement("InsertValue", typeof(InsertValue))]
        [XmlElement("InsertPartial", typeof(InsertPartial))]
        [XmlElement("PlainText", typeof(PlainText))]
        [XmlElement("NewLine", typeof(NewLine))]
        public object TextItem
        {
            get
            {
                return this.textItems;
            }
            set
            {
                this.textItems = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class NewLine
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PlainText
    {
        private string textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class NumericConditionalProperties
    {
        private NumericConditionalProperty[] conditionalPropertyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ConditionalProperty")]
        public NumericConditionalProperty[] ConditionalProperty
        {
            get
            {
                return this.conditionalPropertyField;
            }
            set
            {
                this.conditionalPropertyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class IntegerConditionalProperties
    {
        private IntegerConditionalProperty[] conditionalPropertyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ConditionalProperty")]
        public IntegerConditionalProperty[] ConditionalProperty
        {
            get
            {
                return this.conditionalPropertyField;
            }
            set
            {
                this.conditionalPropertyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class DateTimeConditionalProperties
    {
        private DateTimeConditionalProperty[] conditionalPropertyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ConditionalProperty")]
        public DateTimeConditionalProperty[] ConditionalProperty
        {
            get
            {
                return this.conditionalPropertyField;
            }
            set
            {
                this.conditionalPropertyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class DurationConditionalProperties
    {
        private DurationConditionalProperty[] conditionalPropertyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ConditionalProperty")]
        public DurationConditionalProperty[] ConditionalProperty
        {
            get
            {
                return this.conditionalPropertyField;
            }
            set
            {
                this.conditionalPropertyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ChoiceConditionalProperties
    {
        private ChoiceConditionalProperty[] conditionalPropertyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ConditionalProperty")]
        public ChoiceConditionalProperty[] ConditionalProperty
        {
            get
            {
                return this.conditionalPropertyField;
            }
            set
            {
                this.conditionalPropertyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class BaseConditionalPropertyContents
    {
        private object conditionTypeField;

        private string isRelevantField;

        private string isRequiredField;

        private string displaySequenceField;

        [XmlElement("AndCondition", typeof(AndCondition))]
        [XmlElement("OrCondition", typeof(OrCondition))]
        [XmlElement("NotCondition", typeof(NotCondition))]
        [XmlElement("SectionIf", typeof(SectionIf))]
        [XmlElement("SectionIfNot", typeof(SectionIfNot))]
        [XmlElement("EqualCondition", typeof(EqualCondition))]
        [XmlElement("GreaterThanCondition", typeof(GreaterThanCondition))]
        [XmlElement("LessThanCondition", typeof(LessThanCondition))]
        [XmlElement("NotEqualCondition", typeof(NotEqualCondition))]
        [XmlElement("GreaterThanOrEqualsCondition", typeof(GreaterThanOrEqualsCondition))]
        [XmlElement("LessThanOrEqualsCondition", typeof(LessThanOrEqualsCondition))]
        [XmlElement("ContainsCondition", typeof(ContainsCondition))]
        [XmlElement("HasAnyNChoicesCondition", typeof(HasAnyNChoicesCondition))]
        /// <remarks/>
        public object ConditionType
        {
            get
            {
                return this.conditionTypeField;
            }
            set
            {
                this.conditionTypeField = value;
            }
        }

        public string IsRelevant
        {
            get
            {
                return this.isRelevantField;
            }
            set
            {
                this.isRelevantField = value;
            }
        }

        public string IsRequired
        {
            get
            {
                return this.isRequiredField;
            }
            set
            {
                this.isRequiredField = value;
            }
        }

        public string DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ChoiceConditionalProperty : BaseConditionalPropertyContents
    {
        private ChoiceNotRelevant[] choiceNotRelevantField;

        [System.Xml.Serialization.XmlElementAttribute("ChoiceNotRelevant")]
        public ChoiceNotRelevant[] NotRelevantChoice
        {
            get
            {
                return this.choiceNotRelevantField;
            }
            set
            {
                this.choiceNotRelevantField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class NumericConditionalProperty : BaseConditionalPropertyContents
    {
        private decimal minimumField;

        private bool minimumFieldSpecified;

        private decimal maximumField;

        private bool maximumFieldSpecified;

        public decimal Minimum
        {
            get
            {
                return this.minimumField;
            }
            set
            {
                this.minimumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MinimumSpecified
        {
            get
            {
                return this.minimumFieldSpecified;
            }
            set
            {
                this.minimumFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal Maximum
        {
            get
            {
                return this.maximumField;
            }
            set
            {
                this.maximumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MaximumSpecified
        {
            get
            {
                return this.maximumFieldSpecified;
            }
            set
            {
                this.maximumFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class IntegerConditionalProperty : BaseConditionalPropertyContents
    {
        private string minimumField;

        private bool minimumFieldSpecified;

        private string maximumField;

        private bool maximumFieldSpecified;

        public string Minimum
        {
            get
            {
                return this.minimumField;
            }
            set
            {
                this.minimumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MinimumSpecified
        {
            get
            {
                return this.minimumFieldSpecified;
            }
            set
            {
                this.minimumFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string Maximum
        {
            get
            {
                return this.maximumField;
            }
            set
            {
                this.maximumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MaximumSpecified
        {
            get
            {
                return this.maximumFieldSpecified;
            }
            set
            {
                this.maximumFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class DateTimeConditionalProperty : BaseConditionalPropertyContents
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class DurationConditionalProperty : BaseConditionalPropertyContents
    {
        private string minimumDayField;

        private string minimumHoursField;

        private string minimumMinutesField;

        private string minimumSecondsField;

        private string maximumDayField;

        private string maximumHoursField;

        private string maximumMinutesField;

        private string maximumSecondsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string MinimumDay
        {
            get
            {
                return this.minimumDayField;
            }
            set
            {
                this.minimumDayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string MaximumDay
        {
            get
            {
                return this.maximumDayField;
            }
            set
            {
                this.maximumDayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string MinimumHours
        {
            get
            {
                return this.minimumHoursField;
            }
            set
            {
                this.minimumHoursField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string MaximumHours
        {
            get
            {
                return this.maximumHoursField;
            }
            set
            {
                this.maximumHoursField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string MinimumMinutes
        {
            get
            {
                return this.minimumMinutesField;
            }
            set
            {
                this.minimumMinutesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string MaximumMinutes
        {
            get
            {
                return this.maximumMinutesField;
            }
            set
            {
                this.maximumMinutesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string MinimumSeconds
        {
            get
            {
                return this.minimumSecondsField;
            }
            set
            {
                this.minimumSecondsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string MaximumSeconds
        {
            get
            {
                return this.maximumSecondsField;
            }
            set
            {
                this.maximumSecondsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ChoiceNotRelevant
    {
        private string choiceValueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ChoiceValue
        {
            get
            {
                return this.choiceValueField;
            }
            set
            {
                this.choiceValueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class BaseChoiceElementContents : BaseElementContents
    {
        private ChoiceInfo choiceInfoField;

        private ImageMap imageMapField;

        private ChoiceDataElementAllowFreetext allowFreetextField;

        private bool allowFreetextFieldSpecified;

        private ChoiceConditionalProperties conditionalPropertiesField;

        /// <remarks/>
        public ChoiceInfo ChoiceInfo
        {
            get
            {
                return this.choiceInfoField;
            }
            set
            {
                this.choiceInfoField = value;
            }
        }

        /// <remarks/>
        public ImageMap ImageMap
        {
            get
            {
                return this.imageMapField;
            }
            set
            {
                this.imageMapField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ChoiceDataElementAllowFreetext AllowFreetext
        {
            get
            {
                return this.allowFreetextField;
            }
            set
            {
                this.allowFreetextField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AllowFreetextSpecified
        {
            get
            {
                return this.allowFreetextFieldSpecified;
            }
            set
            {
                this.allowFreetextFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ConditionalProperties")]
        public ChoiceConditionalProperties ConditionalProperties
        {
            get
            {
                return this.conditionalPropertiesField;
            }
            set
            {
                this.conditionalPropertiesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ConditionTypes
    {
        private object[] conditionTypeField;

        [XmlElement("AndCondition", typeof(AndCondition))]
        [XmlElement("OrCondition", typeof(OrCondition))]
        [XmlElement("NotCondition", typeof(NotCondition))]
        [XmlElement("SectionIf", typeof(SectionIf))]
        [XmlElement("SectionIfNot", typeof(SectionIfNot))]
        [XmlElement("EqualCondition", typeof(EqualCondition))]
        [XmlElement("GreaterThanCondition", typeof(GreaterThanCondition))]
        [XmlElement("LessThanCondition", typeof(LessThanCondition))]
        [XmlElement("NotEqualCondition", typeof(NotEqualCondition))]
        [XmlElement("GreaterThanOrEqualsCondition", typeof(GreaterThanOrEqualsCondition))]
        [XmlElement("LessThanOrEqualsCondition", typeof(LessThanOrEqualsCondition))]
        [XmlElement("ContainsCondition", typeof(ContainsCondition))]
        [XmlElement("HasAnyNChoicesCondition", typeof(HasAnyNChoicesCondition))]
        /// <remarks/>
        public object[] ConditionType
        {
            get
            {
                return this.conditionTypeField;
            }
            set
            {
                this.conditionTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class ComparisonConditionContents
    {
        private string comparisonValueField;

        private string dataElementIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        public string DataElementId
        {
            get
            {
                return this.dataElementIdField;
            }
            set
            {
                this.dataElementIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        public string ComparisonValue
        {
            get
            {
                return this.comparisonValueField;
            }
            set
            {
                this.comparisonValueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class BaseSectionIfConditionContent
    {
        private string dataElementIdField;

        private TextTemplateContent textTemplateContentField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "IDREF")]
        public string DataElementId
        {
            get
            {
                return this.dataElementIdField;
            }
            set
            {
                this.dataElementIdField = value;
            }
        }

        /// <remarks/>
        public TextTemplateContent TextTemplateContent
        {
            get
            {
                return this.textTemplateContentField;
            }
            set
            {
                this.textTemplateContentField = value;
            }
        }
    }
}