using System.ComponentModel;
using System.Text.Json.Serialization;

namespace RadElement.Core.Domain
{
    /// <summary>
    /// Gets or sets the modality type
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ElementSetStatus
    {
        /// <summary>
        /// Element Status is Draft
        /// </summary>
        Draft,

        /// <summary>
        /// Element Status is Proposed
        /// </summary>
        Proposed,

        /// <summary>
        /// Element Status is Trial Use
        /// </summary>
        [Description("Trial Use")]
        TrialUse,

        /// <summary>
        /// Element Status is Active
        /// </summary>
        Active,

        /// <summary>
        /// Element Status is Retired
        /// </summary>
        Retired
    }
}