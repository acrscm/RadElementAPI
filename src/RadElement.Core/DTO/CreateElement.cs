namespace RadElement.Core.DTO
{
    public class CreateElement : ElementAttributes
    {
        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        public string ElementId { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        public int? ParentId { get; set; }
    }
}
