using Newtonsoft.Json;

namespace RadElement.Core.DTO
{
    public class PersonIdDetails
    {
        /// <summary>
        /// Gets or sets the person identifier.
        /// </summary>
        public string PersonId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}
