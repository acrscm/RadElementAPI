using Newtonsoft.Json;

namespace RadElement.Core.DTO
{
    public class ReferenceIdDetails
    {
        /// <summary>
        /// Gets or sets the reference identifier.
        /// </summary>
        public string ReferenceId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}
