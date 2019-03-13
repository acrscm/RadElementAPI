using System;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace RadElement.API.IntegrationTests
{
    public class TestClientProvider : IDisposable
    {
        private TestServer server;
        public HttpClient Client { get; private set; }


        private const string serverURL = "https://localhost/";

        public TestClientProvider()
        {
            var projectDir = System.IO.Directory.GetCurrentDirectory();
            var options = new RewriteOptions().AddRedirectToHttpsPermanent();
            var webHostBuilder = WebHost.CreateDefaultBuilder()
               .UseStartup<Startup>()
               .UseContentRoot(projectDir)
               .UseEnvironment("Development")
               .ConfigureAppConfiguration((context, builder) =>
               {
                   var env = context.HostingEnvironment;

                   builder.AddJsonFile("appsettings.json",
                                optional: true, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                                optional: true, reloadOnChange: true);
                   builder.SetBasePath(projectDir);
                   builder.AddEnvironmentVariables();
               })
               .UseSerilog();

            server = new TestServer(webHostBuilder);
            server.BaseAddress = new Uri(serverURL);
            Client = server.CreateClient();
        }

        public String ReturnTestURL(string apiURL)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(serverURL);
            urlBuilder.Append(apiURL);
            return urlBuilder.ToString();

        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                    server.Dispose();
                    server = null;

                    Client.Dispose();
                    Client = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
