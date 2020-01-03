using RadElement.Core.Domain;

namespace RadElement.Core.DTO
{
    /// <summary>
    /// Represents a datetime element
    /// </summary>
    public class DateTimeElement : DataElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeElement"/> class.
        /// </summary>
        public DateTimeElement()
        {
            DataElementType = DataElementType.DateTime;
        }
    }
}
