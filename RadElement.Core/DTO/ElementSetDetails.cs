namespace RadElement.Core.DTO
{
    public class ElementSetDetails
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the contact.
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the status
        /// </summary>
        public string Status { get; set; }
    }
}
