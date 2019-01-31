using System.Collections.Generic;

namespace Acr.Assist.RadElement.Core.Domain
{
    /// <summary>
    /// Represents an Assist Module
    /// </summary>
    public class AssistModule
    {
        /// <summary>
        /// Gets or sets the module id
        /// </summary>
        public string ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the module name
        /// </summary>

        public string ModuleName { get; set; }

        /// <summary>
        /// Gets or sets the version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the XML Content
        /// </summary>

        public string XMLContent { get; set; }

        /// <summary>
        /// Gets or sets the metadata
        /// </summary>
        public Metadata MetaData { get; set; }

        /// <summary>
        /// Represents the data elements
        /// </summary>
        public List<DataElement> DataElements { get; set; }
    }
}