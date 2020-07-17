using System.ComponentModel.DataAnnotations;

namespace RadElement.Core.DTO
{
    public class CreateUpdateReference
    {
        /// <summary>
        /// Gets or sets the citation.
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string Citation { get; set; }

        /// <summary>
        /// Gets or sets the doi URI.
        /// </summary>
        [MaxLength(255)]
        public string DoiUri { get; set; }

        /// <summary>
        /// Gets or sets the pubmed identifier.
        /// </summary>
        public int? PubmedId { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        [MaxLength(255)]
        public string Url { get; set; }
    }
}
