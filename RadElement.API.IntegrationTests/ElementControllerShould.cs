using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Infrastructure;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace RadElement.API.IntegrationTests
{
    public class ElementControllerShould
    {
        private readonly string accessToken;
        private bool disposedValue = false;
        private readonly TestClientProvider testClientProvider;
        private HttpClient client;

        public ElementControllerShould()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("AppSettings.json");
            var configuration = configurationBuilder.Build();
            var configurationManager = new ConfigurationManager(configuration);
            accessToken = configuration["AccessToken"];
            testClientProvider = new TestClientProvider();
            client = testClientProvider.Client;
        }

        #region GetElements

        [Fact]
        public async void Return401InGetElementsIfAccessTokenIsNotProvided()
        {
            string apiURL = $"radelement/api/v1/element";

            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var response = await client.GetAsync(testURL);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void ReturnsElementsInGetElementsIfAccessTokenIsValid()
        {
            string apiURL = $"radelement/api/v1/element";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var response = await client.GetAsync(testURL);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region GetElementByElementId

        [Theory]
        [InlineData(61)]
        [InlineData(62)]
        public async void Return401InGetGetElementByElementIdIfAccessTokenIsNotProvided(int elementId)
        {
            string apiURL = $"radelement/api/v1/element/{elementId}";

            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var response = await client.GetAsync(testURL);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData(61)]
        [InlineData(62)]
        public async void ReturnsElementInGetElementByElementIdIfAccessTokenIsValid(int elementId)
        {
            string apiURL = $"radelement/api/v1/element/{elementId}";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var response = await client.GetAsync(testURL);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region GetElementBySetId

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public async void Return401InGetGetElementsBySetIdIfAccessTokenIsNotProvided(int setId)
        {
            string apiURL = $"radelement/api/v1/set/{setId}/element";

            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var response = await client.GetAsync(testURL);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public async void ReturnsElemenstInGetElementsBySetIdIfAccessTokenIsValid(int setId)
        {
            string apiURL = $"radelement/api/v1/set/{setId}/element";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var response = await client.GetAsync(testURL);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region SearchElement

        [Theory]
        [InlineData("Glasgow")]
        [InlineData("Nodule")]
        public async void Return401InSearchElementIfAccessTokenIsNotProvided(string searchKeyword)
        {
            string apiURL = $"radelement/api/v1/element/search?searchKeyword={searchKeyword}";

            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var response = await client.GetAsync(testURL);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("Glasgow")]
        [InlineData("Nodule")]
        public async void ReturnsSetsInSearchElementIfAccessTokenIsValid(string searchKeyword)
        {
            string apiURL = $"radelement/api/v1/element/search?searchKeyword={searchKeyword}";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var response = await client.GetAsync(testURL);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region CreateElement

        [Theory]
        [InlineData(1, ElementType.Choice)]
        [InlineData(1, ElementType.Numeric)]
        [InlineData(1, ElementType.MultiChoice)]
        [InlineData(1, ElementType.Integer)]
        public async void Return401InCreateElementIfAccessTokenIsNotProvided(int setId, ElementType elementType)
        {
            string apiURL = $"radelement/api/v1/set/{setId}/element/{elementType}";
            var createUpdateEleemnt = new CreateUpdateElement();

            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var param = JsonConvert.SerializeObject(createUpdateEleemnt);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(testURL, contentPost);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData(1, ElementType.Choice)]
        [InlineData(1, ElementType.Numeric)]
        [InlineData(1, ElementType.MultiChoice)]
        [InlineData(1, ElementType.Integer)]
        public async void ReturnsElementIdInCreateElementIfAccessTokenIsValid(int setId, ElementType elementType)
        {
            // Creates temporary element
            string apiURL = $"radelement/api/v1/set/{setId}/element/{elementType}";
            var createUpdateElement = new CreateUpdateElement();
            createUpdateElement.Label = "Tumuor";
            createUpdateElement.Definition = "Tumuor vein";

            if ((DataElementType)elementType == DataElementType.Integer)
            {
                createUpdateElement.ValueMin = 1;
                createUpdateElement.ValueMin = 3;
            }
            else if ((DataElementType)elementType == DataElementType.Numeric)
            {
                createUpdateElement.ValueMin = 1f;
                createUpdateElement.ValueMin = 3f;
            }
            else if ((DataElementType)elementType == DataElementType.Choice || (DataElementType)elementType == DataElementType.MultiChoice)
            {
                createUpdateElement.Options = new List<Core.DTO.Option>();
                createUpdateElement.Options.AddRange(
                    new List<Core.DTO.Option>()
                    {
                        new Core.DTO.Option { Label = "value1", Value = "1" },
                        new Core.DTO.Option { Label = "value2", Value = "2" },
                        new Core.DTO.Option { Label = "value3", Value = "3" }
                    }
                );
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var param = JsonConvert.SerializeObject(createUpdateElement);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(testURL, contentPost);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var elementId = JsonConvert.DeserializeObject<ElementIdDetails>(await response.Content.ReadAsStringAsync()).ElementId;

            // Deletes temporarily created element
            string deleteApiUrl = $"radelement/api/v1/set/{setId}/element/{elementId}";
            testURL = testClientProvider.ReturnTestURL(deleteApiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            response = await client.DeleteAsync(testURL);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region UpdateElement

        [Theory]
        [InlineData(1, 100, ElementType.Choice)]
        public async void Return401InUpdateElementIfAccessTokenIsNotProvided(int setId, int elementId, ElementType elementType)
        {
            string apiURL = $"radelement/api/v1/set/{setId}/element/{elementId}/{elementType}";
            var createUpdateElement = new CreateUpdateElement();

            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var param = JsonConvert.SerializeObject(createUpdateElement);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(testURL, contentPost);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData(1, ElementType.Choice)]
        [InlineData(1, ElementType.Numeric)]
        [InlineData(1, ElementType.MultiChoice)]
        [InlineData(1, ElementType.Integer)]
        public async void ReturnsSuccessInUpdateElementIfAccessTokenIsValid(int setId, ElementType elementType)
        {
            // Creates temporary element
            string apiURL = $"radelement/api/v1/set/{setId}/element/{elementType}";
            var createUpdateElement = new CreateUpdateElement();
            createUpdateElement.Label = "Tumuor";
            createUpdateElement.Definition = "Tumuor vein";

            if ((DataElementType)elementType == DataElementType.Integer)
            {
                createUpdateElement.ValueMin = 1;
                createUpdateElement.ValueMin = 3;
            }
            else if ((DataElementType)elementType == DataElementType.Numeric)
            {
                createUpdateElement.ValueMin = 1f;
                createUpdateElement.ValueMin = 3f;
            }
            else if ((DataElementType)elementType == DataElementType.Choice || (DataElementType)elementType == DataElementType.MultiChoice)
            {
                createUpdateElement.Options = new List<Core.DTO.Option>();
                createUpdateElement.Options.AddRange(
                    new List<Core.DTO.Option>()
                    {
                        new Core.DTO.Option { Label = "value1", Value = "1" },
                        new Core.DTO.Option { Label = "value2", Value = "2" },
                        new Core.DTO.Option { Label = "value3", Value = "3" }
                    }
                );
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var param = JsonConvert.SerializeObject(createUpdateElement);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(testURL, contentPost);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var elementId = JsonConvert.DeserializeObject<ElementIdDetails>(await response.Content.ReadAsStringAsync()).ElementId;

            // Updates the temporarily created element

            createUpdateElement.Label = "Tumuor_Updated";
            createUpdateElement.Definition = "Tumuor_Vein_Updated";

            apiURL = $"radelement/api/v1/set/{setId}/element/{elementId}/{elementType}";
            testURL = testClientProvider.ReturnTestURL(apiURL);
            param = JsonConvert.SerializeObject(createUpdateElement);
            contentPost = new StringContent(param, Encoding.UTF8, "application/json");

            response = await client.PutAsync(testURL, contentPost);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Deletes temporarily created element
            string deleteApiUrl = $"radelement/api/v1/set/{setId}/element/{elementId}";
            testURL = testClientProvider.ReturnTestURL(deleteApiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            response = await client.DeleteAsync(testURL);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region DeleteElement

        [Theory]
        [InlineData(1, 100)]
        public async void Return401InDeleteElementIfAccessTokenIsNotProvided(int setId, int elementId)
        {
            string apiURL = $"radelement/api/v1/set/{setId}/element/{elementId}";

            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var response = await client.DeleteAsync(testURL);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public async void ReturnsSuccessInDeleteElementIfAccessTokenIsValid(int setId)
        {
            // Creates temporary element
            var elementType = ElementType.Integer;
            string apiURL = $"radelement/api/v1/set/{setId}/element/{elementType}";
            var createUpdateElement = new CreateUpdateElement();
            createUpdateElement.Label = "Tumuor";
            createUpdateElement.Definition = "Tumuor vein";
            createUpdateElement.ValueMin = 1;
            createUpdateElement.ValueMin = 3;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var testURL = testClientProvider.ReturnTestURL(apiURL);
            var param = JsonConvert.SerializeObject(createUpdateElement);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(testURL, contentPost);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var elementId = JsonConvert.DeserializeObject<ElementIdDetails>(await response.Content.ReadAsStringAsync()).ElementId;

            // Deletes temporarily created element
            string deleteApiUrl = $"radelement/api/v1/set/{setId}/element/{elementId}";
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