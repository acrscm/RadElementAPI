using RadElement.Core.Domain;
using System.Collections.Generic;

namespace RadElement.Core.DTO
{
    public class PersonAttributes: Person
    {
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonAttributes"/> class.
        /// </summary>
        public PersonAttributes()
        {
            Roles = new List<string>();
        }
    }
}
