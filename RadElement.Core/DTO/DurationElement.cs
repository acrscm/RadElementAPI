using RadElement.Core.Domain;

namespace RadElement.Core.DTO
{
    /// <summary>
    /// Represents a duartion element
    /// </summary>
    public class DurationElement : DataElement
    {
        public DurationElement()
        {
            DataElementType = DataElementType.Duration;
        }

        /// <summary>
        /// Gets or sets the minimum day
        /// </summary>
        public string MinimumDay { get; set; }

        /// <summary>
        /// Gets or sets the maximum day
        /// </summary>
        public string MaximumDay { get; set; }

        /// <summary>
        /// Gets or sets the minimum hours
        /// </summary>
        public string MinimumHours { get; set; }

        /// <summary>
        /// Gets or sets the maximum hours
        /// </summary>
        public string MaximumHours { get; set; }

        /// <summary>
        /// Gets or sets the minimum minutes
        /// </summary>
        public string MinimumMinutes { get; set; }

        /// <summary>
        /// Gets or sets the maximum minutes
        /// </summary>
        public string MaximumMinutes { get; set; }

        /// <summary>
        /// Gets or sets the minimum seconds
        /// </summary>
        public string MinimumSeconds { get; set; }

        /// <summary>
        /// Gets or sets the maximum seconds
        /// </summary>
        public string MaximumSeconds { get; set; }

        /// <summary>
        /// Gets or sets the show days
        /// </summary>
        public bool ShowDays { get; set; }

        /// <summary>
        /// Gets or sets the show hours
        /// </summary>
        public bool ShowHours { get; set; }

        /// <summary>
        /// Gets or sets the show minutes
        /// </summary>
        public bool ShowMinutes { get; set; }

        /// <summary>
        /// Gets or sets the show seconds
        /// </summary>
        public bool ShowSeconds { get; set; }
    }
}
