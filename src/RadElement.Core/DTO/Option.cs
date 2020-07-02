using System.Collections.Generic;

namespace RadElement.Core.DTO
{
    /// <summary>
    /// Represents the option
    /// </summary>
    public class Option
    {
        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the definition.
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// Gets or sets the images.
        /// </summary>
        public string Images { get; set; }

        /// <summary>
        /// Gets or sets the index code references.
        /// </summary>
        public List<int> IndexCodeReferences { get; set; }

        /// <summary>
        /// Gets or sets the references reference.
        /// </summary>
        public List<int> ReferencesRef { get; set; }

        /// <summary>
        /// Gets or sets the images reference.
        /// </summary>
        public List<int> ImagesRef { get; set; }
    }
}
