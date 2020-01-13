using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RadElement.API.Filters
{
    /// <summary>
    /// Provides the filter for authorization headers
    /// </summary>
    /// <seealso cref="Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter" />
    public class AuthorizationHeaderOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="context">The context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
             .Union(context.MethodInfo.GetCustomAttributes(true))
             .OfType<AuthorizeAttribute>();

            if (authAttributes.Any())
            {
                if (operation.Parameters != null)
                {
                    operation.Parameters.Add(new OpenApiParameter()
                    {
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Description = "JWT Authorization header. Example: " + "Bearer {token}",
                        Required = true,
                        Schema = new OpenApiSchema
                        {
                            Type = "string",
                            Default = new OpenApiString("Bearer ")
                        }
                    });
                }
            }
        }
    }
}
