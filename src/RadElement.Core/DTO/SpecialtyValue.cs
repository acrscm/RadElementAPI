using System;
using System.ComponentModel.DataAnnotations;

namespace RadElement.Core.DTO
{
    public class SpecialtyValue
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [MaxLength(2)]
        public string Value { get; set; }
    }
}
