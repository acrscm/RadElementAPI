using System.Net;
using Xunit;

namespace RadElement.API.IntegrationTests
{
    public class HelloWorldControllerShould
    {
        [Fact]
        public async void ShouldReturnHelloWorldWhenCalled()
        {
            using (var testClientProvider = new TestClientProvider())
            {
                var client = testClientProvider.Client;
                var testURL = testClientProvider.ReturnTestURL("api/HelloWorld");
                var response = await client.GetAsync(testURL);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}
