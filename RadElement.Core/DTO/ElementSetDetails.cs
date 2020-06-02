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
        /// Gets or sets the index codes.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<IndexCode> IndexCodes { get; set; }

        /// <summary>
        /// Gets or sets the references.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Reference> References { get; set; }

        /// <summary>
        /// Gets or sets the person information.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<PersonAttributes> PersonInformation { get; set; }

        /// <summary>
        /// Gets or sets the organization information.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<OrganizationAttributes> OrganizationInformation { get; set; }
    }
}
