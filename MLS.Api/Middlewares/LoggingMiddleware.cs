using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MLS.Api.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
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
            finally
            {
                var request = context.Request;
                var logData = "Http {0}" + $" Information: Scheme: {request.Scheme}, Host: {request.Host}, Path: {request.Path}, QueryString: {request.QueryString}";

                _logger.LogInformation(logData, "Request");
                _logger.LogInformation(logData, "Response");
            }
        }
    }
}
