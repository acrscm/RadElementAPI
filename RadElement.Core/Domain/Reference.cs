using Newtonsoft.Json;

namespace RadElement.Core.Domain
{
    public class Reference
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the citation.
        /// </summary>n.
        /// </value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Citation { get; set; }

        /// <summary>
        /// Gets or sets the doi URI.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]    
        public string Doi_Uri { get; set; }

        /// <summary>
        /// Gets or sets the pubmed identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Pubmed_Id { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }
}
