using System.Collections.Generic;

namespace RadElement.API.Models
{
    /// <summary>
    /// Contains the details of the error
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Initialzes the instance of the class
        /// </summary>
        /// <param name="message">Contains the error message</param>
        /// <param name="validationErrors">Contains the list of error message</param>
        public ErrorResponse(string message, Dictionary<string, string> validationErrors = null)
        {
            Message = message;
            ValidationErrors = validationErrors;
        }

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets or sets the validation error
        /// </summary>
        public Dictionary<string, string> ValidationErrors { get; }
    }
}
