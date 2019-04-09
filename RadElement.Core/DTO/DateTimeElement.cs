using RadElement.Core.Domain;

namespace RadElement.Core.DTO
{
    /// <summary>
    /// Represents a datetime element
    /// </summary>
    public class DateTimeElement : DataElement
    {
        public DateTimeElement()
        {
            DataElementType = DataElementType.DateTime;
        }
    }
}
