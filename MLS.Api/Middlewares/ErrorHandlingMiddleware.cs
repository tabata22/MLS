using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MLS.Service.Common;
using Newtonsoft.Json;

namespace MLS.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError($"Error Message: {ex.Message}, Stack Trace: {ex.StackTrace}, Source: {ex.Source}");

            context.Response.Clear();
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var result = JsonConvert.SerializeObject(new ErrorDetails { Message = ex.Message, Source = ex.Source}, Formatting.None);

            return context.Response.WriteAsync(result, Encoding.UTF8);
        }
    }
}
