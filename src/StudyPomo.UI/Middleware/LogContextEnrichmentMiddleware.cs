using Serilog.Context;
using Serilog.Core.Enrichers;
using Serilog.Core;

namespace StudyPomo.UI.Middleware
{
    /// <summary>
    /// Represents the log context enrichment middleware.
    /// </summary>
    public sealed class LogContextEnrichmentMiddleware
    {
        private readonly RequestDelegate _next;

        public LogContextEnrichmentMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <inheritdoc />
        public async Task InvokeAsync(HttpContext context)
        {
            using (LogContext.Push(GetEnrichers(context)))
            {
                await _next(context);
            }
        }

        /// <summary>
        /// Gets the array of enrichers for the current request.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>The array of enrichers for the current request.</returns>
        private static ILogEventEnricher[] GetEnrichers(HttpContext context)
        {
            // Retrieve the user ID, assuming it is available as a claim
            var userId = context.User.GetUserId();

            return new ILogEventEnricher[]
            {
                new PropertyEnricher("IPAddress", context.Connection.RemoteIpAddress),
                new PropertyEnricher("RequestHost", context.Request.Host),
                new PropertyEnricher("RequestPathBase", context.Request.PathBase),
                new PropertyEnricher("RequestQueryParams", context.Request.QueryString),
                new PropertyEnricher("UserId", userId)
            };
        }
    }
}
