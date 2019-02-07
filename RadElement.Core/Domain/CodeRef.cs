namespace Acr.Assist.RadElement.Core.Domain
{
    public class CodeRef
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the code identifier.
        /// </summary>
        /// <value>
        /// The code identifier.
        /// </value>
        public int CodeId { get; set; }

        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        public uint ElementId { get; set; }

        /// <summary>
        /// Gets or sets the value code.
        /// </summary>
        public string ValueCode { get; set; }
    }
}
