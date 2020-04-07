using System.ComponentModel.DataAnnotations;

namespace RadElement.Core.DTO
{
    public class SearchKeyword
    {
        /// <summary>
        /// Gets or sets the keyword.
        /// </summary>
        [Required]
        [MinLength(3, ErrorMessage = "The Keyword field must be a string with a minimum length of '3'.")]
        public string Keyword { get; set; }
    }
}
