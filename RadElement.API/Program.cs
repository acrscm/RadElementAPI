using Microsoft.AspNetCore.Hosting;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace RadElement.API
{
    /// <summary>
    /// Entry point for the application
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point method
        /// </summary>
        /// <param name="args">Represents the command line arguments</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates the web host
        /// </summary>
        /// <param name="args">Represents the command line arguments</param>
        /// <returns>TReturns the web host</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
               Host.CreateDefaultBuilder(args)
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       webBuilder.UseStartup<Startup>()
                       .ConfigureAppConfiguration((context, builder) =>
                       {
                           var env = context.HostingEnvironment;

                           builder.AddJsonFile("appsettings.json",
                                        optional: true, reloadOnChange: true)
                                  .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                                        optional: true, reloadOnChange: true);
                       }).UseSerilog();
                   });
    }
}
