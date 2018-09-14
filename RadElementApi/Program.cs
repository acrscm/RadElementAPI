using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace RadElementApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = context.HostingEnvironment;

                    builder.AddJsonFile("appsettings.json",
                                 optional: true, reloadOnChange: true)
                           .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                                 optional: true, reloadOnChange: true);

                    builder.AddEnvironmentVariables();
                })
                .UseSerilog()
                .Build();
    }
}
