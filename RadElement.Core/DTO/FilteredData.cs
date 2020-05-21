using RadElement.Core.Domain;

namespace RadElement.Core.DTO
{
    public class FilteredData
    {
        /// <summary>
        /// Gets or sets the element.
        /// </summary>
        public Element Element { get; set; }

        /// <summary>
        /// Gets or sets the element set.
        /// </summary>
        public ElementSet ElementSet { get; set; }

        /// <summary>
        /// Gets or sets the element value.
        /// </summary>
        public ElementValue ElementValue { get; set; }

        /// <summary>
        /// Gets or sets the person.
        /// </summary>
        public Person Person { get; set; }

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// Gets or sets the person role.
        /// </summary>
        public string PersonRole { get; set; }

        /// <summary>
        /// Gets or sets the organization roles.
        /// </summary>
        public string OrganizationRole { get; set; }

        /// <summary>
        /// Gets or sets the index code.
        /// </summary>
        public IndexCode IndexCode { get; set; }

        /// <summary>
        /// Gets or sets the element value index code.
        /// </summary>
        public IndexCode ElementValueIndexCode { get; set; }
    }
}
