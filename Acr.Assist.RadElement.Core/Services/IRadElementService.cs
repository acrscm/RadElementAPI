using Acr.Assist.RadElement.Core.DTO;
using System.Threading.Tasks;

namespace Acr.Assist.RadElement.Core.Services
{
    public interface IRadElementService
    {
        /// <summary>
        /// Inserts the Rad element data by module Id
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        Task InsertData(object data);
    }
}
