using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RadElement.Core.DTO;
using RadElement.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace RadElement.API.IntegrationTests
{
    public class ElementSetControllerShould
    {
        private readonly string accessToken;
        private bool disposedValue = false;
        private readonly TestClientProvider testClientProvider;
        private HttpClient client;

        public ElementSetControllerShould()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("AppSettings.json");
            var configuration = configurationBuilder.Build();
            var configurationManager = new ConfigurationManager(configuration);
            accessToken = configuration["AccessToken"];
            testClientProvider = new TestClientProvider();
            client = testClientProvider.Client;
        }

        #region GetSets

        [Fact]
        public async void Return401InGetSetsIfAccessTokenIsNotProvided()
        {
            string apiURL = $"radelement/api/v1/set";

            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var response = await client.GetAsync(testURL);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void ReturnsSetsInGetSetsIfAccessTokenIsValid()
        {
            string apiURL = $"radelement/api/v1/set";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var response = await client.GetAsync(testURL);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region GetSetBySetId

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void Return401InGetSetBySetIdIfAccessTokenIsNotProvided(int setId)
        {
            string apiURL = $"radelement/api/v1/set/{setId}";

            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var response = await client.GetAsync(testURL);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void ReturnsSetInGetSetBySetIdIfAccessTokenIsValid(int setId)
        {
            string apiURL = $"radelement/api/v1/set/{setId}";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var response = await client.GetAsync(testURL);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region SearchSet

        [Theory]
        [InlineData("Tra")]
        [InlineData("Computer")]
        public async void Return401InSearchSetIfAccessTokenIsNotProvided(string searchKeyword)
        {
            string apiURL = $"radelement/api/v1/set/search?searchKeyword={searchKeyword}";

            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var response = await client.GetAsync(testURL);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("Tra")]
        [InlineData("Computer")]
        public async void ReturnsSetsInSearchSetIfAccessTokenIsValid(string searchKeyword)
        {
            string apiURL = $"radelement/api/v1/set/search?searchKeyword={searchKeyword}";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var response = await client.GetAsync(testURL);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region CreateSet

        [Theory]
        [InlineData("Hello Assist", "Hello Assist", "ACR Assist")]
        public async void Return401InCreateSetIfAccessTokenIsNotProvided(string moduleName, string description, string contactName)
        {
            string apiURL = $"radelement/api/v1/set";
            var createUpdateSet = new CreateUpdateSet()
            {
                ModuleName = moduleName,
                ContactName = description,
                Description = contactName
            };

            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var param = JsonConvert.SerializeObject(createUpdateSet);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(testURL, contentPost);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("Hello Assist", "Hello Assist", "ACR Assist")]
        public async void ReturnsSetIdInCreateSetIfAccessTokenIsValid(string moduleName, string description, string contactName)
        {
            // Creates temporary set
            string apiURL = $"radelement/api/v1/set";
            var createUpdateSet = new CreateUpdateSet()
            {
                ModuleName = moduleName,
                ContactName = description,
                Description = contactName
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var param = JsonConvert.SerializeObject(createUpdateSet);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(testURL, contentPost);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var setId = JsonConvert.DeserializeObject<SetIdDetails>(await response.Content.ReadAsStringAsync()).SetId;

            // Deletes temporarily created set
            string deleteApiUrl = $"radelement/api/v1/set/{setId}";
            testURL = testClientProvider.ReturnTestURL(deleteApiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            response = await client.DeleteAsync(testURL);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion
        
        #region UpdateSet

        [Theory]
        [InlineData(1, "Hello Assist", "Hello Assist", "ACR Assist")]
        public async void Return401InUpdateSetIfAccessTokenIsNotProvided(int setId, string moduleName, string description, string contactName)
        {
            string apiURL = $"radelement/api/v1/set/{setId}";
            var createUpdateSet = new CreateUpdateSet()
            {
                ModuleName = moduleName,
                ContactName = description,
                Description = contactName
            };

            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var param = JsonConvert.SerializeObject(createUpdateSet);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(testURL, contentPost);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void ReturnsSuccessInUpdateSetIfAccessTokenIsValid()
        {
            // Creates temporary set
            string apiURL = $"radelement/api/v1/set";
            var createUpdateSet = new CreateUpdateSet()
            {
                ModuleName = "Tumuro",
                ContactName = "ACR Assist",
                Description = "Tumuor in vein"
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var param = JsonConvert.SerializeObject(createUpdateSet);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(testURL, contentPost);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var setId = JsonConvert.DeserializeObject<SetIdDetails>(await response.Content.ReadAsStringAsync()).SetId;

            // Updates the temporarily craeted set
            createUpdateSet = new CreateUpdateSet()
            {
                ModuleName = "Tumuro_edit",
                ContactName = "ACR Assist_edit",
                Description = "Tumuor in vein_edit"
            };

            apiURL = $"radelement/api/v1/set/{setId}";
            testURL = testClientProvider.ReturnTestURL(apiURL);
            param = JsonConvert.SerializeObject(createUpdateSet);
            contentPost = new StringContent(param, Encoding.UTF8, "application/json");

            response = await client.PutAsync(testURL, contentPost);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Deletes temporarily created set
            string deleteApiUrl = $"radelement/api/v1/set/{setId}";
            testURL = testClientProvider.ReturnTestURL(deleteApiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            response = await client.DeleteAsync(testURL);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region DeleteSet

        [Theory]
        [InlineData(1)]
        public async void Return401InDeleteSetIfAccessTokenIsNotProvided(int setId)
        {
            string apiURL = $"radelement/api/v1/set/{setId}";

            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var response = await client.DeleteAsync(testURL);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void ReturnsSuccessInDeleteSetIfAccessTokenIsValid()
        {
            // Creates temporary set
            string apiURL = $"radelement/api/v1/set";
            var createUpdateSet = new CreateUpdateSet()
            {
                ModuleName = "Tumuro",
                ContactName = "ACR Assist",
                Description = "Tumuor in vein"
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var param = JsonConvert.SerializeObject(createUpdateSet);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(testURL, contentPost);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var setId = JsonConvert.DeserializeObject<SetIdDetails>(await response.Content.ReadAsStringAsync()).SetId;

            // Deletes temporarily created set
            string deleteApiUrl = $"radelement/api/v1/set/{setId}";
            testURL = testClientProvider.ReturnTestURL(deleteApiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            response = await client.DeleteAsync(testURL);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

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
