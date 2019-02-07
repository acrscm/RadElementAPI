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
        /// Gets or sets the name.
        /// </summary>
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
        public float? ValueMin { get; set; }

        /// <summary>
        /// Gets or sets the value maximum.
        /// </summary>
        /// <value>
        public float? ValueMax { get; set; }

        /// <summary>
        /// Gets or sets the step value.
        /// </summary>
        public float? StepValue { get; set; }

        /// <summary>
        /// Gets or sets the minimum cardinality.
        /// </summary>
        public short MinCardinality { get; set; }

        /// <summary>
        /// Gets or sets the maximum cardinality.
        /// </summary>
        public short MaxCardinality { get; set; }

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
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the status date.
        /// </summary>
        public DateTime StatusDate { get; set; }

        /// <summary>
        /// Gets or sets the editor.
        /// </summary>
        public string Editor { get; set; }
    }
}
