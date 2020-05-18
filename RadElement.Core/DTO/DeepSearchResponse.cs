using System.Collections.Generic;

namespace RadElement.Core.DTO
{
    public class DeepSearchResponse
    {
        /// <summary>
        /// Gets or sets the execution time.
        /// </summary>
        public string ElementExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the element values execution time.
        /// </summary>
        public string ElementValuesExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the set execution time.
        /// </summary>
        public string SetExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the organization execution time.
        /// </summary>
        public string OrganizationExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the person execution time.
        /// </summary>
        public string PersonExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the elements.
        /// </summary>
        public List<ElementDetails> Elements { get; set; }
    }
}
