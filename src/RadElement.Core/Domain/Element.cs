using Newtonsoft.Json;
using System;

namespace RadElement.Core.Domain
{
    public class Element
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the short name.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets the definition.
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// Gets or sets the type of the value.
        /// </summary>
        public string ValueType { get; set; }

        /// <summary>
        /// Gets or sets the size of the value.
        /// </summary>
        public int ValueSize { get; set; }

        /// <summary>
        /// Gets or sets the value minimum.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float? ValueMin { get; set; }

        /// <summary>
        /// Gets or sets the value maximum.
        /// </summary>
        /// <value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float? ValueMax { get; set; }

        /// <summary>
        /// Gets or sets the step value.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float? StepValue { get; set; }

        /// <summary>
        /// Gets or sets the minimum cardinality.
        /// </summary>
        public uint MinCardinality { get; set; }

        /// <summary>
        /// Gets or sets the maximum cardinality.
        /// </summary>
        public uint MaxCardinality { get; set; }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets the instructions.
        /// </summary>
        public string Instructions { get; set; }

        /// <summary>
        /// Gets or sets the references.
        /// </summary>
        public string References { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the version date.
        /// </summary>
        public DateTime VersionDate { get; set; }

        /// <summary>
        /// Gets or sets the synonyms.
        /// </summary>
        public string Synonyms { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the status date.
        /// </summary>
        public DateTime StatusDate { get; set; }

        /// <summary>
        /// Gets or sets the editor.
        /// </summary>
        public string Editor { get; set; }

        /// <summary>
        /// Gets or sets the modality.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Modality { get; set; }

        /// <summary>
        /// Gets or sets the Biological sex.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BiologicalSex { get; set; }

        /// <summary>
        /// Gets or sets the age upper bound.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float? AgeUpperBound { get; set; }

        /// <summary>
        /// Gets or sets the age lower bound.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float? AgeLowerBound { get; set; }
    }
}
