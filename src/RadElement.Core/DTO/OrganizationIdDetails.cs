using Newtonsoft.Json;

namespace RadElement.Core.DTO
{
    public class OrganizationIdDetails
    {
        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        public string OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}