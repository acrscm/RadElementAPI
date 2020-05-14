using Newtonsoft.Json;
using System.Collections.Generic;

namespace RadElement.Core.DTO
{
    public class BasicElementDetails
    {
        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the set identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<SetBasicAttributes> SetInformation { get; set; }
    }
}
