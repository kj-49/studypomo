using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Pomodoro.Library.Models.Identity;
using Pomodoro.Library.Models.Tables.LabelEntities;
using Pomodoro.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Authorization.LabelRequirements;

public class TaskLabelAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, TaskLabel>
{
    private readonly IUserService _userService;

    public TaskLabelAuthorizationHandler(IUserService userService)
    {
        _userService = userService;
    }

    protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, TaskLabel taskLabel)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        if (user == null)
        {
            return;
        }

        if (user.Id == taskLabel.UserId)
        {
            context.Succeed(requirement);
        }

        return;
    }
}
