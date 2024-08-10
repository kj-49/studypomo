using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Pomodoro.Library.Authorization.CourseAuthorization;
using Pomodoro.Library.Authorization.LabelRequirements;
using Pomodoro.Library.Authorization.StudyTaskAuthorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationHandlers(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationHandler, TaskLabelAuthorizationHandler>();
        services.AddScoped<IAuthorizationHandler, CourseAuthorizationHandler>();
        services.AddScoped<IAuthorizationHandler, StudyTaskAuthorizationHandler>();

        return services;
    }
}
