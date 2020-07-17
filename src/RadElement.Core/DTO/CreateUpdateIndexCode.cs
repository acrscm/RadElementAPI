using System;
using System.ComponentModel.DataAnnotations;

namespace RadElement.Core.DTO
{
    public class CreateUpdateIndexCode
    {
        /// <summary>
        /// Gets or sets the system.
        /// </summary>
        [Required]
        [MaxLength(24)]
        public string System { get; set; }

        /// <summary>
        /// Gets or sets the display.
        /// </summary>
        [Required]
        public string Display { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        [MaxLength(24)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        [MaxLength(2083)]
        public string Href { get; set; }
    }
}
