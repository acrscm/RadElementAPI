using Acr.Assist.RadElement.Core.Domain;

namespace Acr.Assist.RadElement.Core.DTO
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