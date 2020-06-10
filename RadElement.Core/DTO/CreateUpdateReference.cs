namespace RadElement.Core.DTO
{
    public class CreateUpdateReference
    {
        /// <summary>
        /// Gets or sets the citation.
        /// </summary>
        public string Citation { get; set; }

        /// <summary>
        /// Gets or sets the doi URI.
        /// </summary>
        public string DoiUri { get; set; }

        /// <summary>
        /// Gets or sets the pubmed identifier.
        /// </summary>
        public int? PubmedId { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        public string Url { get; set; }
    }
}
