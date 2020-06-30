using System.Net;

namespace RadElement.Core.Domain
{
    public class JsonResult
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public HttpStatusCode Code { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Domain.JsonResult" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="code">The code.</param>
        public JsonResult(object value, HttpStatusCode code)
        {
            this.Value = value;
            this.Code = code;
        }
    }
}
