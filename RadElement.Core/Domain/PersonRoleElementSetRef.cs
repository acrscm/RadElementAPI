using RadElement.Core.DTO;

namespace RadElement.Core.Domain
{
    public class PersonRoleElementSetRef
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the person identifier.
        /// </summary>
        public int PersonID { get; set; }

        /// <summary>
        /// Gets or sets the element set identifier.
        /// </summary>
        public int ElementSetID { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public PersonRole Role { get; set; }
    }
}
