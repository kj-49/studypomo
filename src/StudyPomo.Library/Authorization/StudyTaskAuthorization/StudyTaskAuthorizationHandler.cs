using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using StudyPomo.Library.Services.Interfaces;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.CourseEntities;

namespace StudyPomo.Library.Authorization.StudyTaskAuthorization;

public class StudyTaskAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, StudyTask>
{
    private readonly IUserService _userService;
    private readonly ICourseService _courseService;

    public StudyTaskAuthorizationHandler(IUserService userService, ICourseService courseService)
    {
        _userService = userService;
        _courseService = courseService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, StudyTask studyTask)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        if (user == null)
        {
            return;
        }

        // Requirement for all operation types
        if (user.Id == studyTask.UserId)
        {
            if (requirement.Name == Operations.Create.Name || requirement.Name == Operations.Update.Name)
            {
                // Here we need to ensure that the StudyTask.CourseId property is owned by the user
                if (studyTask.CourseId == null)
                {
                    context.Succeed(requirement);

                } else
                {
                    Course course = await _courseService.GetAsync(studyTask.CourseId.Value);

                    // If user owns the course, then they can create or update the study task
                    if (course.UserId == user.Id)
                    {
                        context.Succeed(requirement);
                    }
                }
            }
            else if (requirement.Name == Operations.Read.Name)
            {
                context.Succeed(requirement);
            }
            else if (requirement.Name == Operations.Delete.Name)
            {
                context.Succeed(requirement);
            }
        }

        return;
    }
}
