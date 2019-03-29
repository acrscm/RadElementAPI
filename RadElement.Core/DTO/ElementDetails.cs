using RadElement.Core.Domain;
using System.Collections.Generic;

namespace RadElement.Core.DTO
{
    public class ElementDetails: Element
    {
        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        public string ElementId { get; set; }

        /// <summary>
        /// Gets or sets the element values.
        /// </summary>
        public List<ElementValue> ElementValues { get; set; }
    }
}
