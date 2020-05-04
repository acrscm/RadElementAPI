using RadElement.Core.Domain;
using System.Collections.Generic;

namespace RadElement.Core.DTO
{
    public class OrganizationAttributes : Organization
    {
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationAttributes"/> class.
        /// </summary>
        public OrganizationAttributes()
        {
            Roles = new List<string>();
        }
    }
}
