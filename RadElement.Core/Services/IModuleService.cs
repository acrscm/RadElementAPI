using RadElement.Core.Domain;
using System.Threading.Tasks;
using System.Xml;

namespace RadElement.Core.Services
{
    public interface IModuleService
    {
        /// <summary>
        /// Creates the module.
        /// </summary>
        /// <param name="xmlContent">Content of the XML.</param>
        /// <returns></returns>
        Task<JsonResult> CreateModule(XmlElement xmlContent);

        /// <summary>
        /// Updates the module.
        /// </summary>
        /// <param name="xmlContent">Content of the XML.</param>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        Task<JsonResult> UpdateModule(XmlElement xmlContent, int setId);
    }
}
