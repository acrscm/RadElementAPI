using System.Text.Json.Serialization;

namespace RadElement.Core.Domain
{
    /// <summary>
    /// Gets or sets the modality type
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ElementStatus
    {
        /// <summary>
        /// Element Status is Proposed
        /// </summary>
        Proposed,

        /// <summary>
        /// Element Status is Retired
        /// </summary>
        Retired,

        /// <summary>
        /// Element Status is Published
        /// </summary>
        Published
    }
}