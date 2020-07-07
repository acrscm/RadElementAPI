namespace RadElement.Core.Domain
{
    public class ImageRef
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        public int ElementId { get; set; }

        /// <summary>
        /// Gets or sets the element value.
        /// </summary>
        public string ElementValue { get; set; }

        /// <summary>
        /// Gets or sets the image identifier.
        /// </summary>
        public int Image_Id { get; set; }

        /// <summary>
        /// Gets or sets the image for identifier.
        /// </summary>
        public int Image_For_Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the image for.
        /// </summary>
        public string Image_For_Type { get; set; }
    }
}
