namespace RadElement.Core.Domain
{
    public class Image
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the local URL.
        /// </summary>
        public string LocalUrl { get; set; }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the source URL.
        /// </summary>
        public string SourceUrl { get; set; }
    }
}
