using Newtonsoft.Json;
using RadElement.Core.Domain;
using System.Collections.Generic;

namespace RadElement.Core.DTO
{
    public class ElementDetails: Element
    {
        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        public string ElementId { get; set; }

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

        /// <summary>
        /// Gets or sets the element values.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ElementValue> ElementValues { get; set; }
    }
}
