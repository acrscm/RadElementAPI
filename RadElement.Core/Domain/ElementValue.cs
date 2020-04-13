using Newtonsoft.Json;

namespace RadElement.Core.Domain
{
    public class ElementValue
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        public uint ElementId { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the definition.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Definition { get; set; }

        /// <summary>
        /// Gets or sets the images.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Images { get; set; }
    }
}
