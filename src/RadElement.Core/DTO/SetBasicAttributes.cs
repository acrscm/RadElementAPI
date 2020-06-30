using Newtonsoft.Json;

namespace RadElement.Core.DTO
{
    public class SetBasicAttributes
    {
        /// <summary>
        /// Gets or sets the set identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SetId { get; set; }

        /// <summary>
        /// Gets or sets the name of the set.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SetName { get; set; }
    }
}
