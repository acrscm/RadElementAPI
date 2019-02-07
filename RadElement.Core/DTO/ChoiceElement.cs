using RadElement.Core.Domain;
using System.Collections.Generic;

namespace RadElement.Core.DTO
{
    /// <summary>
    /// Represents the single choice element
    /// </summary>
    public class ChoiceElement : DataElement
    {
        public ChoiceElement()
        {
            DataElementType = DataElementType.Choice;
        }

        /// <summary>
        /// Gets or sets the list of choices
        /// </summary>
        public List<Option> Options { get; set; }     

        /// <summary>
        /// Gets or sets the name of the image map diagram
        /// </summary>
        public Diagram ImageMapLocation { get; set; }

        /// <summary>
        /// Gets or sets the allow free text.
        /// </summary>
        /// <value>
        public bool? AllowFreeText { get; set; }
    }
}
