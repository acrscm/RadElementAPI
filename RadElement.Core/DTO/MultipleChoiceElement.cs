using RadElement.Core.Domain;

namespace RadElement.Core.DTO
{
    /// <summary>
    /// Represents a multiple choice data element
    /// </summary>
    public class MultipleChoiceElement : ChoiceElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleChoiceElement"/> class.
        /// </summary>
        public MultipleChoiceElement()
        {
            DataElementType = DataElementType.MultiChoice;
        }
    }
}
