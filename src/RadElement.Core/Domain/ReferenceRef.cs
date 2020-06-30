namespace RadElement.Core.Domain
{
    public class ReferenceRef
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the reference identifier.
        /// </summary>
        public int Reference_Id { get; set; }

        /// <summary>
        /// Gets or sets the reference for identifier.
        /// </summary>
        public int Reference_For_Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the reference for.
        /// </summary>
        public string Reference_For_Type { get; set; }
    }
}
