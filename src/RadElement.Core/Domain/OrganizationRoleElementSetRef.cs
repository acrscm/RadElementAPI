using RadElement.Core.DTO;

namespace RadElement.Core.Domain
{
    public class OrganizationRoleElementSetRef
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        public int OrganizationID { get; set; }

        /// <summary>
        /// Gets or sets the element set identifier.
        /// </summary>
        public int ElementSetID { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public string Role { get; set; }
    }
}
