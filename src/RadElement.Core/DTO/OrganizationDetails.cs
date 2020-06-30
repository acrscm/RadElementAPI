using System.Collections.Generic;

namespace RadElement.Core.DTO
{
    public class OrganizationDetails
    {
        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public List<OrganizationRole> Roles { get; set; }
    }
}
