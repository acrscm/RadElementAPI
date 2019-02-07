using RadElement.Core.Domain;

namespace RadElement.Core.DTO
{
    /// <summary>
    /// Represents a numeric element
    /// </summary>
    public class NumericElement : DataElement
    {
        public NumericElement()
        {
            DataElementType = DataElementType.Numeric;
        }

        /// <summary>
        /// Gets or sets the minimum value
        /// </summary>
        public  decimal? MinimumValue { get; set;}

        /// <summary>
        /// Gets or sets the maximum value
        /// </summary>
        public decimal?  MaximumValue { get; set; }     
    }
}
