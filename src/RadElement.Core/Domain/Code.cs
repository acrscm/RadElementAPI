using System;

namespace RadElement.Core.Domain
{
    public class Code
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the code1.
        /// </summary>
        public string Code1 { get; set; }

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
    }
}
