using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RadElement.Core.DTO
{
    public class CreateUpdateOrganization
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the abbreviation.
        /// </summary>
        [MaxLength(255)]
        public string Abbreviation { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        [MaxLength(255)]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        [MaxLength(255)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the twitter handle.
        /// </summary>
        [MaxLength(255)]
        public string TwitterHandle { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [MaxLength(255)]
        public string Email { get; set; }

    }
}
