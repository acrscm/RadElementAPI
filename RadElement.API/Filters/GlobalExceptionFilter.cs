using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RadElement.API.Models;
using RadElement.Core.Services.Exception;

namespace RadElement.API.Filters
{

    /// <summary>
    /// Handles all the exceptions
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> logger;
        
        /// <summary>
        /// Ini
        /// </summary>
        /// <param name="logger"></param>
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Method is called when an exception occurs
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            ErrorResponse res = null;
            var exceptionType = context.Exception.GetType();
            var exception = context.Exception;
            if (exceptionType == typeof(InputValidationFailureException))
            {
                var inputValidationFailureException = (exception as InputValidationFailureException);
                status = HttpStatusCode.BadRequest;
                res = new ErrorResponse(inputValidationFailureException.Message, inputValidationFailureException.Errors);
                logger.LogError(exception, exception.Message);
            }
            else if (exceptionType == typeof(ResourceNotFoundException))
            {
                status = HttpStatusCode.NotFound;
                res = new ErrorResponse(exception.Message);
                logger.LogError(exception, exception.Message);

            }
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                status = HttpStatusCode.Forbidden;
                res = new ErrorResponse(exception.Message);
                logger.LogError(exception, exception.Message);

            }
            else
            {
                logger.LogCritical(exception, exception.Message);
                status = HttpStatusCode.InternalServerError;
            }
            context.ExceptionHandled = true;
            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.ContentType = "application/json";
            var errorResponse = (res == null) ? string.Empty : JsonConvert.SerializeObject(res);
            response.WriteAsync(errorResponse);
        }
    }
}
