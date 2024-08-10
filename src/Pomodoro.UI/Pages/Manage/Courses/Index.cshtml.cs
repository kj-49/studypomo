using Htmx;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pomodoro.Library.Authorization;
using Pomodoro.Library.Models.Identity;
using Pomodoro.Library.Models.Tables.CourseEntities;
using Pomodoro.Library.Models.Tables.LabelEntities;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using Pomodoro.Library.Models.Tables.TaskPriorityEntities;
using Pomodoro.Library.Services.Interfaces;

namespace Pomodoro.UI.Pages.Manage.Courses;

public class IndexModel : PageModel
{
    private readonly ICourseService _courseService;
    private readonly ITaskPriorityService _taskPriorityService;
    private readonly ITaskLabelService _taskLabelService;
    private readonly IUserService _userService;
    private readonly IStudyTaskService _studyTaskService;
    private readonly IAuthorizationService _authorizationService;

    public IndexModel(ICourseService courseService, ITaskPriorityService taskPriorityService, ITaskLabelService taskLabelService, IUserService userService, IStudyTaskService studyTaskService, IAuthorizationService authorizationService)
    {
        _courseService = courseService;
        _taskPriorityService = taskPriorityService;
        _taskLabelService = taskLabelService;
        _userService = userService;
        _studyTaskService = studyTaskService;
        _authorizationService = authorizationService;
    }

    public Course Course { get; set; }

    [BindProperty]
    public StudyTaskCreate StudyTaskCreate { get; set; }
    public ICollection<TaskPriority> TaskPriorities { get; set; }
    public ICollection<TaskLabel> TaskLabels { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        if (user == null) return Challenge();
        
        Course course = await _courseService.GetAsync(id);

        var authResult = await _authorizationService.AuthorizeAsync(User, course, Operations.Read);

        if (!authResult.Succeeded)
        {
            if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }
        }

        await PopulateFields(userId: user.Id, courseId: id);

        return Page();
    }

    public async Task PopulateFields(int userId, int courseId)
    {
        Course = await _courseService.GetAsync(courseId);
        TaskPriorities = await _taskPriorityService.GetAllAsync();
        TaskLabels = await _taskLabelService.GetAllAsync(userId);
    }

    public async Task<IActionResult> OnPostCreateStudyTaskAsync()
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) return Challenge();

        // Need to ensure the created task is for a CourseId that the current user owns.
        StudyTask studyTask = StudyTaskCreate.ToEntity(user.Id);
        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Create);

        if (!authResult.Succeeded)
        {
            if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }
        }

        await _studyTaskService.CreateAsync(StudyTaskCreate);

        return RedirectToPage(new { id = StudyTaskCreate.CourseId });
    }

    public async Task<IActionResult> OnPostUpdateStudyTaskAsync(StudyTaskUpdate studyTaskUpdate)
    {
        StudyTask studyTask = await _studyTaskService.GetAsync(studyTaskUpdate.Id);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Update);

        if (!authResult.Succeeded)
        {
            if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }
        }

        await _studyTaskService.UpdateAsync(studyTaskUpdate);

        return RedirectToPage(new { id = studyTaskUpdate.CourseId });
    }

    public async Task<IActionResult> OnPostCompleteTaskAsync(int studyTaskId, int courseId)
    {
        StudyTask studyTask = await _studyTaskService.GetAsync(studyTaskId);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Update);

        if (!authResult.Succeeded)
        {
            if (User.Identity.IsAuthenticated)
            {
                // TODO: Show status message.
                return Content("");
            }
            else
            {
                // TODO: Show status message.
                return Content("");
            }
        }

        if (Request.IsHtmx())
        {
            await _studyTaskService.CompleteAsync(studyTaskId);

            ApplicationUser? user = await _userService.GetCurrentUserAsync();

            if (user == null) return Challenge();

            await PopulateFields(userId: user.Id, courseId: courseId);

            return Partial("Partials/_Tasks", this);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostUncompleteTaskAsync(int studyTaskId, int courseId)
    {
        StudyTask studyTask = await _studyTaskService.GetAsync(studyTaskId);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Update);

        if (!authResult.Succeeded)
        {
            if (User.Identity.IsAuthenticated)
            {
                // TODO: Show status message.
                return Content("");
            }
            else
            {
                // TODO: Show status message.
                return Content("");
            }
        }

        if (Request.IsHtmx())
        {
            await _studyTaskService.UncompleteAsync(studyTaskId);

            ApplicationUser? user = await _userService.GetCurrentUserAsync();

            if (user == null) return Challenge();

            await PopulateFields(userId: user.Id, courseId: courseId);

            return Partial("Partials/_Tasks", this);
        }

        return Page();

    }

    public async Task<IActionResult> OnPostArchiveStudyTaskAsync(int studyTaskId, int courseId)
    {
        StudyTask studyTask = await _studyTaskService.GetAsync(studyTaskId);

        var authResult = await _authorizationService.AuthorizeAsync(User, studyTask, Operations.Update);

        if (!authResult.Succeeded)
        {
            if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }
        }

        await _studyTaskService.ArchiveAsync(studyTaskId);

        return RedirectToPage(new { id = courseId });
    }

}
