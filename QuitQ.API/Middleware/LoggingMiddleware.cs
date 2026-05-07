namespace QuitQ.API.Middleware
{
    using System.Diagnostics;

    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            Console.WriteLine($"[Request] {context.Request.Method} {context.Request.Path}");

            await _next(context);

            stopwatch.Stop();

            Console.WriteLine($"[Response] {context.Response.StatusCode} - {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
