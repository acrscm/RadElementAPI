using RadElement.Core.Domain;

namespace RadElement.Core.DTO
{
    /// <summary>
    /// Represents a global value
    /// </summary>
    public class GlobalValue : DataElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalValue"/> class.
        /// </summary>
        public GlobalValue()
        {
            DataElementType = DataElementType.Global;
        }
    }
}