namespace RadElement.Core.DTO
{
    public class CreateUpdateImage
    {
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

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the rights.
        /// </summary>
        public string Rights { get; set; }
    }
}
