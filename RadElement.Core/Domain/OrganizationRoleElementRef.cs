using RadElement.Core.DTO;

namespace RadElement.Core.Domain
{
    public class OrganizationRoleElementRef
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
        /// Gets or sets the element identifier.
        /// </summary>
        public int ElementID { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public OrganizationRole Role { get; set; }
    }
}
