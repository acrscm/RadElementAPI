using RadElement.Core.Domain;
namespace RadElement.Core.DTO
{
    public class FilteredElements
    {
        /// <summary>
        /// Gets or sets the element.
        /// </summary>
        public Element Element { get; set; }

        /// <summary>
        /// Gets or sets the element set.
        /// </summary>
        public ElementSet ElementSet { get; set; }

        /// <summary>
        /// Gets or sets the element value.
        /// </summary>
        public ElementValue ElementValues { get; set; }
    }
}
