using Core_API_ConditionalMiddleware.Models;
using System.Text;

namespace Core_API_ConditionalMiddleware.Middlewares
{
    /// <summary>
    /// This middleware will log the request details to SQL Server Database
    /// </summary>
    public class RequestLoggerMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        private readonly WowdbContext _context;

        public async Task InvokeAsync(HttpContext context, WowdbContext dbContext)
        {
            context.Request.EnableBuffering(); // Enable buffering to allow multiple reads

            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024,
                leaveOpen: true))
            {
                // The body 
                var body = await reader.ReadToEndAsync();

                // If the body id empty (In case of the Delete request)
                if(String.IsNullOrEmpty(body))
                    body = "No Request Body, te record is requested for delete";
                // Process the body
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    // Log the request details to SQL Server Database
                    var request = new RequestLogger
                    {
                        RequestId = Guid.NewGuid().ToString(),
                        RequestMethod = context.Request.Method,
                        RequestPath = context.Request.Path,
                        RequestDateTime = DateTime.UtcNow,
                        RequestBody = body
                    };

                    await dbContext.RequestLoggers.AddAsync(request);
                    await dbContext.SaveChangesAsync();
                }
                 
                    // Reset the request body stream position so the next middleware can read it
                    context.Request.Body.Position = 0;
            }

            await _next(context);
        }
    }
}
