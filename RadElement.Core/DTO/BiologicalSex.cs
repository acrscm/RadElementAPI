using System.Text.Json.Serialization;

namespace RadElement.Core.DTO
{
    /// <summary>
    /// Gets or sets the modality type
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BiologicalSex
    {
        /// <summary>
        /// Sex is Male
        /// </summary>
        M,

        /// <summary>
        /// Sex is FeMale
        /// </summary>
        F
    }
}
