using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using NuGet.Protocol.Core.Types;
using Pomodoro.Library.Models.Identity;
using Pomodoro.Library.Models.Tables.CourseEntities;
using Pomodoro.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Authorization.CourseAuthorization;

public class CourseAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Course>
{
    private readonly IUserService _userService;

    public CourseAuthorizationHandler(IUserService userService)
    {
        _userService = userService;
    }

    protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Course course)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        if (user == null)
        {
            return;
        }

        if (user.Id == course.UserId)
        {
            context.Succeed(requirement);
        }

        return;
    }
}
