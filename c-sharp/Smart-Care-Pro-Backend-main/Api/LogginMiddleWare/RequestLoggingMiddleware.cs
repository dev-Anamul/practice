using System.Text;

namespace Api.LogginMiddleWare
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

        public async Task Invoke(HttpContext context)
        {
            var requestBody = await ReadRequestBody(context.Request);

            _logger.LogInformation(
                "Request {Method} {Path} => {RequestBody}",
                context.Request.Method,
                context.Request.Path,
                requestBody);

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                var responseBodyText = await ReadResponseBody(responseBody);

                _logger.LogInformation(
                    "Response {StatusCode} => {ResponseBody}",
                    context.Response.StatusCode,
                    responseBodyText);

                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private static async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();

            using var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            request.Body.Seek(0, SeekOrigin.Begin);

            return body;
        }

        private static async Task<string> ReadResponseBody(Stream responseBody)
        {
            responseBody.Seek(0, SeekOrigin.Begin);

            using var reader = new StreamReader(responseBody, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            responseBody.Seek(0, SeekOrigin.Begin);

            return body;
        }
    }
}
