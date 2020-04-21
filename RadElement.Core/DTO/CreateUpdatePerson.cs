using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RadElement.Core.DTO
{
    public class CreateUpdatePerson
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
        public string Orcid { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        [MaxLength(255)]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the twitter handle.
        /// </summary>
        [MaxLength(255)]
        public string TwitterHandle { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public List<PersonRole> Roles { get; set; }
    }
}
