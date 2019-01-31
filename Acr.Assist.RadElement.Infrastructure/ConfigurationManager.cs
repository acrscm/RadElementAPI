
using Acr.Assist.RadElement.Core.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;

namespace Acr.Assist.RadElement.Infrastructure
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IConfiguration configuration;

        public ConfigurationManager(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string ConnectionString  => configuration.GetConnectionString("Database");

        public string Title => configuration["Title"];

        public string Version => configuration["Version"];

        public string ApplicationURL => configuration["Environment:ApplicationURL"];

        public string SwaggerRoutePrefix => configuration["Environment:SwaggerRoutePrefix"];

        public string RootPath => AppDomain.CurrentDomain.BaseDirectory;

        public string MarvalMicroServiceUrl => configuration["Integrations:MarvalService"];

        public string AccessToken => configuration["AccessToken"];
    }
}
