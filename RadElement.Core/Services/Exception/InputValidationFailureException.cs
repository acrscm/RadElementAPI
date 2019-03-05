using System;
using System.Collections.Generic;

namespace RadElement.Core.Services.Exception
{
    /// <summary>
    /// Exception thrown  input validation failures
    /// </summary>
    public class InputValidationFailureException : ApplicationException
    {
        /// <summary>
        /// Gets or sets the list of errors
        /// </summary>
        public Dictionary<string, string> Errors { get; }

        public InputValidationFailureException(Dictionary<string, string> errors) : base()
        {
            Errors = errors;
        }

        public InputValidationFailureException(string message) : base(message)
        {
        }
    }
}