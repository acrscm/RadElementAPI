using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.Assist.Marval.Core.Data;
using Acr.Assist.Marval.Core.DTO;
using Acr.Assist.Marval.Core.Infrastructure;
using Acr.Assist.Marval.Core.Services;
using Acr.Assist.Marval.Core.Validators;
using Acr.Assist.Marval.Data;
using Acr.Assist.Marval.Infrastructure;
using Acr.Assist.Marval.Service;
using Acr.Assist.Marval.Service.Validator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Serilog;

namespace RadElementApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

                 
          

        
            services.AddMvcCore().AddApiExplorer();

            services.AddCors(o => o.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            //Configure logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IConfigurationManager, ConfigurationManager>();
            services.AddTransient<IModuleRepository, ModuleRepository>();
            services.AddTransient<IUserModuleRepository, UserModuleRepository>();
            services.AddTransient<IUserModuleService, UserModuleService>();
            
            services.AddTransient<IModuleService, ModuleService>();
            services.AddTransient<IDataValidator<UserIdDetails>, UserIdValidator>();
            services.AddTransient<IDataValidator<UserModule>, UserModuleValidator>();
            services.AddTransient<IModuleTestCaseRepository, ModuleTestRepository>();
            services.AddTransient<IDataValidator<ModuleIdDetails>, ModuleIdValidator>();
            services.AddTransient<IDataValidator<UserRoleIdDetails>, UserRoleIdDetailsValidator>();
            services.AddTransient<IDataValidator<AddModule>, AddModuleValidator>();
            services.AddSingleton<Serilog.ILogger>(Log.Logger);

            services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();
            services.AddTransient<IReSTRequestHandler, ReSTRequestHandler>();

         
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAllOrigins");
            app.UseMvc();

        }
    }
}
