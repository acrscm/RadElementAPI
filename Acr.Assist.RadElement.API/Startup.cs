using System;
using Acr.Assist.RadElement.Core.Data;
using Acr.Assist.RadElement.Core.Infrastructure;
using Acr.Assist.RadElement.Core.Integrations;
using Acr.Assist.RadElement.Core.Services;
using Acr.Assist.RadElement.Data;
using Acr.Assist.RadElement.Infrastructure;
using Acr.Assist.RadElement.Integrations;
using Acr.Assist.RadElement.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Acr.Assist.RadElement.API
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
            services.AddMvc().AddXmlSerializerFormatters();
            services.AddDbContext<RadElementDbContext>(options => options.UseMySql(Configuration.GetConnectionString("Database")));
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });
            services.AddMvcCore().AddApiExplorer();
            services.AddCors(o => o.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IConfigurationManager, ConfigurationManager>();
            services.AddTransient<IRadElementService, RadElementService>();
            services.AddTransient<IRadElementDbContext, RadElementDbContext>();
            services.AddSingleton<ILogger>(Log.Logger);

            services.AddTransient<IMarvalMicroService, MarvalMicroService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Configuration["Version"], new Swashbuckle.AspNetCore.Swagger.Info { Title = Configuration["Title"], Version = Configuration["Version"] });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseCors("AllowAllOrigins");
            app.UseMvc();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = Configuration["Environment:SwaggerRoutePrefix"];
                c.DocumentTitle = Configuration["Title"] + " " + Configuration["Version"];
                c.SwaggerEndpoint(Configuration["Environment:ApplicationURL"] + "/swagger/" + Configuration["Version"] + "/swagger.json", Configuration["Title"] + " " + Configuration["Version"]);
            });
        }
    }
}
