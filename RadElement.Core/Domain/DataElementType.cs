using System.Text.Json.Serialization;

namespace RadElement.Core.Domain
{
    /// <summary>
    /// Gets or sets the Data element type
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DataElementType
    {
        /// <summary>
        /// Data Element Type is Integer
        /// </summary>
        Integer,

        /// <summary>
        /// Data Element Type is Numeric
        /// </summary>
        Numeric,

        /// <summary>
        /// Data Element Type is Choice
        /// </summary>
        Choice,

        /// <summary>
        /// Data Element Type is MultiChoice
        /// </summary>
        MultiChoice,

        /// <summary>
        /// Data Element Type is DateTime
        /// </summary>
        DateTime,
        
        /// <summary>
        /// Data Element Type is String
        /// </summary>
        String
    }
}