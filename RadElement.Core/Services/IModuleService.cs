using RadElement.Core.DTO;
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
        Task<SetIdDetails> CreateModule(XmlElement xmlContent);

        /// <summary>
        /// Updates the module.
        /// </summary>
        /// <param name="xmlContent">Content of the XML.</param>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        Task<bool> UpdateModule(XmlElement xmlContent, int setId);
    }
}
