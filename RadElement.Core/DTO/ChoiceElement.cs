using RadElement.Core.Domain;
using System.Collections.Generic;

namespace RadElement.Core.DTO
{
    /// <summary>
    /// Represents the single choice element
    /// </summary>
    public class ChoiceElement : DataElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChoiceElement"/> class.
        /// </summary>
        public ChoiceElement()
        {
            DataElementType = DataElementType.Choice;
        }

        /// <summary>
        /// Gets or sets the list of choices
        /// </summary>
        public List<Option> Options { get; set; }
    }
}
