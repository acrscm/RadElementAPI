using MySql.Data.MySqlClient;
using RadElement.Core.Infrastructure;
using System.Data;

namespace RadElement.Data
{
    public class BaseRepository
    {
        /// <summary>
        /// The configuration manager
        /// </summary>
        private readonly IConfigurationManager configurationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        public BaseRepository(IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }
        public IDbConnection GetConnection()
        {
            var csb = new MySqlConnectionStringBuilder(configurationManager.ConnectionString);
            return new MySqlConnection(csb.ConnectionString);
        }
    }
}
