using System.Collections.Generic;

namespace RadElement.Core.DTO
{
    public class SimpleSearchResponse
    {
        /// <summary>
        /// Gets or sets the execution time.
        /// </summary>
        public string ExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the elements.
        /// </summary>
        public List<BasicElementDetails> Elements { get; set; }
    }
}
