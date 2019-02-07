namespace Acr.Assist.RadElement.Core.Domain
{
    /// <summary>
    /// Contains the details of the module
    /// </summary>
    public class ModuleDetails
    {
        /// <summary>
        /// Gets or sets the serialized version of the module
        /// </summary>
        public ReportingModule Module { get; set; }

        /// <summary>
        /// Gets or sets the XML Contnet
        /// </summary>
        public string XMLContent { get; set; }
    }
}
