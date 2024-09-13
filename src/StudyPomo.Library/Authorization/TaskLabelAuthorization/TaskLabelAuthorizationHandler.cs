using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.LabelEntities;
using StudyPomo.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Authorization.LabelRequirements;

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
