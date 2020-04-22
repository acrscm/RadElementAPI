using Newtonsoft.Json;
using RadElement.Core.Domain;
using System.Collections.Generic;

namespace RadElement.Core.DTO
{
    public class ElementSetDetails: ElementSet
    {
        /// <summary>
        /// Gets or sets the set identifier.
        /// </summary>
        public string SetId { get; set; }

        /// <summary>
        /// Gets or sets the person information.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Person> PersonInformation { get; set; }

        /// <summary>
        /// Gets or sets the organization information.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Organization> OrganizationInformation { get; set; }
    }
}
