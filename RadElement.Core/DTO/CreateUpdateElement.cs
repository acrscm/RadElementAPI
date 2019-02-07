using RadElement.Core.Domain;
using System.Collections.Generic;

namespace RadElement.Core.DTO
{
    public class CreateUpdateElement
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Label { get; set; }
        
        /// <summary>
        /// Gets or sets the definition.
        /// </summary>
        public string Definition { get; set; }        

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        public string Question { get; set; }
        
        /// <summary>
        /// Gets or sets the value minimum.
        /// </summary>
        public float? ValueMin { get; set; }

        /// <summary>
        /// Gets or sets the value maximum.
        /// </summary>
        /// <value>
        public float? ValueMax { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public List<Option> Options { get; set; }
    }
}
