using System.ComponentModel.DataAnnotations;

namespace RadElement.Core.DTO
{
    public class SearchKeyword
    {
        /// <summary>
        /// Gets or sets the keyword.
        /// </summary>
        [Required]
        [MinLength(1, ErrorMessage = "The Keyword field must be a string with a minimum length of '1'.")]
        public string Keyword { get; set; }
    }
}
