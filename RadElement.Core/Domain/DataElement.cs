namespace RadElement.Core.Domain
{
    /// <summary>
    /// Base class for all data elements
    /// </summary>
    public class DataElement
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets of sets the type
        /// </summary>
        public DataElementType DataElementType { get; protected set; }
    }
}
