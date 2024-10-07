using Serilog.Context;
using Serilog.Core.Enrichers;
using Serilog.Core;
using Microsoft.AspNetCore.Identity;
using StudyPomo.Library.Models.Identity;

namespace StudyPomo.UI.Middleware
{
    /// <summary>
    /// Represents the log context enrichment middleware.
    /// </summary>
    public class LogContextEnrichmentMiddleware : IMiddleware
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public LogContextEnrichmentMiddleware(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var userId = _userManager.GetUserId(context.User);


            LogContext.Push(new ILogEventEnricher[]
            {
                new PropertyEnricher("IPAddress", context.Connection.RemoteIpAddress),
                new PropertyEnricher("RequestHost", context.Request.Host),
                new PropertyEnricher("RequestPathBase", context.Request.PathBase),
                new PropertyEnricher("RequestQueryParams", context.Request.QueryString),
                new PropertyEnricher("UserId", userId),
            });
   
            await next(context);
        }

    }
}
