namespace RadElement.Core.DTO
{
    public class DeepSearchResponse
    {
        /// <summary>
        /// Gets or sets the execution time.
        /// </summary>
        public string DBExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the loop execution time.
        /// </summary>
        public string LoopExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the elements.
        /// </summary>
        public object Elements { get; set; }
    }
}
