using Microsoft.AspNetCore.Identity;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Services.Interfaces;

namespace StudyPomo.UI.Middleware;

public class OnboardingMiddleware
{
    private readonly RequestDelegate _next;

    public OnboardingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager, IUserService userService)
    {


        string onboardingPath = "/Identity/Onboarding/Index";

        string logoutPath = "/Identity/Account/Logout";
        if (context.User.Identity.IsAuthenticated)
        {
            // If user wants to log out, allow them.
            if (context.Request.Path.StartsWithSegments(new PathString(logoutPath)))
            {
                await _next(context);
                return;
            }

            var user = await userManager.GetUserAsync(context.User);
            if (!context.Request.Path.StartsWithSegments(new PathString(onboardingPath)))
            {
                if (!user.IsOnboarded)
                {
                    context.Response.Redirect(onboardingPath);
                    return;
                }
            }
        }

        await _next(context);
    }
}
