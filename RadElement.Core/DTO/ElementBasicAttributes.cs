using Newtonsoft.Json;

namespace RadElement.Core.DTO
{
    public class ElementBasicAttributes
    {
        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ElementId { get; set; }

        /// <summary>
        /// Gets or sets the name of the element.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ElementName { get; set; }
    }
}
