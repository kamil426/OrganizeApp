using OrganizeApp.Shared.Common.Models;
using System.Net;

namespace OrganizeApp.WebApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(context, ex).ConfigureAwait(false);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (exception is Application.Common.Exceptions.ValidationException)
            {
                var ex = exception as Application.Common.Exceptions.ValidationException;

                var message = string.Empty;

                foreach (var item in ex.Errors)
                {
                    message += string.Join(" ", item.Value);
                }

                if (string.IsNullOrWhiteSpace(message))
                {
                    message = exception.Message;
                }

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return context.Response.WriteAsJsonAsync(new ResponseDto { Errors = message });
            }

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsJsonAsync(new ResponseDto { Errors = exception.Message });
        }
    }
}
