using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using StudyPomo.Library.Authorization.CourseAuthorization;
using StudyPomo.Library.Authorization.LabelRequirements;
using StudyPomo.Library.Authorization.StudyTaskAuthorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Authorization;

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
