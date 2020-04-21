using System.Collections.Generic;

namespace RadElement.Core.DTO
{
    public class PersonDetails
    {
        /// <summary>
        /// Gets or sets the person identifier.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public List<PersonRole> Roles { get; set; }
    }
}
