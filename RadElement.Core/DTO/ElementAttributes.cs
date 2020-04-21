using RadElement.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RadElement.Core.DTO
{
    public class ElementAttributes
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the short name.
        /// </summary>
        [MaxLength(24)]
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets the definition.
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// Gets or sets the type of the value.
        /// </summary>
        [Required]
        public DataElementType? ValueType { get; set; }

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
        /// Gets or sets the unit.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets the instructions.
        /// </summary>
        public string Instructions { get; set; }

        /// <summary>
        /// Gets or sets the references.
        /// </summary>
        public string References { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        [MaxLength(8)]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the version date.
        /// </summary>
        public DateTime? VersionDate { get; set; }

        /// <summary>
        /// Gets or sets the synonyms.
        /// </summary>
        public string Synonyms { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the editor.
        /// </summary>
        /// <value>
        /// The editor.
        /// </value>
        [MaxLength(12)]
        public string Editor { get; set; }

        /// <summary>
        /// Gets or sets the modality.
        /// </summary>
        public List<ModalityType> Modality { get; set; }

        /// <summary>
        /// Gets or sets the Biological sex.
        /// </summary>
        public List<BiologicalSex> BiologicalSex { get; set; }

        /// <summary>
        /// Gets or sets the age upper bound.
        /// </summary>
        public float? AgeUpperBound { get; set; }

        /// <summary>
        /// Gets or sets the age lower bound.
        /// </summary>
        public float? AgeLowerBound { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        public List<Option> Options { get; set; }
    }
}
