using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using RadElement.API.Filters;
using RadElement.Core.Data;
using RadElement.Core.Infrastructure;
using RadElement.Core.Services;
using RadElement.Data;
using RadElement.Infrastructure;
using RadElement.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using RadElement.API.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RadElement.API
{
    /// <summary>
    /// Class called when the application starts
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Returns the configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Represents the hosting environment
        /// </summary>
        public IHostingEnvironment HostingEnvironment { get; }

        /// <summary>
        /// Called when application starts up
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="hostingEnvironment">Represents the hosting environment</param>
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        private X509SecurityKey GetKey(string keyFilePath, string password)
        {
            X509Certificate2 certificate;
            var certificatePath = HostingEnvironment.WebRootPath + keyFilePath;
            certificate = new X509Certificate2(certificatePath, password);
           
            return new X509SecurityKey(certificate);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var authConfig = Configuration.GetSection("AuthorizationConfig").Get<AuthorizationConfig>();
            var accountsConfig = Configuration.GetSection("AccountsConfig").Get<UserAccounts>();

            authConfig.SetKeyFilePassword(Configuration.GetSection("AuthorizationConfig")["KeyFilePassword"]);
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = authConfig.Issuer,
                        ValidAudience = authConfig.Audience,
                        IssuerSigningKey = GetKey(authConfig.KeyFilePath, authConfig.ConvertToUnsecureString(authConfig.SigningPassword))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserIdExists",
                    policy => policy.Requirements.Add(new UserIdRequirement("UserId")));
            });

            services.AddMvc(
                 config =>
                 {
                     config.Filters.Add(typeof(GlobalExceptionFilter));
                     config.Filters.Add(new RequireHttpsAttribute());
                 }
            );

            services.AddDbContext<RadElementDbContext>(options => options.UseMySql(Configuration.GetConnectionString("Database")));

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
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
            services.AddSingleton<AuthorizationConfig>(authConfig);
            services.AddSingleton<UserAccounts>(accountsConfig);
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IConfigurationManager, ConfigurationManager>();
            services.AddTransient<IElementService, ElementService>();
            services.AddTransient<IElementSetService, ElementSetService>();
            services.AddTransient<IModuleService, ModuleService>();
            services.AddTransient<IRadElementDbContext, RadElementDbContext>();
            services.AddSingleton<IAuthorizationHandler, UserIdExistsRequirementHandler>();
            services.AddSingleton<ILogger>(Log.Logger);
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Configuration["Version"], new Info { Title = Configuration["Title"], Version = Configuration["Version"] });
                c.IncludeXmlComments(@"App_Data\api-comments.xml");
                c.OperationFilter<AuthorizationHeaderOperationFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                await next();
            });

            app.UseHsts();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseStaticFiles();
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
