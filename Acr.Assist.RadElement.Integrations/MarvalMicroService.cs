using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Acr.Assist.RadElement.Core.Domain;
using Acr.Assist.RadElement.Core.DTO;
using Acr.Assist.RadElement.Core.Infrastructure;
using Acr.Assist.RadElement.Core.Integrations;
using Newtonsoft.Json;
using Serilog;

namespace Acr.Assist.RadElement.Integrations
{
    public class MarvalMicroService : IMarvalMicroService
    {
        private readonly IConfigurationManager configurationManager;
        private readonly ILogger logger;

        public MarvalMicroService(IConfigurationManager configurationManager, ILogger logger)
        {
            this.configurationManager = configurationManager;
            this.logger = logger;
        }

        public async Task<string> GetModule(UserModule userModule)
        {
            string xmlContent = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = string.Format("{0}/{1}/modules/{2}/{3}", configurationManager.MarvalMicroServiceUrl, 
                                                                          userModule.UserId, userModule.ModuleId, userModule.RoleId);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configurationManager.AccessToken);

                    using (HttpResponseMessage res = await client.GetAsync(url))
                    {
                        if (res.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            using (HttpContent content = res.Content)
                            {
                                string data = await content.ReadAsStringAsync();
                                if (data != null)
                                {
                                    var module = JsonConvert.DeserializeObject<AssistModule>(data);
                                    xmlContent = module.XMLContent;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetModule:Failed");
            }

            return xmlContent;
        }
    }
}
