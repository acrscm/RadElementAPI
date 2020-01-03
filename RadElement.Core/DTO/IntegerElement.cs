using RadElement.Core.Domain;

namespace RadElement.Core.DTO
{
    /// <summary>
    /// Represents a Integer elemeny
    /// </summary>
    public class IntegerElement : DataElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerElement"/> class.
        /// </summary>
        public IntegerElement()
        {
            DataElementType = DataElementType.Integer;
        }

        /// <summary>
        /// Represents the minimum value
        /// </summary>
        public int? MinimumValue { get; set; }

        /// <summary>
        /// Represents the maximum value
        /// </summary>
        public int? MaximumValue { get; set; }
    }
}
