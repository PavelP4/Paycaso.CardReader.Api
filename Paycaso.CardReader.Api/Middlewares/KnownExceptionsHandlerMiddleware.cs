using System.Net;

namespace Paycaso.CardReader.Api.Middlewares
{
    class KnownExceptionsHandlerMiddleware
    {
        private readonly ILogger<KnownExceptionsHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public KnownExceptionsHandlerMiddleware(RequestDelegate next, ILogger<KnownExceptionsHandlerMiddleware> logger)
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
            catch (OperationCanceledException operationCanceledException)
            {
                var message = "Operation was cancelled. Message: " + operationCanceledException.Message;
                _logger.LogWarning(operationCanceledException, message);
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(message);
            }
            catch (InvalidDataException invalidDataException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(invalidDataException.Message);
            }
        }
    }
}
