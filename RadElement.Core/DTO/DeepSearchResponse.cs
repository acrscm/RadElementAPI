using System.Collections.Generic;

namespace RadElement.Core.DTO
{
    public class DeepSearchResponse
    {
        /// <summary>
        /// Gets or sets the execution time.
        /// </summary>
        /// <value>
        /// The execution time.
        /// </value>
        public string ExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the elements.
        /// </summary>
        public List<ElementDetails> Elements { get; set; }
    }
}
