using Acr.Assist.RadElement.Core.Domain;
using Acr.Assist.RadElement.Core.DTO;
using System.Threading.Tasks;

namespace Acr.Assist.RadElement.Core.Integrations
{
    public interface IMarvalMicroService
    {
        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <param name="userModule">The user module.</param>
        /// <returns></returns>
        Task<string> GetModule(UserModule userModule);
    }
}
