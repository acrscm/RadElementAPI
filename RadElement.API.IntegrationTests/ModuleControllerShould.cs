using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RadElement.Infrastructure;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using Xunit;

namespace RadElement.API.IntegrationTests
{
    public class ModuleControllerShould
    {
        private readonly string accessToken;
        private bool disposedValue = false;
        private readonly TestClientProvider testClientProvider;
        private HttpClient client;

        public ModuleControllerShould()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("AppSettings.json");
            var configuration = configurationBuilder.Build();
            var configurationManager = new ConfigurationManager(configuration);
            accessToken = configuration["AccessToken"];
            testClientProvider = new TestClientProvider();
            client = testClientProvider.Client;
        }

        #region CreateModule

        [Theory]
        [InlineData("//XMLFiles//Invalid.xml")]
        public async void Return401InCreateModuleIfAccessTokenIsNotProvided(string xmlPath)
        {
            string apiURL = $"radelement/api/v1/module";

            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + xmlPath);
            XmlElement data = doc.DocumentElement;

            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var param = JsonConvert.SerializeObject(data);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(testURL, contentPost);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("//XMLFiles//Valid.xml")]
        public async void ReturnsSetIdInCreateIfAccessTokenIsValid(string xmlPath)
        {
            string apiURL = $"radelement/api/v1/module";

            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + xmlPath);
            XmlElement data = doc.DocumentElement;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var param = JsonConvert.SerializeObject(data);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/xml");

            var response = await client.PostAsync(testURL, contentPost);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var setId = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

            // Delete temporarily created module
            string deleteApiUrl = $"radelement/api/v1/set/{setId}";
            testURL = testClientProvider.ReturnTestURL(deleteApiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            response = await client.DeleteAsync(testURL);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region UpdateModule

        //[Theory]
        //[InlineData(61, "//XMLFiles//Invalid.xml")]
        //public async void Return401InUpdateModuleIfAccessTokenIsNotProvided(int setId, string xmlPath)
        //{
        //    string apiURL = $"radelement/api/v1/module";

        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(AppDomain.CurrentDomain.BaseDirectory + xmlPath);
        //    XmlElement data = doc.DocumentElement;

        //    var testURL = testClientProvider.ReturnTestURL(apiURL);
        //    var param = JsonConvert.SerializeObject(data);
        //    HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

        //    var response = await client.PutAsync(testURL, contentPost);
        //    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        //}

        //[Theory]
        //[InlineData(61, "//XMLFiles//Valid.xml")]
        //public async void ReturnsSetIdInUpdateIfAccessTokenIsValid(int setId, string xmlPath)
        //{
        //    string apiURL = $"radelement/api/v1/module";

        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(AppDomain.CurrentDomain.BaseDirectory + xmlPath);
        //    XmlElement data = doc.DocumentElement;

        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        //    var testURL = testClientProvider.ReturnTestURL(apiURL);
        //    var param = JsonConvert.SerializeObject(data);
        //    HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

        //    var response = await client.PutAsync(testURL, contentPost);
        //    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        //    var setId = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

        //    // Delete temporarily created module
        //    string deleteApiUrl = $"radelement/api/v1/set/{setId}";
        //    testURL = testClientProvider.ReturnTestURL(deleteApiUrl);
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //    response = await client.DeleteAsync(testURL);
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    testClientProvider.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ModulesControllerShould() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        internal void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
    }
}
