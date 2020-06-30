using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RadElement.API.AuthorizationRequirements;
using RadElement.API.Filters;
using RadElement.Core.Infrastructure;
using RadElement.Core.Services;
using RadElement.Infrastructure;
using RadElement.Service;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using AutoMapper;
using RadElement.Core.Data;

namespace RadElement.API
{   
     /// <summary>
     /// Program starts here
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
        public IWebHostEnvironment HostingEnvironment { get; }

        /// <summary>
        /// The swagger schema name
        /// </summary>
        private readonly string swaggerSchemaName = "Bearer";

        /// <summary>
        /// Called when application starts up
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="hostingEnvironment">Represents the hosting environment</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Configures the logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            var authConfig = Configuration.GetSection("AuthorizationConfig").Get<AuthorizationConfig>();
            var accountsConfig = Configuration.GetSection("AccountsConfig").Get<UserAccounts>();

            authConfig.SetKeyFilePassword(Configuration.GetSection("AuthorizationConfig")["KeyFilePassword"]);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ClockSkew = TimeSpan.Zero,
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

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
                options.EnableForHttps = true;
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(GetMimeTypesForCompression());
            });

            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            services.AddCors(o => o.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddSingleton<AuthorizationConfig>(authConfig);
            services.AddSingleton<UserAccounts>(accountsConfig);
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IConfigurationManager, ConfigurationManager>();
            services.AddTransient<IElementService, ElementService>();
            services.AddTransient<IElementSetService, ElementSetService>();
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<IOrganizationService, OrganizationService>();
            services.AddTransient<IIndexCodeService, IndexCodeService>();
            services.AddTransient<IReferenceService, ReferenceService>(); 
            services.AddSingleton<IAuthorizationHandler, UserIdExistsRequirementHandler>();
            services.AddSingleton<ILogger>(Log.Logger);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Configuration["Version"], new OpenApiInfo { Title = Configuration["Title"], Version = Configuration["Version"] });
                c.IncludeXmlComments(@"App_Data\api-comments.xml");
                c.AddSecurityDefinition(swaggerSchemaName, GetSwaggerSecurityScheme());
                c.OperationFilter<SecurityRequirementsOperationFilter>(swaggerSchemaName);
            });

            services.AddMvc().AddNewtonsoftJson();
            services.AddMvcCore().AddApiExplorer();
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.Use(async (context, next) => {
                using (LogContext.PushProperty("IPAddress", context.Connection.RemoteIpAddress))
                {
                    await next();
                }
            });

            app.UseHsts();
            app.UseHttpsRedirection();

            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseCors("AllowAllOrigins");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = Configuration["Environment:SwaggerRoutePrefix"];
                c.DocumentTitle = Configuration["Title"] + " " + Configuration["Version"];
                c.SwaggerEndpoint(Configuration["Environment:ApplicationURL"] + "/swagger/" + Configuration["Version"] + "/swagger.json", Configuration["Title"] + " " + Configuration["Version"]);
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="keyFilePath">The key file path.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        private X509SecurityKey GetKey(string keyFilePath, string password)
        {
            X509Certificate2 certificate;
            var certificatePath = HostingEnvironment.WebRootPath + keyFilePath;
            certificate = new X509Certificate2(certificatePath, password, X509KeyStorageFlags.EphemeralKeySet);

            return new X509SecurityKey(certificate);
        }

        /// <summary>
        /// Gets the swagger security scheme.
        /// </summary>
        /// <returns></returns>
        private OpenApiSecurityScheme GetSwaggerSecurityScheme()
        {
            return new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header. Example: " + "{token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "bearer",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT"
            };
        }

        /// <summary>
        /// Gets the MIME types for compression.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetMimeTypesForCompression()
        {
            return new[]
            {
                "application/json",
                "image/png",
                "image/jpeg",
                "image/gif",
                "image/tiff",
                "image/webp"
            };
        }
    }
}
