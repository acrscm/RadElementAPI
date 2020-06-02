using Newtonsoft.Json;

namespace RadElement.Core.DTO
{
    public class IndexCodeIdDetails
    {
        /// <summary>
        /// Gets or sets the index code identifier.
        /// </summary>
        /// <value>
        /// The index code identifier.
        /// </value>
        public string IndexCodeId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}
