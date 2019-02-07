using RadElement.Core.Domain;

namespace RadElement.Core.DTO
{
    /// <summary>
    /// Represents a global value
    /// </summary>
    public class GlobalValue : DataElement
    {
        public GlobalValue()
        {
            DataElementType = DataElementType.Global;
        }
    }
}