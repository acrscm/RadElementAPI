using Microsoft.Extensions.Configuration;
using RadElement.Core.Infrastructure;
using RadElement.Infrastructure;
using System;
using Xunit;

namespace RadElement.API.IntegrationTests
{
    public class ConfigurationManagerShould
    {
        private IConfiguration configuration;
        private IConfigurationManager configurationManager;

        public ConfigurationManagerShould()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("AppSettings.json");
            configuration = configurationBuilder.Build();
            configurationManager = new ConfigurationManager(configuration);
        }

        [Fact]
        public void VerifyTitle()
        {
            Assert.Equal(configurationManager.Title, configuration["Title"]);
        }

        [Fact]
        public void VerifyVersion()
        {
            Assert.Equal(configurationManager.Version, configuration["Version"]);
        }

        [Fact]
        public void VerifySwaggerRoutePrefix()
        {
            Assert.Equal(configurationManager.SwaggerRoutePrefix, configuration["Environment:SwaggerRoutePrefix"]);
        }

        [Fact]
        public void VerifyRootPath()
        {
            Assert.Equal(configurationManager.RootPath, AppDomain.CurrentDomain.BaseDirectory);
        }

        [Fact]
        public void VerifyAuthorizationConfig()
        {
            var authConfig = configuration.GetSection("AuthorizationConfig").Get<AuthorizationConfig>();
            Assert.Equal("assist.acr.org", authConfig.Audience);
            Assert.Equal("assist.acr.org", authConfig.Issuer);
            Assert.Equal("\\Certificates\\Marval.pfx", authConfig.KeyFilePath);
        }
    }
}
