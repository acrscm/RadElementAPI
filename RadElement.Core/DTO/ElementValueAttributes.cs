using Newtonsoft.Json;
using RadElement.Core.Domain;
using System.Collections.Generic;

namespace RadElement.Core.DTO
{
    public class ElementValueAttributes : ElementValue
    {
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
    }
}
