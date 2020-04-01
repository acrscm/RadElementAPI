using RadElement.Core.DTO;
using System;

namespace RadElement.Core.Domain
{
    public class ElementSet
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the contact.
        /// </summary>
        public string ContactName { get; set; }
        
        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the modality.
        /// </summary>
        public string Modality { get; set; }

        /// <summary>
        /// Gets or sets the Biological sex.
        /// </summary>
        public string BiologicalSex { get; set; }

        /// <summary>
        /// Gets or sets the age upper bound.
        /// </summary>
        public float? AgeUpperBound { get; set; }

        /// <summary>
        /// Gets or sets the age lower bound.
        /// </summary>
        public float? AgeLowerBound { get; set; }

        /// <summary>
        /// Gets or sets the status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the status date
        /// </summary>
        public DateTime? StatusDate { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }
    }
}
