using Newtonsoft.Json;
using System;

namespace RadElement.Core.Domain
{
    public class IndexCode
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the system.
        /// </summary>
        public string System { get; set; }

        /// <summary>
        /// Gets or sets the display.
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// Gets or sets the accession date.
        /// </summary>
        public DateTime AccessionDate { get; set; }

        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Href { get; set; }
    }
}
