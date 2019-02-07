using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RadElement.API.Filters
{
    /// <summary>
    /// Adds the authorization header for swager documentation
    /// </summary>
    /// <seealso cref="Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter" />
    public class AuthorizationHeaderOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="context">The context.</param>
        public void Apply(Swashbuckle.AspNetCore.Swagger.Operation operation, OperationFilterContext context)
        {
            IList<Microsoft.AspNetCore.Mvc.Filters.FilterDescriptor> filterDescriptors = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            bool isAuthorized = filterDescriptors.Select(filterInfo => filterInfo.Filter).Any(filter => filter is AuthorizeFilter);
            bool allowAnonymous = filterDescriptors.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);

            if (isAuthorized && !allowAnonymous)
            {
                if (operation.Parameters == null)
                {
                    operation.Parameters = new List<IParameter>();
                }
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Authorization",
                    In = "header",
                    Description = "access token",
                    Required = true,
                    Type = "string",
                    Default = "Bearer "
                });
            }
        }
    }
}

