using Newtonsoft.Json;

namespace RadElement.Core.Domain
{
    public class IndexCodeElementRef
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the code identifier.
        /// </summary>
        public int CodeId { get; set; }

        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        public uint ElementId { get; set; }

        /// <summary>
        /// Gets or sets the value code.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ValueCode { get; set; }
    }
}
