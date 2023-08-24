using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mime;
using System.Threading.Tasks;
using TuiFly.Turnover.Api.Common.Models;

namespace TuiFly.Turnover.Api.Common.Middlewares
{
    /// <summary>
    /// A custom middleware for handling server error
    /// </summary>
    public class ServerErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ServerErrorMiddleware> _logger;

        /// <summary>
        /// The ctor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public ServerErrorMiddleware(RequestDelegate next, ILogger<ServerErrorMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var instanceError = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}";
                _logger.LogError("An error occured in : {}\n when try invoke : {}\n with details : {} ", instanceError, context.Request.Method, error);

                var response = context.Response;
                response.ContentType = MediaTypeNames.Application.Json;
                var responseModel = new ServerError(instanceError);
                response.StatusCode = StatusCodes.Status500InternalServerError;

                await response.WriteAsync(ServerErrors.ToJson(responseModel));
            }
        }
    }

    public static class ServerErrorMiddlewareExtensions
    {
        public static IApplicationBuilder UseServerErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ServerErrorMiddleware>();
        }
    }
}
