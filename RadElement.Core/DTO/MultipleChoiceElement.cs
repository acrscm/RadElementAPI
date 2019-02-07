using RadElement.Core.Domain;

namespace RadElement.Core.DTO
{
    /// <summary>
    /// Represents a multiple choice data element
    /// </summary>
    public class MultipleChoiceElement : ChoiceElement
    {
        public MultipleChoiceElement()
        {
            DataElementType = DataElementType.MultiChoice;
        }
    }
}
