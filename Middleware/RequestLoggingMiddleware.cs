namespace CurrencyConverterAPI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var start = DateTime.UtcNow;

            // Process the request
            await _next(context);

            var clientIp = context.Connection.RemoteIpAddress?.ToString();
            var clientId = context.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value; // Assuming "sub" is used for ClientId in JWT token
            var method = request.Method;
            var endpoint = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
            var responseCode = context.Response.StatusCode;
            var responseTime = DateTime.UtcNow - start;

            _logger.LogInformation("Request Details: Client IP: {ClientIp}, ClientId: {ClientId}, HTTP Method: {Method}, Target Endpoint: {Endpoint}, Response Code: {ResponseCode}, Response Time: {ResponseTime}ms",
                clientIp, clientId, method, endpoint, responseCode, responseTime.TotalMilliseconds);
        }
    }

}
