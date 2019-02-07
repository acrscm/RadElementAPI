using System;
using System.Collections.Generic;
using System.Text;

namespace RadElement.Core.Infrastructure
{
    /// <summary>
    /// Interface for managing configuration related information
    /// </summary>
    public interface IConfigurationManager
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        string ConnectionString { get; }

        /// <summary>
        /// Get Application Title
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Get Applciation Version
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Get Applciation URL
        /// </summary>
        string ApplicationURL { get; }

        /// <summary>
        /// Get Route to Access Swagger
        /// </summary>
        string SwaggerRoutePrefix { get; }

        /// <summary>
        /// Get Root Directory path
        /// </summary>
        string RootPath { get; }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        string AccessToken { get; }
    }
}
