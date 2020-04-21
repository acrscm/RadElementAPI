using Newtonsoft.Json;
using RadElement.Core.Domain;
using System.Collections.Generic;

namespace RadElement.Core.DTO
{
    public class OrganizationDetails : Organization
    {
        /// <summary>
        /// Gets or sets the set identifier.
        /// </summary>
        /// <value>
        /// The set information.
        /// </value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<SetBasicAttributes> SetInformation { get; set; }

        /// <summary>
        /// Gets or sets the element information.
        /// </summary>
        /// <value>
        /// The element information.
        /// </value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ElementBasicAttributes> ElementInformation { get; set; }
    }
}
