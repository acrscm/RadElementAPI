using System.Collections.Generic;

namespace RadElement.Core.Domain
{
    /// <summary>
    /// Base class for all data elements
    /// </summary>
    public class DataElement
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets of sets the type
        /// </summary>
        public DataElementType DataElementType { get; protected set; }

        /// <summary>
        /// Gets or sets the label of the data element
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets if the element is required
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets the hint text
        /// </summary>
        public string HintText { get; set; }

        /// <summary>
        /// Represents the unit
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the diagram collection
        /// </summary>
        public List<Diagram> Diagrams { get; set; }

        /// <summary>
        /// Gets of sets the display sequnce
        /// </summary>
        public int DisplaySequence { get; set; }

        /// <summary>
        /// Gets or sets the values 
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the voice command.
        /// </summary>
        public string VoiceCommand { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DataElement"/> is editable.
        /// </summary>
        public bool Editable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DataElement"/> is hasprefilled.
        /// </summary>
        public bool Hasprefilled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DataElement"/> is output.
        /// </summary>
        public bool Output { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        public Source Source { get; set; }

        /// <summary>
        /// Gets or sets the codable concept.
        /// </summary>
        public CodableConcept CodableConcept { get; set; }

        public DataElement()
        {
            CodableConcept = new CodableConcept();
        }
    }
}
