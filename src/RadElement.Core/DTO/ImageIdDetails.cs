using Newtonsoft.Json;

namespace RadElement.Core.DTO
{
    public class ImageIdDetails
    {
        /// <summary>
        /// Gets or sets the image identifier.
        /// </summary>
        public string ImageId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}
